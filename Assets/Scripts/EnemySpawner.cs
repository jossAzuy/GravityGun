using UnityEngine;
using MoreMountains.Feedbacks;


public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject warningSpritePrefab; // Referencia al prefab del sprite de advertencia
    public Transform[] spawnPoints;
    public Transform[] warningPositions; // Asigna cada posición de advertencia en el inspector
    public float spawnInterval = 2f;
    public float warningLeadTime = 1f;    // Tiempo antes del spawn del enemigo en que aparece la advertencia
    public float warningDisplayTime = 1f; // Tiempo que la advertencia permanece en pantalla

    [Header("Visual Feedbacks")]
    public MMF_Player warningFeedback; // Asigna el feedback visual de advertencia en el inspector
    public MMF_Player spawnFeedback;   // Asigna el feedback visual de spawn en el inspector

    private float timer = 0f;

   /*  void Start()
    {
       warningFeedback.PlayFeedbacks();
    } */

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;
        int index = Random.Range(0, spawnPoints.Length);

        // Usa la posición de advertencia correspondiente al punto de spawn
        Vector3 warningPosition = spawnPoints[index].position;
        if (warningPositions != null && warningPositions.Length > index && warningPositions[index] != null)
            warningPosition = warningPositions[index].position;

        // Instancia la advertencia y obtiene el feedback del prefab si existe
        GameObject warning = Instantiate(warningSpritePrefab, warningPosition, Quaternion.identity);
        Destroy(warning, warningDisplayTime);
 
        // Si el prefab de advertencia tiene un MMF_Player, lo ejecuta
        MMF_Player warningPrefabFeedback = warning.GetComponent<MMF_Player>();
        if (warningPrefabFeedback != null)
        {
           warningPrefabFeedback.PlayFeedbacks();
        } 

        // El enemigo aparece después de warningLeadTime segundos
        StartCoroutine(SpawnEnemyWithDelay(spawnPoints[index].position, warningLeadTime));
    }

    private System.Collections.IEnumerator SpawnEnemyWithDelay(Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(enemyPrefab, position, Quaternion.identity);
    }
}