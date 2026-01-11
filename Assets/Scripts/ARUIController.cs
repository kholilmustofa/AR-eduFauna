using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ARUIController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject instructionPanel;
    public GameObject detailPanel;
    
    [Header("Buttons")]
    public Button selectAnimalButton;
    public Button detailButton;
    public Button closeDetailButton;
    
    [Header("Detail Panel Content")]
    public TextMeshProUGUI animalNameText;
    public TextMeshProUGUI habitatText;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI characteristicsText;
    
    [Header("Instruction Text")]
    public TextMeshProUGUI instructionText;
    
    private GameObject spawnedAnimal;
    
    private void Start()
    {
        // Setup buttons
        if (selectAnimalButton != null)
        {
            selectAnimalButton.onClick.AddListener(OnSelectAnimalClicked);
        }
        
        if (detailButton != null)
        {
            detailButton.onClick.AddListener(OnDetailButtonClicked);
        }
        
        if (closeDetailButton != null)
        {
            closeDetailButton.onClick.AddListener(OnCloseDetailClicked);
        }
        
        // Initial state
        if (detailPanel != null)
        {
            detailPanel.SetActive(false);
        }
        
        if (instructionPanel != null)
        {
            instructionPanel.SetActive(true);
            UpdateInstructionText();
        }
        
        // Update detail panel with selected animal info
        UpdateDetailPanel();
    }
    
    private void Update()
    {
        // Check if animal is spawned (look for spawned object in scene)
        if (spawnedAnimal == null && GameManager.Instance != null && GameManager.Instance.selectedAnimal != null)
        {
            // Try to find spawned animal by prefab name
            string prefabName = GameManager.Instance.selectedAnimal.modelPrefab?.name;
            if (!string.IsNullOrEmpty(prefabName))
            {
                GameObject found = GameObject.Find(prefabName + "(Clone)");
                if (found != null)
                {
                    spawnedAnimal = found;
                    OnAnimalPlaced();
                }
            }
        }
        
        UpdateInstructionText();
    }
    
    private void UpdateInstructionText()
    {
        if (instructionText == null) return;
        
        if (spawnedAnimal != null)
        {
            instructionText.text = "Geser, putar, atau perbesar hewan dengan jari Anda";
        }
        else
        {
            instructionText.text = "Arahkan kamera ke permukaan datar dan ketuk untuk menempatkan hewan";
        }
    }
    
    public void OnAnimalPlaced()
    {
        // Called when animal is successfully placed
        if (instructionPanel != null)
        {
            instructionPanel.SetActive(true);
        }
    }
    
    private void OnSelectAnimalClicked()
    {
        // Return to animal selection
        SceneManager.LoadScene("AnimalSelection");
    }
    
    private void OnDetailButtonClicked()
    {
        if (spawnedAnimal == null)
        {
            // Show warning
            ShowWarning("Silakan tempatkan hewan terlebih dahulu!");
            return;
        }
        
        // Show detail panel
        if (detailPanel != null)
        {
            detailPanel.SetActive(true);
            
            // Animate panel entrance
            AnimateDetailPanel(true);
        }
    }
    
    private void OnCloseDetailClicked()
    {
        if (detailPanel != null)
        {
            AnimateDetailPanel(false);
        }
    }
    
    private void UpdateDetailPanel()
    {
        if (GameManager.Instance == null || GameManager.Instance.selectedAnimal == null)
        {
            return;
        }
        
        AnimalData animal = GameManager.Instance.selectedAnimal;
        
        if (animalNameText != null)
        {
            animalNameText.text = animal.animalName;
        }
        
        if (habitatText != null)
        {
            habitatText.text = animal.habitat;
        }
        
        if (foodText != null)
        {
            foodText.text = animal.food;
        }
        
        if (characteristicsText != null)
        {
            characteristicsText.text = animal.characteristics;
        }
        
        // Animal icon removed - not used
    }
    
    private void AnimateDetailPanel(bool show)
    {
        if (detailPanel == null) return;
        
        CanvasGroup canvasGroup = detailPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = detailPanel.AddComponent<CanvasGroup>();
        }
        
        StopAllCoroutines();
        
        if (show)
        {
            detailPanel.transform.localScale = Vector3.zero;
            canvasGroup.alpha = 0f;
            StartCoroutine(ScalePanelAnimation(Vector3.one, 0.3f));
            StartCoroutine(FadeCanvasGroup(canvasGroup, 1f, 0.2f));
        }
        else
        {
            StartCoroutine(ScalePanelAnimation(Vector3.zero, 0.2f, () => {
                detailPanel.SetActive(false);
            }));
            StartCoroutine(FadeCanvasGroup(canvasGroup, 0f, 0.2f));
        }
    }
    
    private System.Collections.IEnumerator ScalePanelAnimation(Vector3 targetScale, float duration, System.Action onComplete = null)
    {
        Vector3 startScale = detailPanel.transform.localScale;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            // Ease out back effect
            t = 1f - Mathf.Pow(1f - t, 3f);
            detailPanel.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }
        
        detailPanel.transform.localScale = targetScale;
        onComplete?.Invoke();
    }
    
    private System.Collections.IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            yield return null;
        }
        
        canvasGroup.alpha = targetAlpha;
    }
    
    private void ShowWarning(string message)
    {
        // Simple warning using instruction text
        if (instructionText != null)
        {
            string originalText = instructionText.text;
            instructionText.text = message;
            instructionText.color = Color.red;
            
            // Reset after 2 seconds
            Invoke("ResetInstructionText", 2f);
        }
    }
    
    private void ResetInstructionText()
    {
        if (instructionText != null)
        {
            instructionText.color = Color.white;
            UpdateInstructionText();
        }
    }
}
