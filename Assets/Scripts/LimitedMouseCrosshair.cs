using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LimitedMouseCrosshair : MonoBehaviour
{
    public RectTransform crosshair; // Asigna el objeto de la mira en el inspector

    // Límites de movimiento
    private float leftLimit;
    private float rightLimit;
    private float topLimit;
    private float bottomLimit;

    [Header("Márgenes")]
    public float marginX = 10f; // Margen horizontal
    public float marginY = 10f; // Margen vertical

    private Canvas canvas;

    // Referencias para raycast UI
    public GraphicRaycaster raycaster; // Asigna el GraphicRaycaster del Canvas en el inspector
    public EventSystem eventSystem;    // Asigna el EventSystem de la escena en el inspector

    void Start()
    {
        Cursor.visible = false; // Oculta el cursor del sistema
        Cursor.lockState = CursorLockMode.Confined; // Restringe el cursor a los límites de la pantalla

        canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                // Establece los límites según el tamaño del canvas para ScreenSpaceOverlay
                leftLimit = marginX;
                rightLimit = canvasRect.rect.width - marginX;
                bottomLimit = marginY;
                topLimit = canvasRect.rect.height - marginY;
            }
        }
    }

    void Update()
    {
        if (crosshair == null || canvas == null)
            return;

        // Si se presiona Esc, desbloquea y muestra el cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return; // Detiene la ejecución del resto del código en este frame
        }

        // Si se presiona cualquier tecla excepto Esc, vuelve a bloquear y ocultar el cursor
        if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }

        Vector3 mousePos = Input.mousePosition;

        // Actualiza la posición de la crosshair según el modo del canvas
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            Camera cam = canvas.worldCamera;
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            if (cam == null || canvasRect == null)
                return;

            // Convierte la posición del mouse a espacio del canvas
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, mousePos, cam, out Vector2 localPoint);

            // Limita la posición dentro de los márgenes
            localPoint.x = Mathf.Clamp(localPoint.x, canvasRect.rect.xMin + marginX, canvasRect.rect.xMax - marginX);
            localPoint.y = Mathf.Clamp(localPoint.y, canvasRect.rect.yMin + marginY, canvasRect.rect.yMax - marginY);

            // Actualiza la posición de la crosshair
            crosshair.localPosition = localPoint;
        }
        else if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            // Limita la posición del mouse dentro de los márgenes
            mousePos.x = Mathf.Clamp(mousePos.x, leftLimit, rightLimit);
            mousePos.y = Mathf.Clamp(mousePos.y, bottomLimit, topLimit);

            // Actualiza la posición de la crosshair
            crosshair.position = mousePos;
        }

        // --- Interacción con UI usando la crosshair ---
        // Si se hace clic izquierdo
        if (Input.GetMouseButtonDown(0) && raycaster != null && eventSystem != null)
        {
            // Crea un PointerEventData en la posición de la crosshair
            PointerEventData pointerData = new PointerEventData(eventSystem);
            pointerData.position = crosshair.position;

            // Lista para almacenar los resultados del raycast
            var results = new System.Collections.Generic.List<RaycastResult>();
            raycaster.Raycast(pointerData, results);

            // Recorre los resultados y activa el botón si lo encuentra
            foreach (RaycastResult result in results)
            {
                var button = result.gameObject.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.Invoke(); // Simula el clic en el botón
                    break; // Solo activa el primer botón encontrado
                }
            }
        }
    }
}
