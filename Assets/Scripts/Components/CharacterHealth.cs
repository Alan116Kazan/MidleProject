using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour
{
    // Используем DummySettings вместо SO Settings
    private DummySettings _settingsProvider = new DummySettings();


    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _damageTrigger = "TakeDamage";
    [SerializeField] private string _deathTrigger = "Die";

    private ViewModel viewModel;

    private int _health;
    private bool isInitializing = true;

    private GameDataManager _gameDataManager;
    private bool isDead = false;

    public int Health
    {
        get => _health;
        set
        {
            if (_health == value) return;

            _health = Mathf.Max(0, value);

            if (viewModel != null)
                viewModel.Health = _health.ToString();

            if (isDead) return;

            int oldHealth = _health;

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
        viewModel = FindObjectOfType<ViewModel>();

        try
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var stats = await _gameDataManager.LoadPlayerStatsAsync(cts.Token);

            if (stats != null)
            {
                Debug.Log($"Загружено здоровье: {stats.Health}");
                Health = stats.Health;
            }
            else
            {
                Debug.LogWarning("Не удалось загрузить здоровье. Используется дефолт из DummySettings.");
                Health = _settingsProvider.HeroHealth;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Ошибка при загрузке здоровья: " + ex);
            Health = _settingsProvider.HeroHealth;
        }
        finally
        {
            isInitializing = false;
        }
    }
}
