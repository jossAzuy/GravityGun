using System.Collections; // Para usar IEnumerator
using UnityEngine;
using UnityEngine.SceneManagement; // Para reiniciar la escena
using TMPro; // Para manejar el texto de TextMeshPro

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; } 

    public int lives = 3; // Número de vidas del jugador
    public int maxLives = 3; // Máximo número de vidas
    public TextMeshProUGUI livesText; // Referencia al texto de TextMeshPro para mostrar las vidas
    private bool isDead = false; // Estado del jugador, si está muerto o no
    public bool IsDead => isDead;
    [Header("Debug/Invulnerabilidad")]
    public bool godMode = false; // Si está activo, el jugador no recibe daño de enemigos

    public float invulnerabilityDuration = 2f; // Duración de la invulnerabilidad en segundos
    private bool isInvulnerable = false; // Estado de invulnerabilidad

    public SpriteRenderer playerSprite; // Referencia al SpriteRenderer del jugador
    public GameObject gameOverMenu; // Referencia al menú de Game Over

    public MonoBehaviour scriptToDisable1; // Primer script a desactivar
    public MonoBehaviour scriptToDisable2; // Segundo script a desactivar

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Asigna la instancia estática
            // DontDestroyOnLoad(gameObject); // No destruir este objeto al cambiar de escena
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Destruye duplicados
        }
    }

    void Start()
    {
        if (gameOverMenu != null)
        {
            gameOverMenu.SetActive(false); // Asegura que el menú de Game Over esté desactivado al inicio
        }

        UpdateLivesText(); // Actualiza el texto al inicio
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsDead) return;

        if (godMode) return; // Si está activo, ignora el daño

        if (other.CompareTag("Enemy") || other.CompareTag("Hazard"))
        {
            LoseLife();
        }
    }

    public void LoseLife()
    {
        if (isInvulnerable) return; // Si es invulnerable, no pierde vida

        lives--;

        AudioManager.Instance.PlayPlayerTakeDamage(); 

        UpdateLivesText(); // Actualiza el texto al perder una vida

        if (lives <= 0)
        {
            isDead = true;
            RestartLevel();
        }
        else
        {
            Debug.Log($"Player lost a life. Remaining lives: {lives}");
            StartCoroutine(InvulnerabilityCoroutine()); // Inicia la invulnerabilidad
        }
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;

        if (playerSprite != null)
        {
            float elapsedTime = 0f;
            while (elapsedTime < invulnerabilityDuration)
            {
                playerSprite.enabled = !playerSprite.enabled; // Alterna la visibilidad del sprite
                yield return new WaitForSeconds(0.20f); // Tiempo entre parpadeos
                elapsedTime += 0.2f;
            }
            playerSprite.enabled = true; // Asegura que el sprite esté visible al final
        }

        isInvulnerable = false;
    }

    /* public void GainLife()
    {
        if (lives < maxLives)
        {
            lives++;
            UpdateLivesText(); // Actualiza el texto al ganar una vida
            Debug.Log($"Player gained a life. Current lives: {lives}");
        }
    } */

    private void UpdateLivesText()
    {
        if (livesText != null)
        {
            livesText.text = $"LIVES:{lives}";
        }
    }

    private void RestartLevel()
    {
        if (gameOverMenu != null)
        {
            gameOverMenu.SetActive(true); // Muestra el menú de Game Over
        }
        else
        {
            Debug.LogError("Game Over menu is not assigned in the inspector.");
        }

        if (scriptToDisable1 != null)
        {
            scriptToDisable1.enabled = false; // Desactiva el primer script
        }

        if (scriptToDisable2 != null)
        {
            scriptToDisable2.enabled = false; // Desactiva el segundo script
        }

        StartCoroutine(WaitForRestart());
    }

    private IEnumerator WaitForRestart()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reinicia la escena
                break;
            }
            yield return null;
        }
    }
}
