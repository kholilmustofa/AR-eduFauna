using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ARAnimalInteraction : MonoBehaviour
{
    [Header("Interaction Toggle")]
    public bool enableDrag = true;
    public bool enableRotation = true;
    public bool enableScale = true;

    [Header("Rotation")]
    public float rotationSpeed = 0.5f;

    [Header("Scale")]
    public float minScaleMultiplier = 0.5f;
    public float maxScaleMultiplier = 3f;

    private Camera arCamera;
    private ARRaycastManager raycastManager;

    private bool isDragging;
    private float yOffsetToPlane;

    // Pinch scale
    private float initialPinchDistance;
    private Vector3 initialModelScale;
    private Vector3 baseScale;

    // Raycast cache
    private readonly List<ARRaycastHit> hits = new();

    void Start()
    {
        arCamera = Camera.main;

        // Raycast manager is optional - only needed for drag on plane feature
        // With Vuforia, we can use simple raycasting instead
        raycastManager = FindFirstObjectByType<ARRaycastManager>();

        initialModelScale = transform.localScale;
    }

    void Update()
    {
        // IMPORTANT: Skip if touching UI
        if (UnityEngine.EventSystems.EventSystem.current != null)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    // Touching UI, skip
                    return;
                }
            }
        }
        
        if (Input.touchCount == 1)
            HandleSingleTouch();

        else if (Input.touchCount == 2)
            HandleTwoFingerGesture();
    }

    // ======================================================
    // SINGLE TOUCH (DRAG)
    // ======================================================
    void HandleSingleTouch()
    {
        if (!enableDrag) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            if (IsTouchingObject(touch.position, out RaycastHit hit))
            {
                isDragging = true;
                yOffsetToPlane = transform.position.y - hit.point.y;
                Debug.Log($"[AR Interaction] Started dragging {gameObject.name}");
            }
        }
        else if (touch.phase == TouchPhase.Moved && isDragging)
        {
            DragOnPlane(touch.position);
        }
        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            if (isDragging)
            {
                Debug.Log($"[AR Interaction] Stopped dragging {gameObject.name}");
            }
            isDragging = false;
        }
    }

    void DragOnPlane(Vector2 screenPos)
    {
        if (raycastManager == null) return;

        if (raycastManager.Raycast(screenPos, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;
            transform.position = hitPose.position + Vector3.up * yOffsetToPlane;
        }
    }

    // ======================================================
    // TWO FINGER (ROTATE + SCALE)
    // ======================================================
    void HandleTwoFingerGesture()
    {
        Touch t0 = Input.GetTouch(0);
        Touch t1 = Input.GetTouch(1);

        // Minimal 1 jari menyentuh objek
        if (!IsTouchingObject(t0.position) && !IsTouchingObject(t1.position))
            return;

        isDragging = false;

        if (t0.phase == TouchPhase.Began || t1.phase == TouchPhase.Began)
        {
            initialPinchDistance = Vector2.Distance(t0.position, t1.position);
            baseScale = transform.localScale;
            Debug.Log($"[AR Interaction] Started 2-finger gesture on {gameObject.name}");
        }
        else if (t0.phase == TouchPhase.Moved || t1.phase == TouchPhase.Moved)
        {
            HandleRotation(t0, t1);
            HandleScale(t0, t1);
        }
        else if (t0.phase == TouchPhase.Ended || t1.phase == TouchPhase.Ended)
        {
            Debug.Log($"[AR Interaction] Ended 2-finger gesture on {gameObject.name}");
        }
    }

    void HandleRotation(Touch t0, Touch t1)
    {
        if (!enableRotation) return;

        Vector2 prevPos0 = t0.position - t0.deltaPosition;
        Vector2 prevPos1 = t1.position - t1.deltaPosition;

        float prevAngle = Mathf.Atan2(
            prevPos1.y - prevPos0.y,
            prevPos1.x - prevPos0.x
        ) * Mathf.Rad2Deg;

        float currAngle = Mathf.Atan2(
            t1.position.y - t0.position.y,
            t1.position.x - t0.position.x
        ) * Mathf.Rad2Deg;

        float delta = currAngle - prevAngle;
        transform.Rotate(Vector3.up, -delta * rotationSpeed, Space.World);
    }

    void HandleScale(Touch t0, Touch t1)
    {
        if (!enableScale || initialPinchDistance <= 0f) return;

        float currentDistance = Vector2.Distance(t0.position, t1.position);
        float factor = currentDistance / initialPinchDistance;

        Vector3 newScale = baseScale * factor;

        newScale.x = Mathf.Clamp(
            newScale.x,
            initialModelScale.x * minScaleMultiplier,
            initialModelScale.x * maxScaleMultiplier
        );

        newScale.y = Mathf.Clamp(
            newScale.y,
            initialModelScale.y * minScaleMultiplier,
            initialModelScale.y * maxScaleMultiplier
        );

        newScale.z = Mathf.Clamp(
            newScale.z,
            initialModelScale.z * minScaleMultiplier,
            initialModelScale.z * maxScaleMultiplier
        );

        transform.localScale = newScale;
    }

    // ======================================================
    // HIT TEST
    // ======================================================
    bool IsTouchingObject(Vector2 screenPos)
    {
        return IsTouchingObject(screenPos, out _);
    }

    bool IsTouchingObject(Vector2 screenPos, out RaycastHit hit)
    {
        Ray ray = arCamera.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out hit))
        {
            bool isTouching = hit.transform == transform ||
                   hit.transform.IsChildOf(transform);
            
            if (isTouching)
            {
                Debug.Log($"[AR Interaction] Touch hit {hit.transform.name}");
            }
            
            return isTouching;
        }

        return false;
    }
}
