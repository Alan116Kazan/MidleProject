using Assets.Scripts.Components.Interfaces;

public class DummySettings : IGameSettingsProvider
{
    public int HeroHealth => 100;
    public int Speed => 7;
}