using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteItemButton : MonoBehaviour
{
    private List<int> _onGoingTweens = new List<int>();
    [SerializeField] private float _scaleIncreaseModifier;
    [SerializeField] private float _punchTime;
    private Vector3 _startSize = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        _startSize = gameObject.transform.localScale;
    }

    public void PunchButton()
    {
        CancelAllTweens();
        _onGoingTweens.Add(LeanTween.scale(gameObject, _startSize * _scaleIncreaseModifier, _punchTime).setEasePunch().id);
    }

    private void CancelAllTweens()
    {
        foreach (int tween in _onGoingTweens)
        {
            LeanTween.cancel(tween);
        }
    }
}
