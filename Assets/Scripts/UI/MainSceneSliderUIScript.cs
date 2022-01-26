using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneSliderUIScript : MonoBehaviour
{
    [SerializeField] Image marker;
    [SerializeField] RectTransform parent;

    private void Awake()
    {
        var mapData = GameStateManager.Instance.SelectedMapData;
        SetFraction(mapData.completionFraction);
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
