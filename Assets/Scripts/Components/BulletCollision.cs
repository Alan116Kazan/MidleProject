using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour, IAbilityTarget
{
    public List<GameObject> Targets { get; set; }

    public void Execute()
    {
        // Если способность отскока активна, пуля не уничтожается
        if (BounceAbilityManager.IsBounceActive && Time.time < BounceAbilityManager.BounceEndTime)
        {
            return;
        }

        // Иначе уничтожаем пулю
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Уничтожаем пулю при столкновении с любым объектом
        Execute();
    }
}