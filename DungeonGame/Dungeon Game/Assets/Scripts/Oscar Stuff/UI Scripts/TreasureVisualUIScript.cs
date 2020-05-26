using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureVisualUIScript : MonoBehaviour
{
    [Header("Treasure Item Pop Up")]
    [SerializeField] private Image _treasureImage;
    [SerializeField] private RectTransform _desiredTreasureLocation;
    [SerializeField] private Text _treasureName;

    private Vector3 _startPosition;
    private List<int> _tweens = new List<int>();


    private void Start()
    {
        _startPosition = _treasureImage.rectTransform.position;
        _treasureImage.rectTransform.localScale = new Vector3(0, 0, 0);
        TreasureManager.Instance.OnGetTreasureItem += PopUpNewItemTreasure;
        TreasureManager.Instance.OnEndTreasure += ResetTreasureUI;
    }

    private void PopUpNewItemTreasure(object sender, TreasureManager.OnGetTreasureItemEventArgs e)
    {
        Debug.Log(e.name + e.image + e.subtext);
        ResetTreasureUI(sender, e);
        SetTreasureVariables(e.image, e.name, e.subtext);
        NormalTreasureLeanTween();
    }

    private void ResetTreasureUI(object sender, EventArgs e)
    {
        foreach (int tween in _tweens)
        {
            LeanTween.cancel(tween);
        }
        _treasureImage.rectTransform.position = _startPosition;
        _treasureImage.rectTransform.localScale = new Vector3(0, 0, 0);
    }

    private void NormalTreasureLeanTween()
    {
        _tweens.Add(LeanTween.move(_treasureImage.gameObject, _desiredTreasureLocation.position, 0.6f).setEaseOutBack().id);
        _tweens.Add(LeanTween.scale(_treasureImage.gameObject, new Vector3(1, 1, 1), 0.79f).setEaseOutBounce().id);
    }

    private void SetTreasureVariables(Sprite itemSprite, string itemName, string subText)
    {
        _treasureImage.sprite = itemSprite;
        _treasureName.text = itemName;
    }
}
