using UnityEngine;

public class GameMenuManager : MonoBehaviour
{
    public GameObject menuPanel; // Asigna el panel del menú en el inspector

    void Start()
    {
        // Pausa el juego al inicio y muestra el menú
        Time.timeScale = 0f;
        menuPanel.SetActive(true);
    }

    public void OnPlayButton()
    {
        // Reanuda el juego y oculta el menú
        Time.timeScale = 1f;
        menuPanel.SetActive(false);
    }
}