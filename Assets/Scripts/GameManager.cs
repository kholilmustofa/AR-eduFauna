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
            InitializeAnimals();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void InitializeAnimals()
    {
        // Initialize animal data
        // You can add more animals here or load from a database
        if (animals.Count == 0)
        {
            // Example animals - you'll need to assign prefabs and icons in the Inspector
            animals.Add(new AnimalData(
                "Gajah",
                "Hutan tropis dan savana",
                "Rumput, daun, buah-buahan",
                "Mamalia terbesar di darat dengan belalai panjang yang digunakan untuk makan dan minum"
            ));
            
            animals.Add(new AnimalData(
                "Harimau",
                "Hutan hujan tropis",
                "Daging (karnivora)",
                "Kucing besar dengan loreng hitam dan oranye, pemburu yang sangat tangkas"
            ));
            
            animals.Add(new AnimalData(
                "Panda",
                "Hutan bambu pegunungan",
                "Bambu",
                "Beruang hitam putih yang menggemaskan, menghabiskan waktu untuk makan bambu"
            ));
            
            animals.Add(new AnimalData(
                "Jerapah",
                "Savana Afrika",
                "Daun akasia",
                "Mamalia tertinggi dengan leher panjang untuk mencapai daun di pohon tinggi"
            ));
            
            animals.Add(new AnimalData(
                "Penguin",
                "Antartika dan daerah dingin",
                "Ikan dan krill",
                "Burung yang tidak bisa terbang tetapi ahli berenang di air dingin"
            ));
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
