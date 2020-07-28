using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpChoiceScript : MonoBehaviour
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
        _originalSize = _popUpPanel.GetComponent<RectTransform>().localScale;
        _popUpPanel.transform.localScale = Vector3.zero;
        OpenPopUp(this, EventArgs.Empty);
        OnTweenOutPanel += OnTweenOutPanelComplete;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPopUpTextChoices(PopUpTextObject popUpTextObject)
    {
        SetPopUpTextChoices(popUpTextObject.MainText, popUpTextObject.PosText, popUpTextObject.NegText);
    }

    public void SetPopUpTextChoices(string maintext, string postext, string negtext)
    {
        _mainText.text = maintext;
        _positiveButtonText.text = postext;
        _negativeButtonText.text = negtext;
    }

    public void ClickPos()
    {
        EventManager.Instance.DoPopUpPos();
    }

    public void ClickNeg()
    {
        EventManager.Instance.DoPopUpNeg();
    }

    public void ClosePopUp(object sender, EventArgs e)
    {
        LeanTween.scale(_popUpPanel, Vector3.zero, 0.5f).setEaseOutQuint().setOnComplete(OnTweenOutPanel);
    }

    public void OpenPopUp(object sender, EventArgs e)
    {
        TreasureManager.Instance.PopUpActive = true;
        LeanTween.scale(_popUpPanel, _originalSize, 1).setEaseOutBounce();
    }

    private void OnTweenOutPanelComplete()
    {
        TreasureManager.Instance.PopUpActive = false;
        EventManager.Instance.OnDoPopUpNeg -= ClosePopUp;
        Destroy(this.gameObject);
    }

}

public enum PopUpChoices
{
    InventoryFull,
    Sell,
    Buy
}
