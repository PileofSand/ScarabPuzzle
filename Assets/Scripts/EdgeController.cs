using UnityEngine;

public class EdgeController : MonoBehaviour
{
    public Edge Edge { get;  set; }

    [SerializeField]
    private LineRenderer lineRenderer;

    private void Awake()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
    }

    public void UpdateVisuals()
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, Edge.VertexAPosition);
            lineRenderer.SetPosition(1, Edge.VertexBPosition);
        }
    }

    public void Reset()
    {
        
    }
}