using UnityEngine;

// Attach this to each of your 5 sphere GameObjects.
public class SphereBehaviour : MonoBehaviour
{
    [SerializeField]
    private float radius = 1f;

    public float Radius => radius;

    private void OnValidate()
    {
        ApplyScale();
    }

    public void ApplyScale()
    {
        // A default Unity sphere primitive has a radius of 0.5 at scale 1,
        // so we double it here to make "radius" mean what it says.
        transform.localScale = Vector3.one * (radius * 2f);
    }
}