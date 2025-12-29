using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SplashScreenController : MonoBehaviour
{
    [Header("UI References")]
    public Button selectAnimalButton;
    public Button aboutButton;
    
    [Header("Animation Settings")]
    public float fadeInDuration = 1.0f;
    
    private CanvasGroup canvasGroup;
    
    private void Start()
    {
        // Setup canvas group for fade effect
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0f; // Start hidden
        
        // Add button listeners
        if (selectAnimalButton != null)
        {
            selectAnimalButton.onClick.AddListener(OnSelectAnimalClicked);
        }
        
        if (aboutButton != null)
        {
            aboutButton.onClick.AddListener(OnAboutClicked);
        }
        
        // Animate entrance
        StartCoroutine(AnimateEntrance());
    }
    
    private System.Collections.IEnumerator AnimateEntrance()
    {
        // Fade in
        float elapsed = 0f;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeInDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }
    
    private void OnSelectAnimalClicked()
    {
        // Load animal selection scene
        SceneManager.LoadScene("AnimalSelection");
    }
    
    private void OnAboutClicked()
    {
        // Placeholder for About functionality (e.g., show a panel)
        Debug.Log("About clicked");
    }
}
