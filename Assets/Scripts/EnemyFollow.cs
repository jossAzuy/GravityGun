using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;

    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        // Opcional: para 2D, desactiva la rotación y ajusta altura
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }
}