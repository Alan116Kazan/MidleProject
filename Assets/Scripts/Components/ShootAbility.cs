using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAbility : MonoBehaviour, IAbility
{
    public GameObject _bullet;      // ������ ����
    public float shotDelay;         // �������� ����� ����������

    public float shootingForce = 5f;

    private float _shootTime = float.MinValue;  // ����� ���������� ��������

    public void Exicute()
    {
        // ���������, ������ �� ���������� ������� � �������� ��������.
        if (Time.time < _shootTime + shotDelay) return;

        _shootTime = Time.time;  // ��������� ����� ���������� ��������

        if (_bullet != null)
        {
            GameObject newBullet = Instantiate(_bullet, transform.position, transform.rotation);

            // ��������� ���� ��������.
            Rigidbody rb = newBullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(transform.forward * shootingForce, ForceMode.Impulse);
            }
        }
        else
        {
            // ���� ������ ���� �� �����, ������� ��������� �� ������.
            Debug.LogError("[Shoot Ability] No bullet prefab link!");
        }
    }
}
