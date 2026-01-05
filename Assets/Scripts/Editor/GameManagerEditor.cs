using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        GameManager gameManager = (GameManager)target;
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Animal Database Tools", EditorStyles.boldLabel);
        
        if (GUILayout.Button("Initialize Animal Data"))
        {
            InitializeAnimalData(gameManager);
            EditorUtility.SetDirty(gameManager);
        }
        
        if (GUILayout.Button("Clear Animal Data"))
        {
            gameManager.animals.Clear();
            EditorUtility.SetDirty(gameManager);
        }
        
        if (GUILayout.Button("Auto-Load Icons"))
        {
            AutoLoadIcons(gameManager);
            EditorUtility.SetDirty(gameManager);
        }
    }
    
    private void InitializeAnimalData(GameManager gm)
    {
        gm.animals.Clear();
        
        // 1. Gajah (Elephant)
        AnimalData elephant = new AnimalData(
            "Gajah",
            "Hutan tropis dan savana Afrika",
            "Rumput, daun, kulit kayu, buah-buahan",
            "Mamalia terbesar di darat dengan belalai panjang yang multifungsi. Memiliki ingatan yang sangat baik dan hidup berkelompok."
        );
        elephant.icon = Resources.Load<Sprite>("UI/Illustrations/Animals/elephant");
        gm.animals.Add(elephant);
        
        // 2. Singa (Lion)
        AnimalData lion = new AnimalData(
            "Singa",
            "Savana dan padang rumput Afrika",
            "Daging (karnivora) - zebra, rusa, kerbau",
            "Raja hutan dengan surai megah pada jantan. Hidup berkelompok dalam pride dan berburu secara berkelompok."
        );
        lion.icon = Resources.Load<Sprite>("UI/Illustrations/Animals/lion");
        gm.animals.Add(lion);
        
        // 3. Paus Orca (Orca/Killer Whale)
        AnimalData orca = new AnimalData(
            "Paus Orca",
            "Samudra di seluruh dunia, terutama perairan dingin",
            "Ikan, anjing laut, penguin, bahkan paus lain",
            "Predator puncak laut yang sangat cerdas. Hidup berkelompok dan berkomunikasi dengan suara khas."
        );
        orca.icon = Resources.Load<Sprite>("UI/Illustrations/Animals/orca");
        gm.animals.Add(orca);
        
        // 4. Penguin
        AnimalData penguin = new AnimalData(
            "Penguin",
            "Antartika dan daerah dingin belahan selatan",
            "Ikan, krill, cumi-cumi",
            "Burung yang tidak bisa terbang tetapi ahli berenang. Setia pada pasangan dan merawat anak bersama-sama."
        );
        penguin.icon = Resources.Load<Sprite>("UI/Illustrations/Animals/pinguin");
        gm.animals.Add(penguin);
        
        // 5. Zebra
        AnimalData zebra = new AnimalData(
            "Zebra",
            "Padang rumput dan savana Afrika",
            "Rumput, dedaunan, kulit kayu",
            "Kuda liar dengan loreng hitam-putih unik pada setiap individu. Hidup berkelompok untuk perlindungan dari predator."
        );
        zebra.icon = Resources.Load<Sprite>("UI/Illustrations/Animals/zebra");
        gm.animals.Add(zebra);
        
        Debug.Log("Animal data initialized! Total animals: " + gm.animals.Count);
    }
    
    private void AutoLoadIcons(GameManager gm)
    {
        string[] iconNames = { "elephant", "lion", "orca", "pinguin", "zebra" };
        
        for (int i = 0; i < gm.animals.Count && i < iconNames.Length; i++)
        {
            Sprite icon = Resources.Load<Sprite>($"UI/Illustrations/Animals/{iconNames[i]}");
            if (icon != null)
            {
                gm.animals[i].icon = icon;
                Debug.Log($"Loaded icon for {gm.animals[i].animalName}");
            }
        }
        
        Debug.Log("Icons loaded!");
    }
}
