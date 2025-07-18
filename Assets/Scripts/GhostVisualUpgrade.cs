using UnityEngine;

public class GhostVisualUpgrade : MonoBehaviour
{
    [Header("Ghost Appearance")]
    public Color ghostColor = new Color(1f, 1f, 1f, 0.7f);
    public bool enableGlow = true;
    public float glowIntensity = 1.5f;
    
    [Header("Animation")]
    public bool enableFloating = true;
    public float floatSpeed = 1f;
    public float floatHeight = 0.2f;
    
    private MeshRenderer ghostRenderer;
    private Material ghostMaterial;
    private Vector3 originalPosition;
    
    void Start()
    {
        SetupGhostAppearance();
        originalPosition = transform.localPosition;
    }
    
    void Update()
    {
        if (enableFloating)
        {
            AnimateFloating();
        }
    }
    
    void SetupGhostAppearance()
    {
        ghostRenderer = GetComponent<MeshRenderer>();
        
        if (ghostRenderer != null)
        {
            CreateGhostMaterial();
            ghostRenderer.material = ghostMaterial;
            Debug.Log("Ghost visual upgraded: " + gameObject.name);
        }
    }
    
    void CreateGhostMaterial()
    {
        ghostMaterial = new Material(Shader.Find("Standard"));
        
        // Make translucent
        ghostMaterial.SetFloat("_Mode", 3);
        ghostMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        ghostMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        ghostMaterial.SetInt("_ZWrite", 0);
        ghostMaterial.EnableKeyword("_ALPHABLEND_ON");
        ghostMaterial.renderQueue = 3000;
        
        // Set color
        ghostMaterial.color = ghostColor;
        
        // Add glow
        if (enableGlow)
        {
            ghostMaterial.EnableKeyword("_EMISSION");
            ghostMaterial.SetColor("_EmissionColor", ghostColor * glowIntensity);
        }
    }
    
    void AnimateFloating()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.localPosition = originalPosition + Vector3.up * yOffset;
    }
    
    public void SetGhostColor(Color newColor)
    {
        ghostColor = newColor;
        if (ghostMaterial != null)
        {
            ghostMaterial.color = ghostColor;
            if (enableGlow)
            {
                ghostMaterial.SetColor("_EmissionColor", ghostColor * glowIntensity);
            }
        }
    }
}