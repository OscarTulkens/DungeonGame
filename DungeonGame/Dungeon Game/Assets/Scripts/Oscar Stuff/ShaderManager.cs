using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class ShaderManager : MonoBehaviour
{
    [Header("FOG SETTINGS")]
    [SerializeField] private bool _fogOn = false;
    [SerializeField] private float _worldFogHeight = 0;
    [SerializeField] private float _worldSpaceFogWidth = 0;
    [SerializeField] private float _worldSpaceFogStrength = 0;

    [Header("EVENT OVERLAY SETTINGS")]
    [SerializeField] private RectTransform _eventOverlayImage = null;
    [SerializeField] private float _eventOverlayAlphaValue = 0;
    [SerializeField] private float _eventOverlayFadeDuration =0 ;

    private int _doOverlayID;
    private int _disableOverlayID;

    private void Start()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnStartCombat += DoOverlay;
            EventManager.Instance.OnEndCombat += DisableOverlay;

            EventManager.Instance.OnEndTreasure += DisableOverlay;
            EventManager.Instance.OnStartTreasure += DoOverlay;

            EventManager.Instance.OnOpenInventory += DoOverlay;
            EventManager.Instance.OnCloseInventory += DisableOverlay;
        }

        if (_fogOn)
        {
            Shader.SetGlobalFloat("WorldFogHeight", _worldFogHeight);
            Shader.SetGlobalFloat("WorldSpaceFogWidth", _worldSpaceFogWidth);
            Shader.SetGlobalFloat("WorldSpaceFogStrength", _worldSpaceFogStrength);
        }
        else
        {
            Shader.SetGlobalFloat("WorldFogHeight", 0);
            Shader.SetGlobalFloat("WorldSpaceFogWidth", 0);
            Shader.SetGlobalFloat("WorldSpaceFogStrength", 0);
        }
    }

    private void DoOverlay(object sender, EventArgs e)
    {
        LeanTween.cancel(_disableOverlayID);
        _doOverlayID = LeanTween.alpha(_eventOverlayImage, _eventOverlayAlphaValue, _eventOverlayFadeDuration).setEaseOutQuart().id;
    }

    private void DisableOverlay(object sender, EventArgs e)
    {
        LeanTween.cancel(_doOverlayID);
        _disableOverlayID = LeanTween.alpha(_eventOverlayImage, 0, _eventOverlayFadeDuration).setEaseOutQuart().id;
    }


}
