using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    public Camera cam;
    public float minSize = 2f;
    public float maxSize = 8f;
    public float zoomSpeed = 0.02f;

    void Start()
    {
        if (cam == null)
            cam = Camera.main;
    }

    void Update()
    {
        if (cam == null) return;

        // Mouse wheel no PC
        if (Mouse.current != null)
        {
            float scroll = Mouse.current.scroll.ReadValue().y;
            if (scroll != 0)
            {
                cam.orthographicSize -= scroll * zoomSpeed;
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minSize, maxSize);
            }
        }

        // Pinch no celular
        if (Touchscreen.current != null && Touchscreen.current.touches.Count >= 2)
        {
            var touch0 = Touchscreen.current.touches[0];
            var touch1 = Touchscreen.current.touches[1];

            if (touch0.press.isPressed && touch1.press.isPressed)
            {
                Vector2 t0 = touch0.position.ReadValue();
                Vector2 t1 = touch1.position.ReadValue();

                Vector2 t0Prev = t0 - touch0.delta.ReadValue();
                Vector2 t1Prev = t1 - touch1.delta.ReadValue();

                float prevDist = Vector2.Distance(t0Prev, t1Prev);
                float currDist = Vector2.Distance(t0, t1);

                float diff = currDist - prevDist;

                cam.orthographicSize -= diff * 0.01f;
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minSize, maxSize);
            }
        }
    }
}