using UnityEngine;
[RequireComponent(typeof(Collider))]
public class TrapTrigger : MonoBehaviour
{
    [SerializeField] private float debounceTime = 1f; // Задержка перед повторным срабатыванием
    private bool _isCoolingDown; private float _lastTriggerTime;
    private void OnTriggerEnter(Collider other)
    {
        if (_isCoolingDown || Time.time < _lastTriggerTime + debounceTime) return;
        if (other.CompareTag("Player"))
        {
            var ability = GetComponentInChildren<IAbility>(); if (ability != null)
            {
                ability.Execute(); _lastTriggerTime = Time.time; _isCoolingDown = true; Invoke(nameof(ResetCooldown), debounceTime);
            }
        }
    }
    private void ResetCooldown() { _isCoolingDown = false; }
}