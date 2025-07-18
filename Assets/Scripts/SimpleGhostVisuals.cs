using UnityEngine;

public class SimpleGhostVisuals : MonoBehaviour
{
    [Header("Ghost Visual Settings")]
    public Color ghostColor = new Color(0.8f, 0.9f, 1f, 0.6f);
    public bool enableGlow = true;
    public bool enableFloating = true;
    
    private MeshRenderer ghostRenderer;
    private Material ghostMaterial;
    private Vector3 startPosition;
    
    void Start()
    {
        SetupGhostLook();
        startPosition = transform.localPosition;
    }
    
    void Update()
    {
        if (enableFloating)
        {
            float yOffset = Mathf.Sin(Time.time) * 0.2f;
            transform.localPosition = startPosition + Vector3.up * yOffset;
        }
    }
    
    void SetupGhostLook()
    {
        ghostRenderer = GetComponent<MeshRenderer>();
        if (ghostRenderer != null)
        {
            CreateGhostMaterial();
            ghostRenderer.material = ghostMaterial;
            Debug.Log("Ghost visual setup complete: " + gameObject.name);
        }
    }
    
    void CreateGhostMaterial()
    {
        ghostMaterial = new Material(Shader.Find("Standard"));
        
        // Make transparent
        ghostMaterial.SetFloat("_Mode", 3);
        ghostMaterial.SetInt("_SrcBlend", 5);
        ghostMaterial.SetInt("_DstBlend", 10);
        ghostMaterial.SetInt("_ZWrite", 0);
        ghostMaterial.EnableKeyword("_ALPHABLEND_ON");
        ghostMaterial.renderQueue = 3000;
        
        ghostMaterial.color = ghostColor;
        
        if (enableGlow)
        {
            ghostMaterial.EnableKeyword("_EMISSION");
            ghostMaterial.SetColor("_EmissionColor", ghostColor);
        }
    }
}