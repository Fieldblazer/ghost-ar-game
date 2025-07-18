using UnityEngine;
using System.Collections;

public class GhostVFX : MonoBehaviour
{
    [Header("Animation Settings")]
    public float tapScaleMultiplier = 1.3f;
    public float animationDuration = 0.3f;
    public float disappearDuration = 0.8f;
    
    [Header("Effects")]
    public Color foundColor = Color.yellow;
    public bool enableScaleEffect = true;
    public bool enableColorFlash = true;
    
    private Vector3 originalScale;
    private Renderer ghostRenderer;
    private Color originalColor;
    private bool isPlaying = false;
    
    void Start()
    {
        originalScale = transform.localScale;
        ghostRenderer = GetComponentInChildren<Renderer>();
        
        if (ghostRenderer != null)
        {
            originalColor = ghostRenderer.material.color;
        }
    }
    
    public void PlayFoundEffect()
    {
        if (isPlaying) return;
        
        isPlaying = true;
        Debug.Log("Playing found effect for: " + gameObject.name);
        
        StartCoroutine(FoundEffectSequence());
    }
    
    IEnumerator FoundEffectSequence()
    {
        // Step 1: Scale and color effect
        if (enableScaleEffect)
            StartCoroutine(ScaleEffect());
        
        if (enableColorFlash)
            StartCoroutine(ColorFlashEffect());
        
        yield return new WaitForSeconds(animationDuration);
        
        // Step 2: Disappear effect
        yield return StartCoroutine(DisappearEffect());
        
        // Step 3: Destroy ghost
        NotifyDestroyed();
    }
    
    IEnumerator ScaleEffect()
    {
        float elapsed = 0f;
        
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / animationDuration;
            
            float scaleValue = progress < 0.5f ? 
                Mathf.Lerp(1f, tapScaleMultiplier, progress * 2f) :
                Mathf.Lerp(tapScaleMultiplier, 1f, (progress - 0.5f) * 2f);
            
            transform.localScale = originalScale * scaleValue;
            yield return null;
        }
        
        transform.localScale = originalScale;
    }
    
    IEnumerator ColorFlashEffect()
    {
        if (ghostRenderer == null) yield break;
        
        // Flash to found color
        ghostRenderer.material.color = foundColor;
        
        yield return new WaitForSeconds(animationDuration * 0.5f);
        
        // Return to original color
        ghostRenderer.material.color = originalColor;
    }
    
    IEnumerator DisappearEffect()
    {
        float elapsed = 0f;
        
        while (elapsed < disappearDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / disappearDuration;
            
            // Scale down
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, progress);
            
            // Fade out
            if (ghostRenderer != null)
            {
                Color color = ghostRenderer.material.color;
                color.a = Mathf.Lerp(1f, 0f, progress);
                ghostRenderer.material.color = color;
            }
            
            yield return null;
        }
    }
    
    void NotifyDestroyed()
    {
        // Notify ghost manager
        GhostManager ghostManager = FindObjectOfType<GhostManager>();
        if (ghostManager != null)
        {
            ghostManager.SendMessage("OnGhostFound", gameObject, SendMessageOptions.DontRequireReceiver);
        }
        
        Destroy(gameObject, 0.1f);
    }
}