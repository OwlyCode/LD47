using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Line
{
    public readonly string content;
    public readonly string color;

    public Line(string content, string color)
    {
        this.content = content;
        this.color = color;
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
        this.delay = 0.025f;
        characterIndex = 0;
        this.onComplete = onComplete;
        oldLines = "";
    }

    private void Update()
    {
        if (text == null || lines == null) {
            return;
        }

        string content = lines[0].content;

        timer -= Time.deltaTime;
        while (timer < 0f) {
            timer += delay;

            text.text = oldLines + "<color="+lines[0].color+">" + content.Substring(0, characterIndex) + "</color>";
            characterIndex++;

            if (characterIndex >= content.Length) {
                lines.RemoveAt(0);
                oldLines = text.text + "\n";
                characterIndex = 0;
                if (lines.Count == 0) {
                    text = null;
                    onComplete();
                    return;
                }
            }
        } 
    }
}
