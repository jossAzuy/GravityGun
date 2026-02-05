using UnityEngine;


public class WarningSignalColorLerp : MonoBehaviour
{
    public Color startColor = Color.white;
    public Color endColor = Color.red;
    public float duration = 1f; // Duración de la señal

    private float timer = 0f;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = startColor;
        }
    }

    void Update()
    {
        if (spriteRenderer == null) return;
        timer += Time.deltaTime;
        // Oscila el valor t entre 0 y 1 de forma infinita usando PingPong
        float t = Mathf.PingPong(timer / duration, 1f);
        // Solo SmoothStep para un cambio suave y claro
        t = Mathf.SmoothStep(0, 1, t);
        spriteRenderer.color = Color.Lerp(startColor, endColor, t);
    }
}
