using UnityEngine;

[System.Serializable]
public class AnimalData
{
    public string animalName;
    public string habitat;
    public string food;
    public string characteristics;
    public GameObject modelPrefab;
    public Sprite icon;
    
    public AnimalData(string name, string habitat, string food, string characteristics)
    {
        this.animalName = name;
        this.habitat = habitat;
        this.food = food;
        this.characteristics = characteristics;
    }
}
