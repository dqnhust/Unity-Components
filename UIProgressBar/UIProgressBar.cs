using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIProgressBar : MonoBehaviour
{
    [SerializeField] private RectTransform outer;
    [SerializeField] private RectTransform inner;
    [SerializeField] private Padding padding;
    [Range(0, 1)]
    [SerializeField] private float currentPercent = 0;

    private bool inited = false;
    private float MaxWidth
    {
        get
        {
            return outer.rect.width - padding.left - padding.right;
        }
    }

    private void Init()
    {
        inited = true;
        inner.anchorMin = Vector2.zero;
        inner.anchorMax = Vector2.one;
    }

    public void SetPercent(float percent)
    {
        if (!inited)
        {
            Init();
        }
        currentPercent = percent;
        UpdateView();
    }

    private void UpdateView()
    {
        var width = MaxWidth * Mathf.Clamp01(currentPercent);
        var spaceRight = MaxWidth - width;
        inner.offsetMin = new Vector2(padding.left, padding.bottom);
        inner.offsetMax = new Vector2(-(padding.right + spaceRight), -padding.top);
    }

    private void OnValidate()
    {
        UpdateView();
    }

    [System.Serializable]
    public class Padding
    {
        public float left;
        public float top;
        public float right;
        public float bottom;
    }

}
