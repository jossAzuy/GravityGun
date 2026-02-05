using UnityEngine;

public class Testing : MonoBehaviour
{
    [Header("Gravedad")]
    [SerializeField] private float gravityFalling = 2f; // Gravedad al caer (modificable desde el inspector)
    [SerializeField] private float gravityNormal = 1f;  // Gravedad normal (modificable desde el inspector)

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (rb != null)
        {
            if (rb.linearVelocity.y < 0)
                rb.gravityScale = gravityFalling; // Valor de gravedad al caer
            else
                rb.gravityScale = gravityNormal;  // Valor normal de gravedad
        }
    }



}