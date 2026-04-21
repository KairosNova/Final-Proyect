using UnityEngine;

public class YSort : MonoBehaviour
{
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }
}