using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSView : MonoBehaviour
{
    [SerializeField] private Text text;

    private float CurrentFPS => 1f / Time.deltaTime;
    private float AverageFPS
    {
        get
        {
            _averageFPS = Mathf.Lerp(_averageFPS, CurrentFPS, 0.1f);
            return Mathf.RoundToInt(_averageFPS);
        }
    }
    private float _averageFPS = 60;

    private void Update()
    {
        text.text = AverageFPS.ToString();
    }
}
