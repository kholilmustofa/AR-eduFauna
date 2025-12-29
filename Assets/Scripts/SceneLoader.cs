using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    
    [Header("Loading Settings")]
    public GameObject loadingPanel;
    public UnityEngine.UI.Slider loadingBar;
    public TMPro.TextMeshProUGUI loadingText;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }
    
    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }
    
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(true);
        }
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            if (loadingBar != null)
            {
                loadingBar.value = progress;
            }
            
            if (loadingText != null)
            {
                loadingText.text = $"Loading... {(progress * 100):0}%";
            }
            
            if (operation.progress >= 0.9f)
            {
                if (loadingText != null)
                {
                    loadingText.text = "Press any key to continue...";
                }
                
                // Auto-activate after a short delay
                yield return new WaitForSeconds(0.5f);
                operation.allowSceneActivation = true;
            }
            
            yield return null;
        }
        
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
        }
    }
    
    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(true);
        }
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;
        
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            if (loadingBar != null)
            {
                loadingBar.value = progress;
            }
            
            if (loadingText != null)
            {
                loadingText.text = $"Loading... {(progress * 100):0}%";
            }
            
            if (operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.5f);
                operation.allowSceneActivation = true;
            }
            
            yield return null;
        }
        
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
        }
    }
    
    public void QuitApplication()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
