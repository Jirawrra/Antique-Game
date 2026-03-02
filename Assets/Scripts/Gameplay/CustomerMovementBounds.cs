using UnityEngine;

public class CustomerMovementBounds : MonoBehaviour
{
    public BoxCollider2D boundsCollider;

    public static CustomerMovementBounds Instance;

    void Awake()
    {
        Instance = this;
    }

    public Bounds GetBounds()
    {
        return boundsCollider.bounds;
    }
    void OnDrawGizmos()
    {
        if (boundsCollider == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(boundsCollider.bounds.center, boundsCollider.bounds.size);
    }
}
