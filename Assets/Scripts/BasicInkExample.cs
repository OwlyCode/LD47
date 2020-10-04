using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
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
	
	public AudioSource teacherVoice;
	public AudioSource observerVoice;
	public AudioSource violetVoice;

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

		bool willRestart = false;

		while (story.canContinue) {
			string lineContent = story.Continue();
			string color = "#ffffff";
			AudioSource voice = null;

			willRestart = willRestart || lineContent.StartsWith("[DEATH BLUR]");

			lineContent = lineContent.Replace("[DEATH BLUR]", "");

			if (lineContent.StartsWith("TEACHER >")) {
				color = "#00ff00";
				voice = teacherVoice;
				lineContent = lineContent.Replace("TEACHER >", "");
			}

			if (lineContent.StartsWith("OBSERVER >")) {
				color = "#FFFF00";
				voice = observerVoice;
				lineContent = lineContent.Replace("OBSERVER >", "");
			}
	
			if (lineContent.StartsWith("VIOLET >")) {
				color = "#EE82EE";
				voice = violetVoice;
				lineContent = lineContent.Replace("VIOLET >", "");
			}

			lines.Add(new Line(lineContent, color, voice));
		}
		
		CreateContentView(lines, willRestart);
	}

	void OnClickChoiceButton (Choice choice) {
		story.ChooseChoiceIndex (choice.index);
		RefreshView();
	}

	void CreateContentView (List<Line> lines, bool willRestart) {
		Text storyText = Instantiate (textPrefab) as Text;

		GetComponent<TextWriter>().Write(storyText, lines, () => {
			if(story.currentChoices.Count > 0 && !willRestart) {
				for (int i = 0; i < story.currentChoices.Count; i++) {
					Choice choice = story.currentChoices [i];
					Button button = CreateChoiceView (choice.text.Trim ());
					button.onClick.AddListener (delegate {
						OnClickChoiceButton (choice);
					});
				}
			}
			else {
				StartCoroutine("RestartLoop");
			}
		});


		//storyText.text = text;
		storyText.transform.SetParent (canvas.transform, false);
	}

	IEnumerator RestartLoop()
	{
		yield return new WaitForSeconds(3f);
		RemoveChildren();
		yield return new WaitForSeconds(1f);
		story.ChooseChoiceIndex (0);
		RefreshView();
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
