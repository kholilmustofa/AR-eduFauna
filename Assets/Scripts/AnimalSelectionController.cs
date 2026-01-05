using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class AnimalSelectionController : MonoBehaviour
{
    [Header("UI References")]
    public Transform animalGridContainer;
    public GameObject animalCardPrefab;
    public Button backButton;
    
    [Header("Animation Settings")]
    public float cardAnimationDelay = 0.1f;
    
    private List<GameObject> animalCards = new List<GameObject>();
    
    private void Start()
    {
        PopulateAnimalGrid();
        
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackClicked);
        }
    }
    
    private void PopulateAnimalGrid()
    {
        if (GameManager.Instance == null || animalGridContainer == null || animalCardPrefab == null)
        {
            Debug.LogError("Missing references in AnimalSelectionController");
            return;
        }
        
        // Stop all running animations before clearing
        StopAllCoroutines();
        
        // Clear existing cards
        foreach (Transform child in animalGridContainer)
        {
            Destroy(child.gameObject);
        }
        animalCards.Clear();
        
        // Create cards for each animal
        for (int i = 0; i < GameManager.Instance.animals.Count; i++)
        {
            AnimalData animal = GameManager.Instance.animals[i];
            GameObject card = Instantiate(animalCardPrefab, animalGridContainer);
            
            // Setup card
            AnimalCard cardScript = card.GetComponent<AnimalCard>();
            if (cardScript != null)
            {
                cardScript.Setup(animal, i);
            }
            else
            {
                // Fallback if AnimalCard script is not attached
                SetupCardManually(card, animal, i);
            }
            
            animalCards.Add(card);
            
            // Animate card entrance
            StartCoroutine(AnimateCardEntrance(card, i));
        }
    }
    
    private void SetupCardManually(GameObject card, AnimalData animal, int index)
    {
        // Find and setup card components
        Image iconImage = card.transform.Find("Icon")?.GetComponent<Image>();
        TextMeshProUGUI nameText = card.transform.Find("Name")?.GetComponent<TextMeshProUGUI>();
        Button button = card.GetComponent<Button>();
        
        if (iconImage != null && animal.icon != null)
        {
            iconImage.sprite = animal.icon;
        }
        
        if (nameText != null)
        {
            nameText.text = animal.animalName;
        }
        
        if (button != null)
        {
            int animalIndex = index;
            button.onClick.AddListener(() => OnAnimalSelected(animalIndex));
        }
    }
    
    private System.Collections.IEnumerator AnimateCardEntrance(GameObject card, int index)
    {
        // Wait for delay based on index
        yield return new WaitForSeconds(index * cardAnimationDelay);
        
        // Check if card still exists after wait
        if (card == null)
        {
            yield break;
        }
        
        // Scale animation
        CanvasGroup canvasGroup = card.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = card.AddComponent<CanvasGroup>();
        }
        
        canvasGroup.alpha = 0f;
        card.transform.localScale = Vector3.zero;
        
        float duration = 0.3f;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            // Check if card still exists during animation
            if (card == null)
            {
                yield break;
            }
            
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            // Ease out elastic
            float scale = Mathf.Lerp(0f, 1f, t);
            card.transform.localScale = Vector3.one * scale;
            canvasGroup.alpha = t;
            
            yield return null;
        }
        
        // Final check before setting final values
        if (card != null)
        {
            card.transform.localScale = Vector3.one;
            canvasGroup.alpha = 1f;
        }
    }
    
    private void OnAnimalSelected(int index)
    {
        GameManager.Instance.SelectAnimal(index);
        
        // Load AR scene
        SceneManager.LoadScene("ARScene");
    }
    
    private void OnBackClicked()
    {
        SceneManager.LoadScene("SplashScreen");
    }
}
