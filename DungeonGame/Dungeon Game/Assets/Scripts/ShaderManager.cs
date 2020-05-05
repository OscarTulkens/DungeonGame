using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
[ExecuteAlways]
public class ShaderManager : MonoBehaviour
{
    [SerializeField] private float _ppSpeed = 0;
    [SerializeField] private Volume _overlayVolume = null;
    [SerializeField] private bool _fogOn = false;
    [SerializeField] private float _worldFogHeight = 0;
    [SerializeField] private float _worldSpaceFogWidth = 0;
    [SerializeField] private float _worldSpaceFogStrength = 0;

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
        ManagePostPro();
        AssignInstances();
    }

    void ManagePostPro()
    {
        if (_combatManager != null && _treasureManager != null)
        {
            if (_combatManager.enabled || _treasureManager.enabled)
            {
                if (_overlayVolume.weight !=1)
                {
                    _overlayVolume.weight = Mathf.Lerp(_overlayVolume.weight, 1, Time.deltaTime * _ppSpeed);
                    if (_overlayVolume.weight >= 0.95)
                    {
                        _overlayVolume.weight = 1;
                    }
                }
            }

            else
            {
                if(_overlayVolume.weight != 0)
                {
                    _overlayVolume.weight = Mathf.Lerp(_overlayVolume.weight, 0, Time.deltaTime * _ppSpeed);
                    if (_overlayVolume.weight <=0.05f)
                    {
                        _overlayVolume.weight = 0;
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
