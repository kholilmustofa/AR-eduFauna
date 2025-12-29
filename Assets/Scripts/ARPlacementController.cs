using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

[RequireComponent(typeof(ARRaycastManager))]
public class ARPlacementController : MonoBehaviour
{
    [Header("AR Components")]
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;
    
    [Header("Placement Settings")]
    public GameObject placementIndicator;
    public float indicatorRotationSpeed = 50f;
    
    [Header("Current Animal")]
    private GameObject spawnedAnimal;
    private bool isAnimalPlaced = false;
    
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    
    private void Start()
    {
        // Auto-find components if not assigned
        if (raycastManager == null)
            raycastManager = GetComponent<ARRaycastManager>();
        
        if (planeManager == null)
            planeManager = FindFirstObjectByType<ARPlaneManager>();
        
        // Hide indicator initially
        if (placementIndicator != null)
        {
            placementIndicator.SetActive(false);
        }
    }
    
    private void Update()
    {
        // Update placement pose
        UpdatePlacementPose();
        
        // Update placement indicator
        UpdatePlacementIndicator();
        
        // Handle touch input for placement
        if (placementPoseIsValid && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                // Check if touch is not on UI
                if (!IsPointerOverUIObject(touch.position))
                {
                    PlaceAnimal();
                }
            }
        }
        
        // Editor testing with mouse
        #if UNITY_EDITOR
        if (placementPoseIsValid && Input.GetMouseButtonDown(0))
        {
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                PlaceAnimal();
            }
        }
        #endif
    }
    
    private void UpdatePlacementPose()
    {
        Vector3 screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        
        hits.Clear();
        placementPoseIsValid = raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon);
        
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;
            
            // Adjust rotation to face camera
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }
    
    private void UpdatePlacementIndicator()
    {
        if (placementIndicator != null)
        {
            if (placementPoseIsValid && !isAnimalPlaced)
            {
                placementIndicator.SetActive(true);
                placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
                
                // Rotate indicator
                placementIndicator.transform.Rotate(Vector3.up, indicatorRotationSpeed * Time.deltaTime);
            }
            else
            {
                placementIndicator.SetActive(false);
            }
        }
    }
    
    private void PlaceAnimal()
    {
        if (GameManager.Instance == null || GameManager.Instance.selectedAnimal == null)
        {
            Debug.LogWarning("No animal selected!");
            return;
        }
        
        // Remove existing animal if any
        if (spawnedAnimal != null)
        {
            Destroy(spawnedAnimal);
        }
        
        // Get the animal prefab
        GameObject animalPrefab = GameManager.Instance.selectedAnimal.modelPrefab;
        
        if (animalPrefab == null)
        {
            Debug.LogError("Animal prefab is null!");
            return;
        }
        
        // Instantiate the animal
        spawnedAnimal = Instantiate(animalPrefab, placementPose.position, placementPose.rotation);
        
        // Add AR interaction component
        ARAnimalInteraction interaction = spawnedAnimal.GetComponent<ARAnimalInteraction>();
        if (interaction == null)
        {
            interaction = spawnedAnimal.AddComponent<ARAnimalInteraction>();
        }
        
        isAnimalPlaced = true;
        
        // Notify UI
        ARUIController uiController = FindFirstObjectByType<ARUIController>();
        if (uiController != null)
        {
            uiController.OnAnimalPlaced();
        }
        
        Debug.Log("Animal placed: " + GameManager.Instance.selectedAnimal.animalName);
    }
    
    public void RemoveAnimal()
    {
        if (spawnedAnimal != null)
        {
            Destroy(spawnedAnimal);
            spawnedAnimal = null;
            isAnimalPlaced = false;
        }
    }
    
    public bool IsAnimalPlaced()
    {
        return isAnimalPlaced && spawnedAnimal != null;
    }
    
    public GameObject GetSpawnedAnimal()
    {
        return spawnedAnimal;
    }
    
    private bool IsPointerOverUIObject(Vector2 touchPosition)
    {
        UnityEngine.EventSystems.PointerEventData eventDataCurrentPosition = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
        eventDataCurrentPosition.position = touchPosition;
        
        List<UnityEngine.EventSystems.RaycastResult> results = new List<UnityEngine.EventSystems.RaycastResult>();
        UnityEngine.EventSystems.EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        
        return results.Count > 0;
    }
}
