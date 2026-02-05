using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerScore : MonoBehaviour
{
    public static PlayerScore Instance { get; private set; }
    public TMP_Text scoreText;
    public float animationDuration = 0.5f; // Duración de la animación de puntos
    public float scaleMultiplier = 1.5f;   // Escalado máximo durante la animación

    private int score = 0;
    private Coroutine animCoroutine;

    void Awake()
    {
        Instance = this;
        Debug.Log("PlayerScore Awake ejecutado" + Instance);

    }

    void Start()
    {
        UpdateScoreText();
    }

    public void AddScore(int amount)
    {
        score += amount;
        if (animCoroutine != null) StopCoroutine(animCoroutine);
        animCoroutine = StartCoroutine(ClockScoreAnimation());
    }

    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }

    private IEnumerator ClockScoreAnimation()
    {
        float duration = animationDuration;
        float elapsed = 0f;
        Quaternion originalRot = scoreText.transform.localRotation;
        Quaternion midRot = Quaternion.Euler(-90, 0, 0);
        Quaternion endRot = Quaternion.Euler(0, 0, 0);

        // Gira hasta 90° (mitad, invisible)
        while (elapsed < duration / 2f)
        {
            scoreText.transform.localRotation = Quaternion.Slerp(originalRot, midRot, elapsed / (duration / 2f));
            elapsed += Time.deltaTime;
            yield return null;
        }
        scoreText.transform.localRotation = midRot;

        // Cambia el número cuando está de lado (invisible)
        UpdateScoreText();

        // Gira de 90° a 0° (aparece el nuevo número)
        elapsed = 0f;
        while (elapsed < duration / 2f)
        {
            scoreText.transform.localRotation = Quaternion.Slerp(midRot, endRot, elapsed / (duration / 2f));
            elapsed += Time.deltaTime;
            yield return null;
        }
        scoreText.transform.localRotation = endRot;
    }
}