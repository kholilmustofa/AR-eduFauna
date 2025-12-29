using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimalCard : MonoBehaviour
{
    [Header("UI Components")]
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public Button cardButton;
    
    [Header("Visual Effects")]
    public Image highlightImage;
    public float hoverScale = 1.05f;
    
    private AnimalData animalData;
    private int animalIndex;
    private Vector3 originalScale;
    private bool isHovered = false;
    
    private void Awake()
    {
        originalScale = transform.localScale;
        
        // Auto-find components if not assigned
        if (iconImage == null)
            iconImage = transform.Find("Icon")?.GetComponent<Image>();
        
        if (nameText == null)
            nameText = transform.Find("Name")?.GetComponent<TextMeshProUGUI>();
        
        if (cardButton == null)
            cardButton = GetComponent<Button>();
        
        if (highlightImage == null)
            highlightImage = transform.Find("Highlight")?.GetComponent<Image>();
    }
    
    public void Setup(AnimalData animal, int index)
    {
        animalData = animal;
        animalIndex = index;
        
        // Set icon
        if (iconImage != null && animal.icon != null)
        {
            iconImage.sprite = animal.icon;
        }
        
        // Set name
        if (nameText != null)
        {
            nameText.text = animal.animalName;
        }
        
        // Setup button
        if (cardButton != null)
        {
            cardButton.onClick.AddListener(OnCardClicked);
        }
        
        // Hide highlight initially
        if (highlightImage != null)
        {
            highlightImage.enabled = false;
        }
    }
    
    private void OnCardClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SelectAnimal(animalIndex);
            
            // Load AR scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("ARScene");
        }
    }
    
    public void OnPointerEnter()
    {
        isHovered = true;
        if (highlightImage != null)
        {
            highlightImage.enabled = true;
        }
        
        // Scale up animation (simple version without LeanTween)
        StopAllCoroutines();
        StartCoroutine(ScaleAnimation(originalScale * hoverScale, 0.2f));
    }
    
    public void OnPointerExit()
    {
        isHovered = false;
        if (highlightImage != null)
        {
            highlightImage.enabled = false;
        }
        
        // Scale down animation (simple version without LeanTween)
        StopAllCoroutines();
        StartCoroutine(ScaleAnimation(originalScale, 0.2f));
    }
    
    private System.Collections.IEnumerator ScaleAnimation(Vector3 targetScale, float duration)
    {
        Vector3 startScale = transform.localScale;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            // Ease out back effect
            t = 1f - Mathf.Pow(1f - t, 3f);
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }
        
        transform.localScale = targetScale;
    }
}
