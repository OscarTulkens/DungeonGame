using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ShaderManager : MonoBehaviour
{
    [SerializeField] private bool _fogOn;
    [SerializeField] private float _worldFogHeight;
    [SerializeField] private float _worldSpaceFogWidth;
    [SerializeField] private float _worldSpaceFogStrength;

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
