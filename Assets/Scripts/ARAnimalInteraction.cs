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
        // MOUSE FALLBACK FOR EDITOR TESTING
        #if UNITY_EDITOR
        else if (Input.GetMouseButton(0))
            HandleMouseInteraction();
        #endif
    }

    void HandleMouseInteraction()
    {
        // ROTASI: Klik Kiri + Geser Mouse (Horizontal)
        float rotSpeedModifier = rotationSpeed * 50f;
        float mouseX = Input.GetAxis("Mouse X");
        
        if (Mathf.Abs(mouseX) > 0.1f)
        {
            transform.Rotate(Vector3.up, -mouseX * rotSpeedModifier * Time.deltaTime, Space.World);
        }
    }

    // ======================================================
    // SINGLE TOUCH (DRAG OR ROTATE)
    // ======================================================
    void HandleSingleTouch()
    {
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            // Cek apakah menyentuh hewan
            isDragging = IsTouchingObject(touch.position, out RaycastHit hit);
            if (isDragging)
            {
                yOffsetToPlane = transform.position.y - hit.point.y;
            }
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            // ROTASI: 1 Jari (Selalu bisa rotasi jika tidak sedang menyeret posisi)
            if (enableRotation && Mathf.Abs(touch.deltaPosition.x) > 1.0f)
            {
                float rotSpeedModifier = rotationSpeed * 20f;
                transform.Rotate(Vector3.up, -touch.deltaPosition.x * rotSpeedModifier * Time.deltaTime, Space.World);
            }
            
            // DRAG: Pindah posisi (Hanya jika awal touch kena badan hewan)
            if (enableDrag && isDragging && Mathf.Abs(touch.deltaPosition.y) > Mathf.Abs(touch.deltaPosition.x))
            {
                DragWithPhysics(touch.position);
            }
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            isDragging = false;
        }
    }

    void DragWithPhysics(Vector2 screenPos)
    {
        Ray ray = arCamera.ScreenPointToRay(screenPos);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);
            transform.position = new Vector3(targetPoint.x, transform.position.y, targetPoint.z);
        }
    }

    void HandleTwoFingerGesture()
    {
        Touch t0 = Input.GetTouch(0);
        Touch t1 = Input.GetTouch(1);

        if (t0.phase == TouchPhase.Began || t1.phase == TouchPhase.Began)
        {
            initialPinchDistance = Vector2.Distance(t0.position, t1.position);
            baseScale = transform.localScale;
        }
        else if (t0.phase == TouchPhase.Moved || t1.phase == TouchPhase.Moved)
        {
            // ZOOM: Pinch with 2 fingers
            if (enableScale && initialPinchDistance > 0.01f)
            {
                float currentDistance = Vector2.Distance(t0.position, t1.position);
                float factor = currentDistance / initialPinchDistance;
                Vector3 targetScale = baseScale * factor;

                // Batas Scale
                float minS = initialModelScale.x * minScaleMultiplier;
                float maxS = initialModelScale.x * maxScaleMultiplier;
                targetScale.x = Mathf.Clamp(targetScale.x, minS, maxS);
                targetScale.y = Mathf.Clamp(targetScale.y, minS, maxS);
                targetScale.z = Mathf.Clamp(targetScale.z, minS, maxS);

                transform.localScale = targetScale;
            }
        }
    }

    bool IsTouchingObject(Vector2 screenPos, out RaycastHit hit)
    {
        Ray ray = arCamera.ScreenPointToRay(screenPos);
        // Raycast all untuk memastikan tidak terhalang collider transparan
        RaycastHit[] allHits = Physics.RaycastAll(ray, 100f);
        
        foreach (var h in allHits)
        {
            if (h.transform == transform || h.transform.IsChildOf(transform))
            {
                hit = h;
                return true;
            }
        }

        hit = new RaycastHit();
        return false;
    }
}
