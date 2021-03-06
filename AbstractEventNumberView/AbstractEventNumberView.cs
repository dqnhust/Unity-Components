﻿#pragma warning disable 0649
using UnityEngine;

public abstract class AbstractEventNumberView<T> : MonoBehaviour
{
    [SerializeField] protected TextFormatView textView;

    protected abstract EventNumber<T> Data
    {
        get;
    }

    protected virtual string StringValue => Data.Value.ToString();

    protected virtual void OnEnable()
    {
        Data.OnValueChanged += UpdateUI;
        UpdateUI();
    }

    protected virtual void OnDisable()
    {
        Data.OnValueChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        textView.SetString(StringValue);
    }
}