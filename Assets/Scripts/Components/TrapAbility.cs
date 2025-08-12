using UnityEngine;
using DG.Tweening;

public class TrapAbility : MonoBehaviour, IAbility
{
    [Header("Настройки движения")]
    [SerializeField] private float dropDistance = 2f;
    [SerializeField] private float dropDuration = 0.3f;
    [SerializeField] private float riseDuration = 1f;
    [SerializeField] private float resetDelay = 1f;

    private Vector3 _startPosition;
    private bool _isRunning;

    private void Start()
    {
        _startPosition = transform.position;
    }

    public void Execute()
    {
        if (_isRunning) return;

        _isRunning = true;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOMoveY(_startPosition.y - dropDistance, dropDuration).SetEase(Ease.InQuad));

        sequence.AppendInterval(resetDelay);

        sequence.Append(transform.DOMoveY(_startPosition.y, riseDuration).SetEase(Ease.OutQuad));

        sequence.OnComplete(() =>
        {
            _isRunning = false;
        });
    }

}
