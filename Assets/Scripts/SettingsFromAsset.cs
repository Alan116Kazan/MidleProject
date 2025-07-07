using Assets.Scripts.Components.Interfaces;

public class SettingsFromAsset : IGameSettingsProvider
{
    private readonly Settings _settings;

    public SettingsFromAsset(Settings settings)
    {
        _settings = settings;
    }

    //public int HeroHealth => _settings.HeroHealth;
    public int Speed => _settings.Speed;
}
