using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerSizeChecker : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        GameManager manager = (GameManager)target;
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("=== CEK UKURAN MODEL ===", EditorStyles.boldLabel);
        
        if (manager.animals != null && manager.animals.Count > 0)
        {
            foreach (var animal in manager.animals)
            {
                if (animal.modelPrefab != null)
                {
                    EditorGUILayout.BeginVertical("box");
                    EditorGUILayout.LabelField($"🐾 {animal.animalName}", EditorStyles.boldLabel);
                    
                    // Hitung bounds asli (scale 1.0)
                    Renderer[] renderers = animal.modelPrefab.GetComponentsInChildren<Renderer>(true);
                    if (renderers.Length > 0)
                    {
                        Bounds bounds = renderers[0].bounds;
                        foreach (Renderer r in renderers)
                        {
                            bounds.Encapsulate(r.bounds);
                        }
                        
                        Vector3 originalSize = bounds.size;
                        
                        EditorGUILayout.LabelField("Ukuran Asli (Scale 1.0):");
                        EditorGUILayout.LabelField($"  Lebar: {originalSize.x:F2}m | Tinggi: {originalSize.y:F2}m | Panjang: {originalSize.z:F2}m");
                        
                        // Hitung dengan scale AR (0.02)
                        float arScale = 0.02f;
                        Vector3 arSize = originalSize * arScale;
                        
                        EditorGUILayout.LabelField($"Ukuran di AR (Scale {arScale}):");
                        EditorGUILayout.LabelField($"  Lebar: {arSize.x:F2}m | Tinggi: {arSize.y:F2}m | Panjang: {arSize.z:F2}m");
                        
                        // Perbandingan
                        if (arSize.y > 2f)
                            EditorGUILayout.HelpBox("⚠️ TERLALU TINGGI! Lebih tinggi dari manusia!", MessageType.Warning);
                        else if (arSize.y < 0.1f)
                            EditorGUILayout.HelpBox("⚠️ TERLALU KECIL! Seperti mainan!", MessageType.Warning);
                        else
                            EditorGUILayout.HelpBox("✅ Ukuran wajar untuk AR", MessageType.Info);
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("❌ Tidak ada Renderer!", MessageType.Error);
                    }
                    
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();
                }
                else
                {
                    EditorGUILayout.HelpBox($"❌ {animal.animalName} - Model Prefab NULL!", MessageType.Error);
                }
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Tidak ada hewan di list", MessageType.Info);
        }
    }
}
#endif
