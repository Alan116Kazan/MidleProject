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
            // Игнорируем, если персонаж уже мёртв
            if (isDead) return;

            int oldHealth = _health;
            _health = Mathf.Max(0, value);

            UpdateHealthUI();

            // Триггерим анимацию урона
            if (!isInitializing && _health < oldHealth)
            {
                _animator?.SetTrigger(_damageTrigger);
            }

            // Смерть
            if (_health <= 0)
            {
                isDead = true;

                if (_animator != null && !string.IsNullOrEmpty(_deathTrigger))
                {
                    _animator.SetTrigger(_deathTrigger);
                    Destroy(gameObject, 2f); // дать анимации проиграться
                }
                else
                {
                    Destroy(gameObject);
                }
            }

            // Сохраняем новое здоровье (если это не инициализация)
            if (!isInitializing)
            {
                _gameDataManager.SavePlayerStats(new PlayerStats { Health = _health });
            }
        }
    }

    private void Awake()
    {
        _gameDataManager = new GameDataManager(LocalFileSaver.SaveFileName);

        if (_animator == null)
            _animator = GetComponent<Animator>();
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
