using UnityEngine;
using Vuforia;

public class VuforiaAnimalSpawner : MonoBehaviour
{
    private GameObject spawnedAnimal;
    private bool hasSpawned = false;
    
    void Start()
    {
        // Subscribe to Ground Plane Stage events
        DefaultObserverEventHandler observerHandler = GetComponent<DefaultObserverEventHandler>();
        if (observerHandler != null)
        {
            // Spawn when plane is found
            observerHandler.OnTargetFound.AddListener(OnPlaneFound);
        }
    }
    
    void OnPlaneFound()
    {
        // Only spawn once
        if (hasSpawned) return;
        
        // Check if animal is selected
        if (GameManager.Instance == null || GameManager.Instance.selectedAnimal == null)
        {
            Debug.LogWarning("[Vuforia] No animal selected!");
            return;
        }
        
        GameObject prefab = GameManager.Instance.selectedAnimal.modelPrefab;
        if (prefab == null)
        {
            Debug.LogError("[Vuforia] Animal prefab is null!");
            return;
        }
        
        // Get spawn position from hit result
        Vector3 spawnPosition = transform.position;
        
        // Face camera
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        Quaternion spawnRotation = Quaternion.LookRotation(cameraForward);
        
        // Spawn animal
        spawnedAnimal = Instantiate(prefab, spawnPosition, spawnRotation);
        
        // Set scale from AnimalData
        float scale = GameManager.Instance.selectedAnimal.modelScale;
        if (scale <= 0) scale = 0.3f; // Default
        spawnedAnimal.transform.localScale = Vector3.one * scale;
        
        // Parent to Ground Plane Stage (untuk tracking)
        spawnedAnimal.transform.SetParent(transform);
        
        // Fix shaders
        FixMaterialShaders(spawnedAnimal);
        
        // Add collider for interaction
        AddBoxCollider(spawnedAnimal);
        
        // Add interaction script
        if (spawnedAnimal.GetComponent<ARAnimalInteraction>() == null)
        {
            spawnedAnimal.AddComponent<ARAnimalInteraction>();
        }
        
        // Setup animation
        SetupAnimation(spawnedAnimal);
        
        // Lock placement
        hasSpawned = true;
        
        // Update UI
        ARUIController uiController = FindFirstObjectByType<ARUIController>();
        if (uiController != null)
        {
            uiController.OnAnimalPlaced();
        }
        
        Debug.Log($"[Vuforia] Animal placed: {prefab.name}");
    }
    
    void FixMaterialShaders(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        
        foreach (Renderer renderer in renderers)
        {
            foreach (Material mat in renderer.materials)
            {
                if (mat.shader.name.Contains("Universal") || mat.shader.name.Contains("URP"))
                {
                    Texture mainTex = mat.mainTexture;
                    mat.shader = Shader.Find("Unlit/Texture");
                    mat.mainTexture = mainTex;
                }
            }
        }
    }
    
    void AddBoxCollider(GameObject obj)
    {
        if (obj.GetComponent<Collider>() == null)
        {
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            if (renderers.Length > 0)
            {
                Bounds bounds = renderers[0].bounds;
                foreach (Renderer r in renderers)
                    bounds.Encapsulate(r.bounds);
                
                BoxCollider collider = obj.AddComponent<BoxCollider>();
                collider.center = bounds.center - obj.transform.position;
                collider.size = bounds.size;
            }
        }
    }
    
    void SetupAnimation(GameObject obj)
    {
        Animator animator = obj.GetComponent<Animator>();
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            animator.enabled = true;
            Debug.Log("[Vuforia] Animator found and enabled");
        }
    }
}
