using UnityEngine;
using UnityEngine.EventSystems;

namespace NagaisoraFamework
{
    public class ImageDrap : CommMonoScriptObject, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public RectTransform rectTransform;
        public Canvas canvas;

        //开始拖拽
        public void OnBeginDrag(PointerEventData eventData)
        {

        }

        //拖拽中
        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        //停止拖拽
        public void OnEndDrag(PointerEventData eventData)
        {

        }
    }
}