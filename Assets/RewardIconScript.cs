using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class RewardIconScript : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text countText;
    public void SetSprite(Sprite s)
    {
        image.sprite = s;
    }
    public void SetCount(int val)
    {
        countText.text = "x" + val;
    }
}
