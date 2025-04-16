using Assets.Scripts.Components.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitBehaviour : MonoBehaviour, IBehaviour
{
    public void Behave()
    {
        Debug.Log("Wait");
    }

    public float Evaluate()
    {
        return 0.5f;
    }
}
