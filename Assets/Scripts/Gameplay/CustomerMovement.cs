using UnityEngine;
using System.Collections;

public class CustomerFloatMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public Vector2 moveSpeedRange = new Vector2(1f, 3f);
    public float stoppingDistance = 0.1f;
    public Vector2 pauseDurationRange = new Vector2(1f, 3f);

    [Header("Easing")]
    [Range(0.05f, 1f)]
    public float easingFactor = 0.1f; // Lower = slower ease near target

    private Bounds bounds;
    private Vector3 targetPosition;
    private float moveSpeed;
    private bool isMoving = true;

    void Awake()
    {
        if (CustomerMovementBounds.Instance != null)
        {
            bounds = CustomerMovementBounds.Instance.GetBounds();
            PickNewTarget();
        }
        else
        {
            Debug.LogError("CustomerMovementBounds.Instance is null! Make sure it exists in the scene.");
        }
    }

    void Update()
    {
        if (!isMoving || CustomerMovementBounds.Instance == null) return;

        MoveToTarget();
    }

    void MoveToTarget()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * easingFactor * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < stoppingDistance)
        {
            isMoving = false;
            StartCoroutine(PauseBeforeNextTarget());
        }
    }

    void PickNewTarget()
    {
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        targetPosition = new Vector3(randomX, randomY, transform.position.z);
        moveSpeed = Random.Range(moveSpeedRange.x, moveSpeedRange.y);

        isMoving = true;

    }

    IEnumerator PauseBeforeNextTarget()
    {
        float pauseTime = Random.Range(pauseDurationRange.x, pauseDurationRange.y);
        yield return new WaitForSeconds(pauseTime);

        PickNewTarget();
    }
}