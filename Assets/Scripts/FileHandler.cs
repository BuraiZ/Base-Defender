using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System;
using System.Linq;

public class FileHandler {
    public static string[] ReadString(TextAsset data) {
        //string pattern = ";";
        //string[] elements = Regex.Split(data.text, pattern, RegexOptions.Singleline);
        string tempText = data.text;
        tempText = Regex.Replace(tempText, System.Environment.NewLine, string.Empty);

        char[] pattern = { ';' };
        string[] elements = tempText.Split(pattern, StringSplitOptions.RemoveEmptyEntries);
        return elements;
    }

}