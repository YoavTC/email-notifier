using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableWindow : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform draggingArea;
    
    private bool isDragging;
    private Vector3 dragPointOffset;
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = RectTransformUtility.RectangleContainsScreenPoint(draggingArea, Input.mousePosition, null);
        if (isDragging)
        {
            Application.targetFrameRate = (int) Screen.currentResolution.refreshRateRatio.value;
            dragPointOffset = Input.mousePosition - transform.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            transform.position = Input.mousePosition - dragPointOffset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Application.targetFrameRate = 24;
    }
}
