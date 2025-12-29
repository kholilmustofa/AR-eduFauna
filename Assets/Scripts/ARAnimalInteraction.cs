using UnityEngine;
using System.Collections.Generic;

public class ARAnimalInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public bool enableDrag = true;
    public bool enableRotation = true;
    public bool enableScale = true;
    
    [Header("Rotation Settings")]
    public float rotationSpeed = 0.5f;
    
    [Header("Scale Settings")]
    public float minScale = 0.5f;
    public float maxScale = 3f;
    public float scaleSpeed = 0.01f;
    
    private Camera arCamera;
    private bool isDragging = false;
    private Vector3 offset;
    private float initialDistance;
    private Vector3 initialScale;
    
    private void Start()
    {
        arCamera = Camera.main;
        initialScale = transform.localScale;
    }
    
    private void Update()
    {
        HandleTouchInput();
    }
    
    private void HandleTouchInput()
    {
        // Single touch - drag or rotate
        if (Input.touchCount == 1 && enableDrag)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                // Check if touching this object
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hit;
                
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == transform || hit.transform.IsChildOf(transform))
                    {
                        isDragging = true;
                    }
                }
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                // Drag the object
                DragObject(touch.position);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isDragging = false;
            }
        }
        
        // Two finger touch - rotate and scale
        if (Input.touchCount == 2)
        {
            isDragging = false;
            
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);
            
            // Check if both touches are on this object
            if (!IsTouchingObject(touch0.position) && !IsTouchingObject(touch1.position))
            {
                return;
            }
            
            if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(touch0.position, touch1.position);
            }
            else if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
            {
                // Rotation
                if (enableRotation)
                {
                    Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
                    Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
                    
                    float prevAngle = Mathf.Atan2(touch1PrevPos.y - touch0PrevPos.y, touch1PrevPos.x - touch0PrevPos.x) * Mathf.Rad2Deg;
                    float currentAngle = Mathf.Atan2(touch1.position.y - touch0.position.y, touch1.position.x - touch0.position.x) * Mathf.Rad2Deg;
                    
                    float angleDelta = currentAngle - prevAngle;
                    transform.Rotate(Vector3.up, -angleDelta * rotationSpeed, Space.World);
                }
                
                // Scale
                if (enableScale)
                {
                    float currentDistance = Vector2.Distance(touch0.position, touch1.position);
                    float scaleFactor = currentDistance / initialDistance;
                    
                    Vector3 newScale = transform.localScale * scaleFactor;
                    newScale.x = Mathf.Clamp(newScale.x, initialScale.x * minScale, initialScale.x * maxScale);
                    newScale.y = Mathf.Clamp(newScale.y, initialScale.y * minScale, initialScale.y * maxScale);
                    newScale.z = Mathf.Clamp(newScale.z, initialScale.z * minScale, initialScale.z * maxScale);
                    
                    transform.localScale = newScale;
                    initialDistance = currentDistance;
                }
            }
        }
        
        // Editor testing with mouse
        #if UNITY_EDITOR
        HandleMouseInput();
        #endif
    }
    
    private void DragObject(Vector2 screenPosition)
    {
        // Raycast to AR plane
        Ray ray = arCamera.ScreenPointToRay(screenPosition);
        
        // Try to hit AR planes
        ARPlacementController placementController = FindFirstObjectByType<ARPlacementController>();
        if (placementController != null)
        {
            List<UnityEngine.XR.ARFoundation.ARRaycastHit> hits = new List<UnityEngine.XR.ARFoundation.ARRaycastHit>();
            if (placementController.raycastManager.Raycast(screenPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;
                transform.position = hitPose.position;
            }
        }
    }
    
    private bool IsTouchingObject(Vector2 touchPosition)
    {
        Ray ray = arCamera.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            return hit.transform == transform || hit.transform.IsChildOf(transform);
        }
        
        return false;
    }
    
    #if UNITY_EDITOR
    private void HandleMouseInput()
    {
        // Simple rotation with mouse drag for testing
        if (Input.GetMouseButton(0) && enableRotation)
        {
            float rotX = Input.GetAxis("Mouse X") * rotationSpeed * 10f;
            transform.Rotate(Vector3.up, -rotX, Space.World);
        }
        
        // Scale with mouse wheel
        if (enableScale)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                Vector3 newScale = transform.localScale * (1 + scroll);
                newScale.x = Mathf.Clamp(newScale.x, initialScale.x * minScale, initialScale.x * maxScale);
                newScale.y = Mathf.Clamp(newScale.y, initialScale.y * minScale, initialScale.y * maxScale);
                newScale.z = Mathf.Clamp(newScale.z, initialScale.z * minScale, initialScale.z * maxScale);
                transform.localScale = newScale;
            }
        }
    }
    #endif
}
