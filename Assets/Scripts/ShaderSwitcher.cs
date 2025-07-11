using UnityEngine;

public class ShaderSwitcher : MonoBehaviour
{
    [SerializeField] private Renderer _targetRenderer;
    [SerializeField] private Shader _newShader;

    private Shader _originalShader;
    private bool _isUsingNewShader;

    private void Awake()
    {
        if (_targetRenderer == null)
        {
            Debug.LogError($"{nameof(ShaderSwitcher)}: Target Renderer не назначен.");
            enabled = false;
            return;
        }

        _originalShader = _targetRenderer.sharedMaterial.shader;
    }

    public void ChangeShader()
    {
        if (_newShader == null || _originalShader == null)
        {
            Debug.LogWarning($"{nameof(ShaderSwitcher)}: Shader не задан.");
            return;
        }

        var material = _targetRenderer.material;
        material.shader = _isUsingNewShader ? _originalShader : _newShader;
        _isUsingNewShader = !_isUsingNewShader;
    }
}
