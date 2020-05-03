using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ShaderManager : MonoBehaviour
{
    [SerializeField] private bool _fogOn = false;
    [SerializeField] private float _worldFogHeight = 0;
    [SerializeField] private float _worldSpaceFogWidth = 0;
    [SerializeField] private float _worldSpaceFogStrength = 0;

    private void Update()
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

}
