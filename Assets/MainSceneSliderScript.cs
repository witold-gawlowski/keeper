using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneSliderScript : MonoBehaviour
{
    [SerializeField] Image marker;
    [SerializeField] RectTransform parent;

    private void Awake()
    {
        SetFraction(GameManager.Instance.SelectedLevel.targetCompletionFraction);
    }
    void SetFraction(float value)
    {
        RectTransform markerTransform = marker.rectTransform;
        float parentWidth = parent.rect.width;
        Vector3 newLocalPosition = markerTransform.anchoredPosition;
        newLocalPosition.x = value * parentWidth;
        markerTransform.anchoredPosition = newLocalPosition;
    }
}
