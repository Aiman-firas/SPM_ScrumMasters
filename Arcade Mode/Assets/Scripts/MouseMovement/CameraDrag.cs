using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    public float dragSpeed = 2.0f;
    private Vector3 dragOrigin;



    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += difference;
        }

    }
}
