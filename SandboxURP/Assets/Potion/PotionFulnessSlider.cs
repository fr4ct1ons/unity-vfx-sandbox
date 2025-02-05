using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionFulnessSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private LiquidPlaneCutoff cutoff;

    public void SetFulness(float value)
    {
        cutoff.SetFulness(value);
    }
}
