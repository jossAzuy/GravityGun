using UnityEngine;

public class EnemyChaseRaycast : MonoBehaviour
{
    public Transform target; // El jugador
    public float speed = 3f;
    public float obstacleDetectDistance = 1.5f;
    public LayerMask obstacleMask;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;

        // Lanza un raycast hacia el jugador
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, obstacleDetectDistance, obstacleMask);

        if (hit.collider == null)
        {
            // No hay obstáculo → moverse hacia el jugador
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        }
        else
        {
            // Hay obstáculo → hacer una evasión simple (ej. intentar moverse hacia arriba)
            Vector2 evadeDir = Vector2.Perpendicular(direction).normalized;
            rb.MovePosition(rb.position + evadeDir * speed * Time.fixedDeltaTime);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + (target.position - transform.position).normalized * obstacleDetectDistance);
        }
    }
}
