using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour
{
    public Settings settings;
    public Text healthText;

    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _damageTrigger = "TakeDamage";
    [SerializeField] private string _deathTrigger = "Die";

    private int _health;
    private bool isInitializing = true;

    private GameDataManager _gameDataManager;
    private bool isDead = false;

    public int Health
    {
        get => _health;
        set
        {
            if (isDead) return;

            int oldHealth = _health;
            _health = Mathf.Max(0, value);

            UpdateHealthUI();

            if (!isInitializing && _health < oldHealth)
            {
                _animator?.SetTrigger(_damageTrigger);
            }

            if (_health <= 0)
            {
                isDead = true;
                if (_animator != null && !string.IsNullOrEmpty(_deathTrigger))
                {
                    _animator.SetTrigger(_deathTrigger);
                    Destroy(gameObject, 2f);
                }
                else
                {
                    Destroy(gameObject);
                }
            }

            if (!isInitializing)
            {
                SaveHealthAsync();
            }
        }
    }

    private async void SaveHealthAsync()
    {
        try
        {
            await _gameDataManager.SavePlayerStatsAsync(new PlayerStats { Health = _health });
        }
        catch (Exception ex)
        {
            Debug.LogError("Ошибка при сохранении: " + ex);
        }
    }


    private void Awake()
    {
        _gameDataManager = new GameDataManager(LocalFileSaver.SaveFileName);

        if (_animator == null)
            _animator = GetComponent<Animator>();
    }

    private async void Start()
    {
        var stats = await _gameDataManager.LoadPlayerStatsAsync();
        if (stats != null)
        {
            Debug.Log($"Загружено здоровье: {stats.Health}");
            Health = stats.Health;
        }
        else
        {
            Debug.Log("Используем значение из настроек.");
            Health = settings.HeroHealth;
        }

        isInitializing = false;
    }


    private void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = _health.ToString();
        else
            Debug.LogWarning("Health UI не назначен!");
    }
}
