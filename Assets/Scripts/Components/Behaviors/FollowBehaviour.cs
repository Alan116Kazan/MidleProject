#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AI;
using Assets.Scripts.Components.Interfaces;

[RequireComponent(typeof(NavMeshAgent))]
public class FollowBehaviour : MonoBehaviour, IBehaviour
{
    public Transform target;
    public float viewRadius = 10f;
    [Range(0, 360)] public float viewAngle = 60f;

    private NavMeshAgent _agent;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public float Evaluate()
    {
        if (target == null)
            return float.MinValue;

        Vector3 dir = (target.position - transform.position).normalized;
        if (Vector3.Distance(transform.position, target.position) <= viewRadius &&
            Vector3.Angle(transform.forward, dir) <= viewAngle / 2f)
        {
            return 100f;
        }
        return 0f;
    }

    public void Behave()
    {
        if (target != null)
            _agent.SetDestination(target.position);
    }

    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;

        Gizmos.color = new Color(0f, 0.5f, 1f, 0.2f);
        Gizmos.DrawWireSphere(pos, viewRadius);

        float halfAngle = viewAngle / 2f;
        Quaternion leftRot = Quaternion.Euler(0, -halfAngle, 0);
        Quaternion rightRot = Quaternion.Euler(0, halfAngle, 0);
        Vector3 leftDir = leftRot * transform.forward;
        Vector3 rightDir = rightRot * transform.forward;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(pos, pos + leftDir * viewRadius);
        Gizmos.DrawLine(pos, pos + rightDir * viewRadius);

#if UNITY_EDITOR
        Handles.color = new Color(0f, 0.5f, 1f, 0.1f);
        Handles.DrawSolidArc(pos, Vector3.up, leftDir, viewAngle, viewRadius);
#endif
    }
}
