using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0f, 0f, 100f); // Velocidad de rotación en cada eje

    void Update()
    {
        // Aplica la rotación al objeto
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
