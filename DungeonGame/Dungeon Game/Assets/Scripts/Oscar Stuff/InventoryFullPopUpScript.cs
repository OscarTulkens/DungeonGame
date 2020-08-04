using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryFullPopUpScript : MonoBehaviour
{
    [SerializeField] private Text _mainText = null;
    [SerializeField] private Text _positiveButtonText = null;
    [SerializeField] private Text _negativeButtonText = null;

    [SerializeField] private Button _negativeButton = null;
    [SerializeField] private Button _positiveButton = null;
    [SerializeField] private GameObject _popUpPanel = null;

    private Vector3 _originalSize = new Vector3(0,0,0);

    private Action OnTweenOutPanel;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.OnCloseInventoryFullPopUp += ClosePopUp;
        _originalSize = _popUpPanel.GetComponent<RectTransform>().localScale;
        _popUpPanel.transform.localScale = Vector3.zero;
        OpenPopUp();
        OnTweenOutPanel += OnTweenOutPanelComplete;
    }

    public void ClickPos()
    {
        StartReplaceItemSequence();
    }

    public void ClickNeg()
    {
        ClosePopUp(this, EventArgs.Empty);
    }

    public void ClosePopUp(object sender, EventArgs e)
    {
        LeanTween.scale(_popUpPanel, Vector3.zero, 0.5f).setEaseOutQuint().setOnComplete(OnTweenOutPanel);
        InventoryManager.Instance.Replace = false;
    }

    public void OpenPopUp()
    {
        TreasureManager.Instance.PopUpActive = true;
        LeanTween.scale(_popUpPanel, _originalSize, 1).setEaseOutBounce();
    }

    private void OnTweenOutPanelComplete()
    {
        TreasureManager.Instance.PopUpActive = false;
        Destroy(gameObject);
        EventManager.Instance.CloseItemWheel();
        EventManager.Instance.OnCloseInventoryFullPopUp -= ClosePopUp;
    }

    private void StartReplaceItemSequence()
    {
        InventoryManager.Instance.Replace = true;
        EventManager.Instance.OpenItemWheel();
    }

}
