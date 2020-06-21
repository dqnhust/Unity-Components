#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFormatView : MonoBehaviour
{
    [Multiline][SerializeField] private string format;

    public void SetString(params string[] inputs)
    {
        string s = format;
        for (int i = 0; i < inputs.Length; i++)
        {
            var rep = "{" + i + "}";
            if (s.Contains(rep))
            {
                s = s.Replace(rep, inputs[i]);
            }
        }
    }
}
