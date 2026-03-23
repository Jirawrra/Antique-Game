using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    public static FloatingTextManager Instance;

    [SerializeField] private FloatingText floatingTextPrefab;
    [SerializeField] private Transform canvasTransform; // World Space Canvas

    private void Awake()
    {
        Instance = this;
    }

    public void Spawn(string message, Vector3 worldPosition)
    {
        // Optional slight randomness (feels better)
        Vector3 offset = new Vector3(
            Random.Range(-0.5f, 0.5f),
            Random.Range(0.5f, 1f),
            0f
        );

        FloatingText text = Instantiate(
           floatingTextPrefab,
           worldPosition + offset,
           Quaternion.identity,
           canvasTransform
       );

        text.Setup(message);
    }
}