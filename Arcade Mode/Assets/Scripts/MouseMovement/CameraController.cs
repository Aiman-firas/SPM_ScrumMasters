using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zoomSpeed = 2.0f;
    public float minZoom = 5f;
    public float maxZoom = 15f;

    void Update()
    {
        // Zooming
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float newSize = Camera.main.orthographicSize - scroll * zoomSpeed * 100f;
        newSize = Mathf.Clamp(newSize, minZoom, maxZoom);

        // Apply new zoom
        Camera.main.orthographicSize = newSize;
    }
}
