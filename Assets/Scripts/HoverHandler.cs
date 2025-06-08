using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Events
    public event Action HoverEnterEvent;
    public event Action HoverExitEvent;
    #endregion

    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverEnterEvent?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HoverExitEvent?.Invoke();
    }
}
