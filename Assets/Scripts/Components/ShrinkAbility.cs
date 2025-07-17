using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkAbility : MonoBehaviour, IAbility
{
    public float scaleFactor = 0.2f;

    private Vector3 startScale;

    private bool _started = false;

    private void Start()
    {
        startScale = transform.localScale;
    }

    public void Execute()
    {
        if (_started) return;
        _started = true;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(startScale * scaleFactor, 0.3f));
        sequence.Append(transform.DOScale(startScale, 0.3f));
        //.DOScale(startScale * scaleFactor, 0.3f).OnComplete(shrinkBack);
    }

    void shrinkBack()
    {
        transform.DOScale(startScale, 0.3f);
    }

}