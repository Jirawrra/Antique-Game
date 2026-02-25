using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class CameraController : MonoBehaviour
{

    [Header("Pan Settings")]
    public float panSpeed = 20f;
    public Vector2 minBounds;
    public Vector2 maxBounds;

    [Header("Pan Smoothing")]
    public float panSmoothTime = 0.1f;

    private Vector3 targetPosition;
    private Vector3 panVelocity;

    [Header("Return Settings")]
    public float returnDelay = 2f;
    public float returnSpeed = 2.5f;

    private Vector3 lastMousePos;
    private Vector3 startPosition;

    public float returnSmoothTime = 0.3f;
    private bool isReturning = false;
    private Coroutine returnRoutine;


    void Start()
    {
        startPosition = transform.position;
        targetPosition = transform.position;

    }

    void Update()
    {
        HandleZoom();
        HandlePan();
    }

    void HandleZoom()
    {
        Debug.Log("Zoom: " + Mouse.current.scroll.ReadValue().y);

    }
    void HandlePan()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (returnRoutine != null)
                StopCoroutine(returnRoutine);
            isReturning = false;
            lastMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Vector3 direction = lastMousePos - currentMousePos;

            targetPosition += new Vector3(
                direction.x * panSpeed,
                direction.y * panSpeed,
                0f
            );

            targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);

            lastMousePos = currentMousePos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            returnRoutine = StartCoroutine(ReturnToStart());
        }


        float smoothTime = isReturning ? returnSmoothTime : panSmoothTime;
        // Smooth movement toward target
        transform.position = Vector3.SmoothDamp(
      transform.position,
      targetPosition,
      ref panVelocity,
      panSmoothTime
      );
    }

    IEnumerator ReturnToStart()
    {
        isReturning = true;
        yield return new WaitForSeconds(returnDelay);


        targetPosition = startPosition;

        // Wait until close enough
        while (Vector3.Distance(transform.position, startPosition) > 0.01f)
            yield return null;

        transform.position = startPosition;
        isReturning = false;
    }


}
