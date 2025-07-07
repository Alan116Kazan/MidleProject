using Assets.Scripts.Components.Interfaces;
using UnityEngine;
using Zenject;

public class InjectionTest : MonoBehaviour
{
    private ITest _test;
    private IGameSettingsProvider _settings;

    [Inject]
    public void Init(ITest test, IGameSettingsProvider settings)
    {
        _test = test;
        _settings = settings;
    }

    void Start()
    {
        _test.Echo();
        Debug.Log($"Hero Speed from injected config: {_settings.Speed}");
    }
}