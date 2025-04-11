using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour
{
    public Settings settings;
    public Text healthText;

    private int _health;
    private bool isInitializing = true;

    private GameDataManager _gameDataManager;

    public int Health
    {
        get => _health;
        set
        {
            _health = Mathf.Max(0, value);
            UpdateHealthUI();

            if (_health <= 0)
                Destroy(gameObject);

            if (!isInitializing)
            {
                _gameDataManager.SavePlayerStats(new PlayerStats { Health = _health });
            }
        }
    }

    private void Awake()
    {
        _gameDataManager = new GameDataManager(LocalFileSaver.SaveFileName);
    }

    private void Start()
    {
        _gameDataManager.LoadPlayerStats(
            stats =>
            {
                Debug.Log($"Загружено здоровье: {stats.Health}");
                Health = stats.Health;
                isInitializing = false;
            },
            onError: () =>
            {
                Debug.Log("Используем значение из настроек.");
                Health = settings.HeroHealth;
                isInitializing = false;
            });
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = _health.ToString();
        else
            Debug.LogWarning("Health UI не назначен!");
    }
}