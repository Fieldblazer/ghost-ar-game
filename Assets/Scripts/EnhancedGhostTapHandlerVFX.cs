using UnityEngine;

public class EnhancedGhostTapHandlerVFX : MonoBehaviour
{
    [Header("VFX Integration")]
    public GhostVFX ghostVFX;
    public AudioSource audioSource;
    public AudioClip tapSound;
    
    [Header("Interaction Settings")]
    public bool enableHapticFeedback = true;
    public LayerMask interactionLayers = -1;
    
    private bool hasBeenTapped = false;
    private SphereCollider sphereCollider;
    
    void Start()
    {
        // Get VFX component
        if (ghostVFX == null)
            ghostVFX = GetComponent<GhostVFX>();
        
        // Setup audio
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        
        // Ensure we have a collider for interaction
        sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider == null)
        {
            sphereCollider = gameObject.AddComponent<SphereCollider>();
            sphereCollider.radius = 0.5f;
            sphereCollider.isTrigger = true;
        }
        
        Debug.Log("Enhanced ghost tap handler with VFX initialized");
    }
    
    void OnMouseDown()
    {
        if (!hasBeenTapped)
        {
            HandleGhostTap();
        }
    }
    
    void Update()
    {
        // Handle touch input for mobile
        if (Input.touchCount > 0 && !hasBeenTapped)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                CheckTouchHit(touch.position);
            }
        }
    }
    
    void CheckTouchHit(Vector2 screenPosition)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;
        
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactionLayers))
        {
            if (hit.collider == sphereCollider)
            {
                HandleGhostTap();
            }
        }
    }
    
    void HandleGhostTap()
    {
        if (hasBeenTapped) return;
        
        hasBeenTapped = true;
        
        Debug.Log("Ghost tapped with VFX: " + gameObject.name);
        
        // Play audio
        if (audioSource != null && tapSound != null)
        {
            audioSource.PlayOneShot(tapSound);
        }
        
        // Trigger haptic feedback on mobile
        if (enableHapticFeedback)
        {
            TriggerHapticFeedback();
        }
        
        // Start VFX sequence
        if (ghostVFX != null)
        {
            ghostVFX.PlayFoundEffect();
        }
        else
        {
            // Fallback if no VFX component
            Debug.LogWarning("No GhostVFX component found, destroying immediately");
            NotifyGhostManager();
            Destroy(gameObject);
        }
        
        // Disable collider to prevent multiple taps
        if (sphereCollider != null)
            sphereCollider.enabled = false;
    }
    
    void TriggerHapticFeedback()
    {
        // Simple haptic feedback for mobile devices
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Handheld.Vibrate();
        }
    }
    
    void NotifyGhostManager()
    {
        // Find and notify ghost manager about the found ghost
        GhostManager ghostManager = FindObjectOfType<GhostManager>();
        if (ghostManager != null)
        {
            // Try to call increment method
            ghostManager.SendMessage("IncrementGhostCount", SendMessageOptions.DontRequireReceiver);
            ghostManager.SendMessage("OnGhostFound", gameObject, SendMessageOptions.DontRequireReceiver);
        }
        
        // Also try to notify smart spawning manager
        SmartSpawningManager smartManager = FindObjectOfType<SmartSpawningManager>();
        if (smartManager != null)
        {
            smartManager.SendMessage("OnGhostFound", gameObject, SendMessageOptions.DontRequireReceiver);
        }
    }
    
    // Called by VFX system when animation is complete
    void OnVFXComplete()
    {
        NotifyGhostManager();
        Debug.Log("VFX sequence completed for: " + gameObject.name);
    }
    
    // Public methods for configuration
    public void SetTapSound(AudioClip sound)
    {
        tapSound = sound;
    }
    
    public void SetHapticFeedback(bool enabled)
    {
        enableHapticFeedback = enabled;
    }
}