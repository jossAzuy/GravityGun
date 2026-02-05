using UnityEngine;

public class GamePauseManager : MonoBehaviour
{
    public KeyCode pauseKey1 = KeyCode.Escape; // Primera tecla para pausar/reanudar
    public KeyCode pauseKey2 = KeyCode.P;      // Segunda tecla para pausar/reanudar
    public GameObject pauseMenuUI; // Asigna el panel de pausa en el inspector (opcional)

    private static bool _isPaused = false;
    // Permite que otros scripts consulten si el juego está en pausa
    public static bool IsPaused
    {
        get => _isPaused;
        set => _isPaused = value;
    }

    void Update()
    {
        if (PlayerHealth.Instance.IsDead)
        {
            // Si el jugador está muerto, no permite pausar el juego
            return;
        }

        if (Input.GetKeyDown(pauseKey1) || Input.GetKeyDown(pauseKey2))
        {
            if (_isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        _isPaused = true;
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        _isPaused = false;
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
    }
}
