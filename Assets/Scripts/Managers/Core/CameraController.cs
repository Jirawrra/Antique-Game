using UnityEngine;
using System.Collections;
public class CameraController : MonoBehaviour
{

     [Header("Settings")]
    public float panSpeed = 20f;
    public Vector2 minBounds; 
    public Vector2 maxBounds; 

    private Vector3 lastMousePos;
    


    void Update()
    {
        // Start Drag
        if (Input.GetMouseButtonDown(0)) 
        {
            lastMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Debug.Log("Dragging");
        }

        // Perform Drag
        if (Input.GetMouseButton(0))
        {
            Vector3 currentMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Vector3 direction = lastMousePos - currentMousePos;

            // Move camera (X and Y for 2D, or X and Z for 3D/Top-down)
            Vector3 targetPos = transform.position + new Vector3(direction.x * panSpeed, direction.y * panSpeed, 0);

            //Apply Clamp
            targetPos.x = Mathf.Clamp(targetPos.x, minBounds.x, maxBounds.x);
            targetPos.y = Mathf.Clamp(targetPos.y, minBounds.y, maxBounds.y);

            transform.position = targetPos;
            lastMousePos = currentMousePos;
        }
    }
  
}
