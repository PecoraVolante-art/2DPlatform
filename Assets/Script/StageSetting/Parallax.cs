using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float startPos, length;
    public Transform cameraTransform;
    public float parallaxEffect;

    void Start()
    {
        startPos = transform.position.x;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            length = sr.bounds.size.x;
        }
    }

    void Update()
    {
        float distance = cameraTransform.position.x * parallaxEffect;
        float movement = cameraTransform.position.x * (1 - parallaxEffect);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (movement > startPos + length)
        {
            startPos += length;
        }
        else if (movement < startPos - length)
        {
            startPos -= length;
        }
    }
}