using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Animal Database")]
    public List<AnimalData> animals = new List<AnimalData>();
    
    [Header("Current Selection")]
    public AnimalData selectedAnimal;
    
    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Load icons for animals from Inspector
            LoadIconsForExistingAnimals();
            
            // Debug: Check if model prefabs are assigned
            Debug.Log($"[GameManager] Menggunakan {animals.Count} hewan dari Inspector");
            foreach (var animal in animals)
            {
                if (animal.modelPrefab == null)
                {
                    Debug.LogError($"{animal.animalName} - Model Prefab KOSONG! Assign di Inspector!");
                }
                else
                {
                    Debug.Log($" {animal.animalName} - Model: {animal.modelPrefab.name}");
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    
    private Sprite LoadAnimalIcon(string iconName)
    {
        // Load sprite from UI/Illustrations/Animals folder
        // Note: The folder must be inside a Resources folder for this to work
        // Alternative: Use direct reference in Inspector
        Sprite sprite = Resources.Load<Sprite>($"UI/Illustrations/Animals/{iconName}");
        
        if (sprite == null)
        {
            Debug.LogWarning($"Could not load icon: {iconName}. Make sure the image is in a Resources folder or assign it manually in the Inspector.");
        }
        
        return sprite;
    }
    
    
    private void LoadIconsForExistingAnimals()
    {
        // Auto-load icons for animals that don't have icons yet
        string[] iconNames = { "elephant", "lion", "orca", "pinguin", "zebra" };
        
        for (int i = 0; i < animals.Count && i < iconNames.Length; i++)
        {
            if (animals[i].icon == null)
            {
                animals[i].icon = LoadAnimalIcon(iconNames[i]);
            }
        }
    }
    
    public void SelectAnimal(int index)
    {
        if (index >= 0 && index < animals.Count)
        {
            selectedAnimal = animals[index];
            Debug.Log("Selected animal: " + selectedAnimal.animalName);
        }
    }
    
    public void SelectAnimal(AnimalData animal)
    {
        selectedAnimal = animal;
        Debug.Log("Selected animal: " + selectedAnimal.animalName);
    }
}
