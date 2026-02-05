using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // Carga una escena por su nombre
    public void LoadSceneByName(string sceneName)
    {
        GamePauseManager.IsPaused = false; // Asegurarse de que el juego no esté en pausa
        Time.timeScale = 1f; // Restaurar el tiempo antes de cambiar de escena
        SceneManager.LoadScene(sceneName);
    }

    // Carga la siguiente escena en el índice de construcción
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        if (currentSceneIndex + 1 < totalScenes)
        {
            GamePauseManager.IsPaused = false; // Asegurarse de que el juego no esté en pausa
            Time.timeScale = 1f; // Restaurar el tiempo antes de cambiar de escena

            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            Debug.LogWarning("No hay más escenas para cargar.");
        }
    }

    // Carga la escena anterior en el índice de construcción
    public void LoadPreviousScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex > 0)
        {
            SceneManager.LoadScene(currentSceneIndex - 1);
        }
    }
}
