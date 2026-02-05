using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseCrosshair : MonoBehaviour
{
    public RectTransform crosshair; // Asigna el objeto de la mira en el inspector
    public GraphicRaycaster raycaster; // Asigna el GraphicRaycaster del Canvas
    public EventSystem eventSystem; // Asigna el EventSystem de la escena

    void Start()
    {
        Cursor.visible = false; // Oculta el puntero del mouse
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        crosshair.position = mousePos;

        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerData = new PointerEventData(eventSystem);
            pointerData.position = mousePos;

            var results = new System.Collections.Generic.List<RaycastResult>();
            raycaster.Raycast(pointerData, results);

            foreach (RaycastResult result in results)
            {
                var button = result.gameObject.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.Invoke();
                    break;
                }
            }
        }
    }
}
