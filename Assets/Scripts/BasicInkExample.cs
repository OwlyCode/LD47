using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Ink.Runtime;

public class BasicInkExample : MonoBehaviour {
	[SerializeField]
	private TextAsset inkJSONAsset = null;

	public Story story;

	[SerializeField]
	private Canvas canvas = null;

	// UI Prefabs
	[SerializeField]
	private Text textPrefab = null;

	[SerializeField]
	private Button buttonPrefab = null;

    public static event Action<Story> OnCreateStory;
	
	public AudioSource trainerVoice;

    void Awake () {
		StartStory();
	}

	void StartStory () {
		story = new Story (inkJSONAsset.text);
        if(OnCreateStory != null) OnCreateStory(story);
		RefreshView();
	}

	void RefreshView () {
		RemoveChildren ();
		
		List<Line> lines = new List<Line>();

		while (story.canContinue) {
			string lineContent = story.Continue();
			string color = "#ffffff";

			if (lineContent.StartsWith("VOICE >") || lineContent.StartsWith("TRAINER >")) {
				color = "#00ff00";
			}

			if (lineContent.StartsWith("DISTANT VOICE >") || lineContent.StartsWith("OBSERVER >")) {
				color = "#FFFF00";
			}
	
			if (lineContent.StartsWith("OTHER VOICE >") || lineContent.StartsWith("VIOLET >")) {
				color = "#EE82EE";
			}

			lines.Add(new Line(lineContent, color));
		}
		
		CreateContentView(lines);
	}

	void OnClickChoiceButton (Choice choice) {
		story.ChooseChoiceIndex (choice.index);
		RefreshView();
	}

	void CreateContentView (List<Line> lines) {
		Text storyText = Instantiate (textPrefab) as Text;

		trainerVoice.Play();
		GetComponent<TextWriter>().Write(storyText, lines, () => {
			trainerVoice.Stop();

			if(story.currentChoices.Count > 0) {
				for (int i = 0; i < story.currentChoices.Count; i++) {
					Choice choice = story.currentChoices [i];
					Button button = CreateChoiceView (choice.text.Trim ());
					button.onClick.AddListener (delegate {
						OnClickChoiceButton (choice);
					});
				}
			}
			else {
				Button choice = CreateChoiceView("End of story.\nRestart?");
				choice.onClick.AddListener(delegate{
					StartStory();
				});
			}
		});


		//storyText.text = text;
		storyText.transform.SetParent (canvas.transform, false);
	}

	Button CreateChoiceView (string text) {
		Button choice = Instantiate (buttonPrefab) as Button;
		choice.transform.SetParent (canvas.transform, false);
		
		Text choiceText = choice.GetComponentInChildren<Text> ();
		choiceText.text = text;

		HorizontalLayoutGroup layoutGroup = choice.GetComponent <HorizontalLayoutGroup> ();
		layoutGroup.childForceExpandHeight = false;

		return choice;
	}

	void RemoveChildren () {
		int childCount = canvas.transform.childCount;
		for (int i = childCount - 1; i >= 0; --i) {
			GameObject.Destroy (canvas.transform.GetChild (i).gameObject);
		}
	}
}
