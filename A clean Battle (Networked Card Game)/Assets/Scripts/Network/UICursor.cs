using UnityEngine;
using UnityEngine.InputSystem;

public class UICursor : MonoBehaviour
{
    [SerializeField] private RectTransform cursor;
    [SerializeField] private Canvas canvas;
    [SerializeField] private float cursorCenterOffset = 15f;

    void Awake()
    {
        // Hide hardware cursor
        Cursor.visible = false;
    }

    void Update()
    {
        if (Mouse.current == null)
            return;

        Vector2 screenPos = Mouse.current.position.ReadValue();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null
                : canvas.worldCamera,
            out Vector2 localPos
        );

        cursor.localPosition = localPos + new Vector2(cursorCenterOffset, -cursorCenterOffset);
    }

    void OnDisable()
    {
        Cursor.visible = true;
    }
}
