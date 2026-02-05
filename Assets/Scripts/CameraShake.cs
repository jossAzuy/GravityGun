using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    [Header("Shake Settings")]
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.2f;
    public float dampingSpeed = 1.0f;

    private Vector3 initialPosition;
    private float currentDuration = 0f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void OnEnable()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        if (currentDuration > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
            currentDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            currentDuration = 0f;
            transform.localPosition = initialPosition;
        }

        //Shake();
    }

    /// <summary>
    /// Llama a este método para sacudir la cámara.
    /// </summary>
    /// <param name="duration">Duración del shake (opcional)</param>
    /// <param name="magnitude">Magnitud del shake (opcional)</param>
    public void Shake(float? duration = null, float? magnitude = null)
    {
        currentDuration = duration ?? shakeDuration;
        shakeMagnitude = magnitude ?? shakeMagnitude;
    }
}