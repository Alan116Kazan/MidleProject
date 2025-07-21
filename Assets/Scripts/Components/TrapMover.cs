using UnityEngine;
using DG.Tweening;

public class TrapMover : MonoBehaviour, IAbility
{
    public float moveDistance = 2f;        // Насколько поднимается/опускается блок
    public float moveDuration = 1f;        // Время на одно движение
    public float delayBetweenMoves = 0.5f; // Пауза между движениями

    private bool _started = false;
    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
    }

    public void Execute()
    {
        if (_started) return;
        _started = true;

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(delayBetweenMoves);
        seq.Append(transform.DOMoveY(_startPosition.y - moveDistance, moveDuration).SetEase(Ease.InQuad));
        seq.AppendInterval(delayBetweenMoves);
        seq.Append(transform.DOMoveY(_startPosition.y, moveDuration).SetEase(Ease.OutQuad));
        seq.SetLoops(-1); // Бесконечно
    }
}
