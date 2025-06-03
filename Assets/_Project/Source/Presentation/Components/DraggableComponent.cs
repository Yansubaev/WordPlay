using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Source.Presentation.Components
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
        private Vector2 dragOffset;
        #endregion

        public event Action<PointerEventData> OnDragStarted;
        public event Action<PointerEventData> OnDragging;
        public event Action<PointerEventData> OnDragEnded;

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

                        // Position the object with bottom-right corner at pointer
                        PositionObjectBottomRightAtPointer(eventData);

                        // NOW fire the OnDragStarted event, since we've reached the threshold
                        if (!dragStartedEventFired)
                        {
                            dragStartedEventFired = true;
                            OnDragStarted?.Invoke(eventData);
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
                // Get the screen position of the pointer
                Vector2 pointerPosition = eventData.position;

                // Calculate the offset needed for the bottom-right corner in screen space
                Vector2 pivotOffset = CalculatePivotOffsetInScreenSpace();

                // Convert the desired position (pointer - offset) to rectTransform position
                Vector2 targetPositionScreen = pointerPosition - pivotOffset;
                Vector3 targetPositionWorld;

                // Convert screen position to world position
                if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                {
                    // For overlay canvas, screen position is directly usable
                    targetPositionWorld = targetPositionScreen;
                    targetPositionWorld.z = rectTransform.position.z; // Keep original z
                }
                else
                {
                    // For camera-based canvas, use camera to convert
                    Camera cam = canvas.worldCamera ?? Camera.main;
                    targetPositionWorld = cam.ScreenToWorldPoint(new Vector3(targetPositionScreen.x, targetPositionScreen.y, canvas.planeDistance));
                }

                // Set position directly in world space
                rectTransform.position = targetPositionWorld;

                // Notify listeners
                OnDragging?.Invoke(eventData);
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

                // Notify listeners
                OnDragEnded?.Invoke(eventData);
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
        /// Positions the object so its bottom-right corner is at the pointer position
        /// Works correctly even if parent changes
        /// </summary>
        private void PositionObjectBottomRightAtPointer(PointerEventData eventData)
        {
            // Get the screen position of the pointer
            Vector2 pointerPosition = eventData.position;

            // Calculate the offset needed for the bottom-right corner in screen space
            Vector2 pivotOffset = CalculatePivotOffsetInScreenSpace();

            // Convert the desired position (pointer - offset) to rectTransform position
            // This works regardless of parent changes
            Vector2 targetPositionScreen = pointerPosition - pivotOffset;
            Vector3 targetPositionWorld;

            // Convert screen position to world position
            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                // For overlay canvas, screen position is directly usable
                targetPositionWorld = targetPositionScreen;
                targetPositionWorld.z = rectTransform.position.z; // Keep original z
            }
            else
            {
                // For camera-based canvas, use camera to convert
                Camera cam = canvas.worldCamera ?? Camera.main;
                targetPositionWorld = cam.ScreenToWorldPoint(new Vector3(targetPositionScreen.x, targetPositionScreen.y, canvas.planeDistance));
            }

            // Set position directly in world space
            rectTransform.position = targetPositionWorld;
        }

        /// <summary>
        /// Calculates the offset from screen position to position the bottom-right corner at the pointer
        /// Takes into account pivot, canvas scaling, and render mode
        /// </summary>
        private Vector2 CalculatePivotOffsetInScreenSpace()
        {
            // Get the rect's size in screen space
            Vector2 rectSize = rectTransform.rect.size;

            // Apply canvas scaling
            rectSize.x *= canvas.scaleFactor;
            rectSize.y *= canvas.scaleFactor;

            // Calculate pivot offset in screen space
            // For right-bottom corner:
            // X: We want the right edge, so use (1 - pivot.x) * width
            // Y: We want the bottom edge, so use (0 - pivot.y) * height
            float pivotOffsetX = (1 - rectTransform.pivot.x) * rectSize.x;
            float pivotOffsetY = (0 - rectTransform.pivot.y) * rectSize.y; // For bottom edge

            return new Vector2(pivotOffsetX, pivotOffsetY);
        }

        /// <summary>
        /// Calculates the offset from anchoredPosition to position the bottom-right corner at the pointer
        /// Takes into account pivot and anchors
        /// </summary>
        private Vector2 CalculatePivotOffset()
        {
            // Get the rect's size
            Vector2 rectSize = rectTransform.rect.size;

            // Calculate pivot offset
            // For bottom-right corner positioning:
            // X: offset from pivot to right edge
            // Y: offset from pivot to bottom edge
            float pivotOffsetX = (1 - rectTransform.pivot.x) * rectSize.x;
            float pivotOffsetY = (0 - rectTransform.pivot.y) * rectSize.y; // For bottom edge

            return new Vector2(pivotOffsetX, pivotOffsetY);
        }

        #endregion
    }
}