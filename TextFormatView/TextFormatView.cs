#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFormatView : MonoBehaviour
{
    [Multiline] [SerializeField] private string format;
    [SerializeField] private Text textView;

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
        if (textView != null)
        {
            textView.text = s;
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (textView == null)
        {
            UnityEditor.Undo.RecordObject(this, "Get Text Component");
            textView = GetComponent<Text>();
        }
    }
#endif
}
