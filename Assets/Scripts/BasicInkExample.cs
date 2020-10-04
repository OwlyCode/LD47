using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine.SceneManagement;

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
	public AudioSource resetSound;

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

			if (lineContent.StartsWith("[GAME END]")) {
				StartCoroutine("QuitLoop");
				return;
			}

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

			lines.Add(new Line(lineContent.TrimStart(), color, voice));
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
		resetSound.Play();
		RemoveChildren();
		Camera.main.backgroundColor = new Color(0.5f, 0.5f, 0.5f);
		yield return new WaitForSeconds(1.5f);
		Camera.main.backgroundColor = Color.black;
		story.ChooseChoiceIndex (0);
		RefreshView();
	}

	IEnumerator QuitLoop()
	{
		resetSound.Play();
		RemoveChildren();
		Camera.main.backgroundColor = new Color(0.5f, 0.5f, 0.5f);
		yield return new WaitForSeconds(1.5f);
		Camera.main.backgroundColor = Color.black;
        SceneManager.LoadScene("Intro", LoadSceneMode.Single);
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
