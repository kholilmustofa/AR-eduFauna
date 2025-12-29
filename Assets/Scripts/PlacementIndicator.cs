using UnityEngine;

public class PlacementIndicator : MonoBehaviour
{
    [Header("Visual Settings")]
    public Material indicatorMaterial;
    public Color validColor = new Color(0, 1, 0, 0.5f);
    public Color invalidColor = new Color(1, 0, 0, 0.5f);
    
    [Header("Animation")]
    public bool animateScale = true;
    public float scaleSpeed = 1f;
    public float minScale = 0.9f;
    public float maxScale = 1.1f;
    
    public bool animateRotation = true;
    public float rotationSpeed = 50f;
    
    private Renderer indicatorRenderer;
    private Vector3 baseScale;
    
    private void Start()
    {
        indicatorRenderer = GetComponent<Renderer>();
        baseScale = transform.localScale;
        
        if (indicatorRenderer != null && indicatorMaterial != null)
        {
            indicatorRenderer.material = indicatorMaterial;
        }
        
        SetValid(true);
    }
    
    private void Update()
    {
        if (animateScale)
        {
            float scale = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(Time.time * scaleSpeed) + 1f) / 2f);
            transform.localScale = baseScale * scale;
        }
        
        if (animateRotation)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
    
    public void SetValid(bool isValid)
    {
        if (indicatorRenderer != null && indicatorRenderer.material != null)
        {
            indicatorRenderer.material.color = isValid ? validColor : invalidColor;
        }
    }
}
