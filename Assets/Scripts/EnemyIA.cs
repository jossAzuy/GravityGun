using UnityEngine;
using UnityEngine.AI;

public class EnemyIA : MonoBehaviour
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

     public void DestroyEnemy()
    {
        if (PlayerScore.Instance != null)
        {
            PlayerScore.Instance.AddScore(100);
            Debug.Log("Enemy destroyed, score added!");
        }
        Destroy(gameObject);
    }
}