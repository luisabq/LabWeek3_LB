using UnityEngine;

// Attach this to each of your 5 cube GameObjects.
public class CubeBehaviour : MonoBehaviour
{
    [SerializeField]
    private float size = 1f;

    public float Size => size;

    // Keeps the cube's transform in sync whenever "size" changes,
    // whether from script, the default inspector, or our custom editor.
    private void OnValidate()
    {
        ApplyScale();
    }

    public void ApplyScale()
    {
        transform.localScale = Vector3.one * size;
    }
}