using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 normalSize;
    public Vector3 hoverSize;

    void Start()
    {
        normalSize = transform.localScale;
        hoverSize = normalSize * 1.1f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = hoverSize;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = normalSize;
    }
}