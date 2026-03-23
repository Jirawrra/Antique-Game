using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Gameplay
{
    public class CameraController : MonoBehaviour
    {
        [Header("Pan Settings")]
        public float panSensitivity = 1.2f;
        public Vector2 minBounds;
        public Vector2 maxBounds;

        [Header("Pan Smoothing")]
        public float panSmoothTime = 0.05f;

        private Vector3 targetPosition;
        private Vector3 panVelocity;

        [Header("Return Settings")]
        public float returnDelay = 2f;
        public float returnSmoothTime = 0.3f;
        
        private Vector3 startPosition;
        private bool isReturning;
        private Coroutine returnRoutine;

        private void Start()
        {
            startPosition = transform.position;
            targetPosition = transform.position;
        }

        private void Update()
        {
            //HandleZoom();
            HandlePan();
            
            // Smooth movement toward target
            var smoothing = isReturning ? returnSmoothTime : panSmoothTime;
            transform.position = Vector3.SmoothDamp(
                transform.position,
                targetPosition,
                ref panVelocity,
                smoothing
            );
        }

        /*private void HandleZoom()
        {
            Debug.Log("Zoom: " + Mouse.current.scroll.ReadValue().y);

        }*/

        private void HandlePan()
        {
            var pointer = Pointer.current;
            if (pointer == null) return;
            
            // Check for the initial press
            if (pointer.press.wasPressedThisFrame)
            {
                // Only stop the pan if we are ACTUALLY over UI on the first click
                if (EventSystem.current && EventSystem.current.IsPointerOverGameObject())
                    return;

                if (returnRoutine != null) StopCoroutine(returnRoutine);
                isReturning = false;
            }

            if (pointer.press.isPressed)
            {
                // If we started a "Return" and then clicked again, stop it immediately
                if (isReturning) 
                {
                    isReturning = false;
                    if (returnRoutine != null) StopCoroutine(returnRoutine);
                }

                Vector2 delta = pointer.delta.ReadValue();
                
                // If there's no movement, don't calculate (saves jitter)
                if (delta == Vector2.zero) return;

                float deviceMultiplier = 0.01f; 

                Vector3 move = new Vector3(
                    -delta.x * panSensitivity * deviceMultiplier,
                    -delta.y * panSensitivity * deviceMultiplier,
                    0f
                );
                
                targetPosition += move;
                
                // Clamp bounds
                targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
                targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);
            }

            if (!Pointer.current.press.wasReleasedThisFrame) return;
            if (returnRoutine != null) StopCoroutine(returnRoutine);
            returnRoutine = StartCoroutine(ReturnToStart());
        }

        private IEnumerator ReturnToStart()
        {
            yield return new WaitForSeconds(returnDelay);
            isReturning = true;
            targetPosition = startPosition;
        }
    }
}