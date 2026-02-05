using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class PlayerGun : MonoBehaviour
{
    [Header("Recarga y Disparo")]
    [SerializeField] private float fireRate = 0.3f;
    [SerializeField] private int maxAmmo = 4;
    [SerializeField] private int currentAmmo;
    [SerializeField] private float reloadCooldown = 2f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float recoilForce = 5f;

    [Header("Gravedad")]
    [SerializeField] private float gravityFalling = 2f;
    [SerializeField] private float gravityNormal = 1f;

    [Header("Trail Renderer")]
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private bool enableTrail = true;

    [Header("Efectos Visuales")]
    [SerializeField] private float startWidth = 0.1f;
    [SerializeField] private float endWidth = 0.1f;
    [SerializeField] private float timeToDestroyLine = 0.2f;

    private float nextFireTime = 0f;
    private Rigidbody2D rb;
    private bool isReloading = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        // Si el juego está en pausa, no procesar la lógica del arma
        if (GamePauseManager.IsPaused) return;

        // Rotación hacia el mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Aumenta la gravedad si está cayendo
        if (rb != null)
        {
            rb.gravityScale = rb.linearVelocity.y < 0 ? gravityFalling : gravityNormal;
        }

        // Disparo con intervalo y si hay balas
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime && currentAmmo > 0 && !isReloading)
        {

            CameraShake.Instance.Shake();

            Shoot(direction.normalized);
            nextFireTime = Time.time + fireRate;
            currentAmmo--;

            // Si se quedan sin balas, inicia recarga automática
            if (currentAmmo == 0)
            {
                StartCoroutine(AutoReload());
            }
        }

        // Activar o desactivar el TrailRenderer según el movimiento
        if (trailRenderer != null)
        {
            trailRenderer.emitting = enableTrail && rb != null && rb.linearVelocity.magnitude > 0.1f;
        }

        Debug.Log($"Current velocity update: {rb.linearVelocity.y}");

    }

    public void Shoot(Vector2 direction)
    {
        float radius = Mathf.Max(startWidth, endWidth) * 0.5f;
        Vector2 start = firePoint.position;
        Vector2 lastPoint = start;
        float remainingDistance = maxDistance;
        float step = 0.05f;

        while (remainingDistance > 0f)
        {
            Vector2 nextPoint = lastPoint + direction * Mathf.Min(step, remainingDistance);

            Collider2D hit = Physics2D.OverlapCircle(nextPoint, radius);
            if (hit != null && hit.CompareTag("Enemy"))
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                    enemy.DestroyEnemy();
                else
                    Destroy(hit.gameObject);
            }

            lastPoint = nextPoint;
            remainingDistance -= step;
        }

        // Visualización del disparo SIEMPRE hasta el máximo alcance
        Debug.DrawLine(start, lastPoint, Color.red, 0.5f);
        ShowPenetratingLine(start, lastPoint);

        AudioManager.Instance.PlayShootSound();

        if (rb != null)
        {
            float playerSpeed = rb.linearVelocity.magnitude / 8.5f;
            float dynamicRecoil = recoilForce + playerSpeed;
            rb.AddForce(-direction * dynamicRecoil, ForceMode2D.Impulse);
        }
    }

    private void ShowPenetratingLine(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("ShotLine");
        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        lineRenderer.startWidth = startWidth;
        lineRenderer.endWidth = endWidth;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.white;
        lineRenderer.sortingLayerName = "Default";
        lineRenderer.sortingOrder = 10;

        Destroy(lineObj, timeToDestroyLine);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(PlayerHealth.Instance.IsDead) return;

        if (collision.gameObject.CompareTag("Auto") && currentAmmo == 0)
        {
            Reload();
        }
        else
        {
            AudioManager.Instance.PlayPlayerBounce();
        } 

    }

    private IEnumerator AutoReload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadCooldown);
        currentAmmo = maxAmmo;
        isReloading = false;

        AudioManager.Instance.PlayReloadAmmoSound();
    }

    public void Reload()
    {
        StopAllCoroutines();
        currentAmmo = maxAmmo;
        isReloading = false;

        AudioManager.Instance.PlayReloadAmmoSound();

    }

    public bool IsReloading => isReloading;
    public float ReloadCooldown => reloadCooldown;
    public int CurrentAmmo => currentAmmo;
}
