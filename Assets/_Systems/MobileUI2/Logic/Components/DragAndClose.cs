using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AtlasSpace.UI
{
    public class DragAndClose : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public bool on = true;
        [SerializeField] MaxHeightType maxHeightType;
        [SerializeField] Canvas canvas;
        private Vector3 initialPoint;
        private RectTransform _rect;

        private Vector3 dragStartPoint;
        [SerializeField] float minumumDragAmountToClose;

        float screenMinY;
        float screenMaxY;

        private Coroutine moveCoroutine;
        float targetPosition;
        [SerializeField] float duration;
        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            initialPoint = _rect.position;

            screenMinY = -(Screen.height / 2);
            //float a = screenMaxY = (Screen.height / 2) + (_rect.rect.height / 2); ; 
            //float b = screenMaxY = _rect.position.y + (_rect.rect.height / 2);

            if (maxHeightType == MaxHeightType.Screen)
            {
                screenMaxY = Screen.height - (_rect.rect.height / 2);
            }
            else
            {
                screenMaxY = _rect.position.y;
            }


            targetPosition = screenMinY - (_rect.sizeDelta.y / 2);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            dragStartPoint = _rect.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (on == false)
                return;

            var dragAmount = eventData.delta / canvas.scaleFactor;
            if (dragAmount.y > 0 && _rect.position.y > screenMaxY)
                return;

            _rect.anchoredPosition += new Vector2(0, dragAmount.y);

            //pivot altına gecerse
            if (_rect.position.y < screenMinY)
            {
                if (moveCoroutine == null)
                {
                    moveCoroutine = StartCoroutine(MoveDown());
                }
            }

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (on == false)
                return;
            Vector3 dragEndPoint = _rect.position;

            float dragAmount = Mathf.Abs(dragEndPoint.y - dragStartPoint.y);
            if (dragAmount > minumumDragAmountToClose && dragStartPoint.y > dragEndPoint.y)
            {
                if (moveCoroutine == null)
                {
                    moveCoroutine = StartCoroutine(MoveDown());
                }
            }
            else
            {
                if (moveCoroutine == null)
                {
                    moveCoroutine = StartCoroutine(MoveUp());
                }
            }

        }

        private IEnumerator MoveDown()
        {
            float startY = _rect.position.y;
            float time = 0f;
            while (time < duration)
            {
                float nextPos = Mathf.Lerp(startY, targetPosition, time / duration);
                _rect.position = new Vector3(_rect.position.x, nextPos);
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            moveCoroutine = null;
            gameObject.SetActive(false);
            _rect.position = initialPoint;
        }

        public void Close()
        {
            moveCoroutine = StartCoroutine(MoveDown());
        }

        private IEnumerator MoveUp()
        {
            float startY = _rect.position.y;
            float time = 0f;
            while (time < duration)
            {
                float nextPos = Mathf.Lerp(startY, initialPoint.y, time / duration);
                _rect.position = new Vector3(_rect.position.x, nextPos);
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            moveCoroutine = null;
        }

        public void Open()
        {
            _rect.position = new Vector3(_rect.position.x, targetPosition);
            if (moveCoroutine == null)
            {
                moveCoroutine = StartCoroutine(MoveUp());
            }
        }
    }

    public enum MaxHeightType
    {
        CurrentPos,
        Screen
    }
}
