using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

public class OrientationManager : MonoBehaviour
{
    // Import functions from the jslib file
    [DllImport("__Internal")]
    private static extern void SetCurrentScene(string sceneName);
    
    [DllImport("__Internal")]
    private static extern void CheckOrientation();
    
    [DllImport("__Internal")]
    private static extern void ShowOrientationWarning(string requiredOrientation);
    
    [DllImport("__Internal")]
    private static extern void HideOrientationWarning();
    
    [DllImport("__Internal")]
    private static extern string GetRequiredOrientation(string gameName, string sceneName);
    
    [DllImport("__Internal")]
    private static extern string GetCurrentOrientation();
    
    // Singleton instance
    public static OrientationManager Instance { get; private set; }
    
    [Header("Configuration")]
    [Tooltip("Should orientation check be enforced for this game")]
    public bool enforceOrientation = true;
    
    [Tooltip("Current game name (must match the jslib configuration)")]
    public string currentGame = "Game1";
    
    [Tooltip("Should the game pause when orientation is incorrect")]
    public bool pauseOnIncorrectOrientation = true;
    
    // Keep track of the current scene
    private string currentScene;
    private bool gameWasPaused = false;
    
    private void Awake()
    {
        // Setup singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Register for scene change events
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    private void OnDestroy()
    {
        // Unregister from scene change events
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void Start()
    {
        // Set initial scene
        currentScene = SceneManager.GetActiveScene().name;
        SetSceneToJS(currentScene);
        
        Debug.Log("OrientationManager initialized with game: " + currentGame);
    }
    
    // Called when a scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Auto-detect scene changes
        currentScene = scene.name;
        SetSceneToJS(currentScene);
        
        Debug.Log("Scene changed to: " + currentScene);
    }
    
    // Set the scene to JavaScript
    private void SetSceneToJS(string sceneName)
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
        if (enforceOrientation)
        {
            Debug.Log("Setting current scene in JS: " + sceneName);
            SetCurrentScene(sceneName);
        }
        #else
        Debug.Log("OrientationManager: SetScene called in Editor or non-WebGL build.");
        #endif
    }
    
    // Force check the orientation (can be called from UI buttons)
    public void ForceCheckOrientation()
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
        if (enforceOrientation)
        {
            Debug.Log("Forcing orientation check from Unity");
            CheckOrientation();
        }
        #endif
    }
    
    // Callback when orientation doesn't match requirement (called from JavaScript)
    public void OnOrientationMismatch(string requiredOrientation)
    {
        Debug.Log($"Orientation mismatch! Required: {requiredOrientation}");
        
        if (pauseOnIncorrectOrientation && Time.timeScale > 0)
        {
            // Remember that we paused the game
            gameWasPaused = true;
            
            // Pause the game
            Time.timeScale = 0;
            
            // Optionally disable audio
            AudioListener.pause = true;
            
            Debug.Log("Game paused due to orientation mismatch");
        }
    }
    
    // Callback when orientation is correct (called from JavaScript)
    public void OnOrientationCorrect()
    {
        Debug.Log("Orientation is now correct!");
        
        if (gameWasPaused)
        {
            // Resume the game
            Time.timeScale = 1;
            
            // Re-enable audio
            AudioListener.pause = false;
            
            gameWasPaused = false;
            
            Debug.Log("Game resumed after orientation corrected");
        }
    }
    
    // Manually set the current scene (use this when SceneManager.LoadScene is called)
    public void SetScene(string sceneName)
    {
        currentScene = sceneName;
        SetSceneToJS(sceneName);
    }
}