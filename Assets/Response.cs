using System;
using UnityEngine;

[Serializable]
public class Response
{
    public string text;
    [Multiline]
    public string reaction;
    public Question nextQuestion;
}