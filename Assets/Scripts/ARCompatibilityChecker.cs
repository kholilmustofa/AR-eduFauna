using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SceneManagement;
using TMPro;

public class ARCompatibilityChecker : MonoBehaviour
{
    [Header("UI References")]
    public GameObject compatibilityWarningPanel;
    public TextMeshProUGUI warningMessageText;
    public UnityEngine.UI.Button continueButton;
    public UnityEngine.UI.Button exitButton;
    
    [Header("Settings")]
    public bool checkOnStart = true;
    public string fallbackScene = "SplashScreen";
    
    private void Start()
    {
        if (checkOnStart)
        {
            CheckARCompatibility();
        }
    }
    
    public void CheckARCompatibility()
    {
        #if UNITY_ANDROID
        CheckAndroidARSupport();
        #elif UNITY_IOS
        CheckiOSARSupport();
        #else
        ShowWarning("AR is only supported on Android and iOS devices.");
        #endif
    }
    
    private void CheckAndroidARSupport()
    {
        // Check if ARCore is supported
        bool isSupported = ARSession.state != ARSessionState.Unsupported;
        
        if (!isSupported)
        {
            ShowWarning(
                "AR tidak didukung di perangkat ini.\n\n" +
                "Pastikan:\n" +
                "1. Perangkat Anda mendukung ARCore\n" +
                "2. ARCore sudah terinstal\n" +
                "3. Izin kamera telah diberikan\n\n" +
                "Kunjungi: https://developers.google.com/ar/devices"
            );
        }
        else
        {
            Debug.Log("ARCore is supported on this device!");
            HideWarning();
        }
    }
    
    private void CheckiOSARSupport()
    {
        // Check if ARKit is supported
        bool isSupported = ARSession.state != ARSessionState.Unsupported;
        
        if (!isSupported)
        {
            ShowWarning(
                "AR is not supported on this device.\n\n" +
                "Make sure:\n" +
                "1. Your device supports ARKit\n" +
                "2. iOS version is 11.0 or newer\n" +
                "3. Camera permission is granted"
            );
        }
        else
        {
            Debug.Log("ARKit is supported on this device!");
            HideWarning();
        }
    }
    
    private void ShowWarning(string message)
    {
        if (compatibilityWarningPanel != null)
        {
            compatibilityWarningPanel.SetActive(true);
        }
        
        if (warningMessageText != null)
        {
            warningMessageText.text = message;
        }
        
        if (continueButton != null)
        {
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(OnContinueClicked);
        }
        
        if (exitButton != null)
        {
            exitButton.onClick.RemoveAllListeners();
            exitButton.onClick.AddListener(OnExitClicked);
        }
        
        Debug.LogWarning("AR Compatibility Issue: " + message);
    }
    
    private void HideWarning()
    {
        if (compatibilityWarningPanel != null)
        {
            compatibilityWarningPanel.SetActive(false);
        }
    }
    
    private void OnContinueClicked()
    {
        // User chooses to continue anyway (for testing or fallback mode)
        HideWarning();
        Debug.Log("User chose to continue despite AR compatibility warning");
    }
    
    private void OnExitClicked()
    {
        // Return to main menu or exit
        if (!string.IsNullOrEmpty(fallbackScene))
        {
            SceneManager.LoadScene(fallbackScene);
        }
        else
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
    
    // Public method to check camera permission
    public void CheckCameraPermission()
    {
        #if UNITY_ANDROID
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.Camera))
        {
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.Camera);
        }
        #endif
    }
    
    // Get device info for debugging
    public string GetDeviceInfo()
    {
        string info = "Device Information:\n";
        info += $"Model: {SystemInfo.deviceModel}\n";
        info += $"OS: {SystemInfo.operatingSystem}\n";
        info += $"Processor: {SystemInfo.processorType}\n";
        info += $"Memory: {SystemInfo.systemMemorySize} MB\n";
        info += $"Graphics: {SystemInfo.graphicsDeviceName}\n";
        info += $"AR Session State: {ARSession.state}\n";
        
        return info;
    }
    
    // Log device info to console
    public void LogDeviceInfo()
    {
        Debug.Log(GetDeviceInfo());
    }
}
