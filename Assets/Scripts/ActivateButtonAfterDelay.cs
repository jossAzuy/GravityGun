using UnityEngine;
using UnityEngine.UI;

public class ActivateButtonAfterDelay : MonoBehaviour
{
    public Button targetButton; // Asigna el botón en el inspector
    public float delay = 5f;    // Tiempo en segundos antes de activar el botón

    void Start()
    {
        if (targetButton != null)
        {
            targetButton.gameObject.SetActive(false); // Oculta el botón al inicio
            Invoke(nameof(ActivateButton), delay);    // Llama a ActivateButton después del delay
        }
    }

    void ActivateButton()
    {
        if (targetButton != null)
        {
            targetButton.gameObject.SetActive(true); // Muestra el botón
        }
    }
}
