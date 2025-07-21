using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TrapTrigger : MonoBehaviour
{
    private bool _playerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_playerInside) return;

        if (other.CompareTag("Player"))
        {
            var ability = GetComponentInChildren<IAbility>();
            if (ability != null)
            {
                ability.Execute();
                _playerInside = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInside = false;
        }
    }
}
