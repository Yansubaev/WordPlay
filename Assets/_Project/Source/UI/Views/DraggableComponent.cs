using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Source.UI.Components
{
    /// <summary>
    /// Component that enables drag and drop functionality for UI elements,
    /// with special handling for ScrollRect compatibility.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class DraggableComponent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IInitializePotentialDragHandler
    {
        #region private fields

        [Header("Drag Settings")]
        [SerializeField]
        private float dragThreshold = 15f;

        [Tooltip("If true, dragging occurs perpendicular to scroll direction. If false, dragging occurs in the same direction as scroll.")]
        [SerializeField]
        private bool useInverseAxis = true;

        private RectTransform rectTransform;
        private Canvas canvas;
        private CanvasGroup canvasGroup;
        private ScrollRect parentScrollRect;
        private Vector2 dragStartPosition;
        private Vector2 elementStartPosition;
        private bool isDragging = false;
        private bool thresholdReached = false;
        private bool shouldPassEventsToParent = true;
        private bool dragStartedEventFired = false;
        #endregion

        public event Action<PointerEventData, GameObject> OnDragStarted;
        public event Action<PointerEventData, GameObject> OnDragging;
        public event Action<PointerEventData, GameObject> OnDragEnded;

        #region public methods

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            // Ensure this passes through to the scroll rect
            if (parentScrollRect != null)
            {
                ExecuteEvents.Execute(parentScrollRect.gameObject, eventData, ExecuteEvents.initializePotentialDrag);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Store starting positions
            dragStartPosition = eventData.position;
            elementStartPosition = rectTransform.anchoredPosition;
            isDragging = true;
            thresholdReached = false;
            shouldPassEventsToParent = true;
            dragStartedEventFired = false;

            // Pass event to parent scroll rect initially
            if (parentScrollRect != null)
            {
                ExecuteEvents.Execute(parentScrollRect.gameObject, eventData, ExecuteEvents.beginDragHandler);
            }

            // Don't fire OnDragStarted yet - wait until threshold is reached
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isDragging) return;

            Vector2 dragDelta = eventData.position - dragStartPosition;
            float dragDistance = dragDelta.magnitude;

            // If we haven't reached the threshold yet, check if we should start dragging
            if (!thresholdReached)
            {
                bool isScrollHorizontal = parentScrollRect != null &&
                                         parentScrollRect.horizontal;

                bool isScrollVertical = parentScrollRect != null &&
                                       parentScrollRect.vertical;

                // Determine if we should activate dragging
                if (dragDistance >= dragThreshold)
                {
                    bool isHorizontalDrag = Mathf.Abs(dragDelta.x) > Mathf.Abs(dragDelta.y);

                    // Determine whether to activate dragging based on useInverseAxis
                    bool shouldActivate;

                    if (useInverseAxis)
                    {
                        // Activate when drag direction is perpendicular to scroll direction
                        shouldActivate = (isScrollHorizontal && !isScrollVertical && !isHorizontalDrag) ||
                                        (isScrollVertical && !isScrollHorizontal && isHorizontalDrag) ||
                                        (!isScrollHorizontal && !isScrollVertical);
                    }
                    else
                    {
                        // Activate when drag direction matches scroll direction
                        shouldActivate = (isScrollHorizontal && isHorizontalDrag) ||
                                        (isScrollVertical && !isHorizontalDrag) ||
                                        (!isScrollHorizontal && !isScrollVertical);
                    }

                    if (shouldActivate)
                    {
                        thresholdReached = true;
                        shouldPassEventsToParent = false;

                        // Prepare the object for dragging
                        canvasGroup.blocksRaycasts = false;
                        canvasGroup.alpha = 0.7f;

                        // NOW fire the OnDragStarted event, since we've reached the threshold
                        if (!dragStartedEventFired)
                        {
                            dragStartedEventFired = true;
                            OnDragStarted?.Invoke(eventData, FindObjectUnderPointer(eventData));
                        }
                    }
                }
            }

            // Pass event to parent scroll rect if needed
            if (shouldPassEventsToParent && parentScrollRect != null)
            {
                ExecuteEvents.Execute(parentScrollRect.gameObject, eventData, ExecuteEvents.dragHandler);
            }

            // Move the object if threshold has been reached
            if (thresholdReached)
            {
                Vector2 pointerDelta = eventData.delta / (canvas.scaleFactor > 0 ? canvas.scaleFactor : 1);
                rectTransform.anchoredPosition += pointerDelta;

                // Notify listeners
                OnDragging?.Invoke(eventData, FindObjectUnderPointer(eventData));
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // Pass event to parent if needed
            if (parentScrollRect != null && shouldPassEventsToParent)
            {
                ExecuteEvents.Execute(parentScrollRect.gameObject, eventData, ExecuteEvents.endDragHandler);
            }

            // Only fire drag ended if we actually started dragging
            if (thresholdReached)
            {
                canvasGroup.blocksRaycasts = true;
                canvasGroup.alpha = 1f;

                // Notify listeners
                GameObject hoveredObject = FindObjectUnderPointer(eventData);
                OnDragEnded?.Invoke(eventData, hoveredObject);
            }

            // Reset drag state
            isDragging = false;
            dragStartedEventFired = false;
            shouldPassEventsToParent = true;
        }

        /// <summary>
        /// Reset the object to its original position
        /// </summary>
        public void ResetPosition()
        {
            rectTransform.anchoredPosition = elementStartPosition;
        }

        #endregion

        #region private methods

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
            parentScrollRect = GetComponentInParent<ScrollRect>();
        }

        /// <summary>
        /// Find what object is currently under the pointer
        /// </summary>
        private GameObject FindObjectUnderPointer(PointerEventData eventData)
        {
            // Temporarily enable raycast blocking to prevent finding this object
            bool originalBlocksRaycast = canvasGroup.blocksRaycasts;
            canvasGroup.blocksRaycasts = false;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            // Restore original setting
            canvasGroup.blocksRaycasts = originalBlocksRaycast;

            // Return the first hit object that isn't this one
            foreach (var result in results)
            {
                if (result.gameObject != gameObject)
                {
                    return result.gameObject;
                }
            }

            return null;
        }

        #endregion
    }
}