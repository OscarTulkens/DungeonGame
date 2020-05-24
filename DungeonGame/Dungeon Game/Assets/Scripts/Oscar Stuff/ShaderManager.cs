using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
[ExecuteAlways]
public class ShaderManager : MonoBehaviour
{
    [SerializeField] private float _ppSpeed = 0;
    [SerializeField] private Volume _overlayVolume = null;
    [SerializeField] private bool _fogOn = false;
    [SerializeField] private float _worldFogHeight = 0;
    [SerializeField] private float _worldSpaceFogWidth = 0;
    [SerializeField] private float _worldSpaceFogStrength = 0;
    [SerializeField] private Image _eventOverlayImage = null;
    [SerializeField] private float _eventOverlayAlpha = 0;
    [SerializeField] private float _alphaLerpSpeed;

    private Color _desiredColor;
    private CombatManagerScript _combatManager;
    private TreasureManager _treasureManager;

    private void Start()
    {
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

    private void Update()
    {
        ManageEventColorOverlay();
        AssignInstances();
    }

    void ManageEventColorOverlay()
    {
        if (_combatManager != null && _treasureManager != null)
        {
            if (_combatManager.enabled || _treasureManager.enabled)
            {
                if (_eventOverlayImage.color.a != _eventOverlayAlpha)
                {
                    _eventOverlayImage.color = new Color(_eventOverlayImage.color.r, _eventOverlayImage.color.g, _eventOverlayImage.color.b, Mathf.Lerp(_eventOverlayImage.color.a, _eventOverlayAlpha, _alphaLerpSpeed*Time.deltaTime));
                    if (_eventOverlayImage.color.a >= 0.95)
                    {
                        _eventOverlayImage.color = new Color(_eventOverlayImage.color.r, _eventOverlayImage.color.g, _eventOverlayImage.color.b, 1);
                    }
                }
            }

            else
            {
                if(_eventOverlayImage.color.a != 0)
                {
                    _eventOverlayImage.color = new Color(_eventOverlayImage.color.r, _eventOverlayImage.color.g, _eventOverlayImage.color.b, Mathf.Lerp(_eventOverlayImage.color.a, 0,_alphaLerpSpeed *Time.deltaTime));
                    if (_eventOverlayImage.color.a <=0.05f)
                    {
                        _eventOverlayImage.color = new Color(_eventOverlayImage.color.r, _eventOverlayImage.color.g, _eventOverlayImage.color.b, 0);
                    }
                }
            }
        }
    }

    void AssignInstances()
    {
        if (_combatManager == null)
        {
            if (CombatManagerScript.Instance)
            {
                _combatManager = CombatManagerScript.Instance;
            }
        }

        if (_treasureManager == null)
        {
            if (TreasureManager.Instance)
            {
                _treasureManager = TreasureManager.Instance;
            }
        }
    }

}
