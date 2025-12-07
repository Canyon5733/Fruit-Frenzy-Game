using UnityEngine;

public class HeartAnimation : MonoBehaviour
{
    public float speed = 2f;
    public float scaleAmount = 0.3f;

    Vector3 baseScale;

    void Start()
    {
        baseScale = transform.localScale;
    }

    void Update()
    {
        float scale = 1 + Mathf.Sin(Time.time * speed) * scaleAmount;
        transform.localScale = baseScale * scale;
    }
}
