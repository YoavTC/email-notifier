using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class CycleGUISize : MonoBehaviour
{
    [SerializeField] private float[] scales;
    private int currentScaleIndex;
    private CanvasScaler canvasScaler;

    [SerializeField] private TMP_Text buttonDisplay;
    
    private void Start()
    {
        canvasScaler = GetComponent<CanvasScaler>();

        currentScaleIndex = 0;
    }

    public void OnChangeSizeButtonPress()
    {
        currentScaleIndex++;
        if (currentScaleIndex + 1 > scales.Length) currentScaleIndex = 0;
        
        SetScaleFactor();

        buttonDisplay.text = $"Scale: {scales[currentScaleIndex]}";
    }

    private void SetScaleFactor()
    {
        canvasScaler.scaleFactor = scales[currentScaleIndex];
    }
}
