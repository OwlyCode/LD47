using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Line
{
    public readonly string content;
    public readonly string color;

    public readonly AudioSource voice;

    public Line(string content, string color, AudioSource voice)
    {
        this.content = content;
        this.color = color;
        this.voice = voice;
    }
}

public class TextWriter : MonoBehaviour
{
    private Text text;
    private float delay;
    private float timer;
    private int characterIndex;

    private Action onComplete;

    private List<Line> lines;

    private string oldLines;

    public void Write(Text text, List<Line> lines, Action onComplete)
    {
        this.text = text;
        this.lines = lines;
        this.delay = 0.05f;
        characterIndex = 0;
        this.onComplete = onComplete;
        oldLines = "";
    }

    private void Update()
    {
        if (text == null || lines == null) {
            return;
        }

        Line line = lines[0];

        timer -= Time.deltaTime;
        while (timer < 0f) {
            timer += delay;

            if (characterIndex == 0 && lines[0].voice != null) {
                line.voice.loop = true;
                line.voice.playOnAwake = true;
                line.voice.Play();
            }

            text.text = oldLines + "<color="+line.color+">" + line.content.Substring(0, characterIndex) + "_</color>";
            characterIndex++;

            if (characterIndex >= line.content.Length) {
                if (line.voice != null) {
                    line.voice.Stop();
                }
                timer += delay * 10;
                lines.RemoveAt(0);
                oldLines = text.text.Replace("_", "") + "\n";
                characterIndex = 0;
                if (lines.Count == 0) {
                    text.text = oldLines;
                    text = null;
                    onComplete();
                    return;
                }
            }
        } 
    }
}
