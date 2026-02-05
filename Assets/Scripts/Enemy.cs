using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    private Transform player;
    private NavMeshAgent agent;

    public Material dissolveMaterial; // Asigna tu material con shader de disolución en el inspector
    public float dissolveDuration = 1.5f;

    private Material _runtimeMat;
    private bool isDissolving = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;

        // Busca automáticamente al jugador por tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (player != null && !isDissolving)
        {
            agent.SetDestination(player.position);
        }
    }

    public void DestroyEnemy()
    {
        if (isDissolving) return; // Evita que se ejecute más de una vez
        isDissolving = true;

        if (PlayerScore.Instance != null)
        {
            PlayerScore.Instance.AddScore(100);
            Debug.Log("Enemy destroyed, score added!");
        }

        // Reproduce el sonido de muerte
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayEnemyDeathSound();
        }

        // Detener el movimiento antes de disolver
        if (agent != null)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            agent.enabled = false;
        }

        // Iniciar disolución
        StartCoroutine(DissolveAndDestroy());
    }

    private IEnumerator DissolveAndDestroy()
    {
        isDissolving = true;

        // Asignar el material de disolución al renderer
        var renderer = GetComponentInChildren<Renderer>();
        if (renderer != null && dissolveMaterial != null)
        {
            // Instanciar el material para no afectar a otros enemigos
            _runtimeMat = new Material(dissolveMaterial);
            renderer.material = _runtimeMat;

            float t = 0f;
            while (t < dissolveDuration)
            {
                float dissolveAmount = Mathf.Lerp(0f, 1f, t / dissolveDuration);
                _runtimeMat.SetFloat("_DissolveAmount", dissolveAmount);
                t += Time.deltaTime;
                yield return null;
            }
            _runtimeMat.SetFloat("_DissolveAmount", 1f);
        }

        Destroy(gameObject);
    }
}