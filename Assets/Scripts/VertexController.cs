using System.Collections.Generic;
using UnityEngine;

public class VertexController : MonoBehaviour
{
    public Vertex Vertex { get; private set; }
    public List<VertexController> NeighbourVertices { get => neighbourVertices; set => neighbourVertices = value; }

    [SerializeField] private Material visitedMaterial;

    [SerializeField]
    private List<VertexController> neighbourVertices;
    [SerializeField] private Material selectedMaterial;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public void Initialize()
    {
        Vertex = new Vertex();
    }

    public void Select()
    {
        meshRenderer.material = selectedMaterial;
    }

    public void Deselect()
    {
        meshRenderer.material = visitedMaterial;
    }

    public void UpdateVisuals()
    {
        // Update visuals based on Edge state
    }

    public void Reset()
    {

    }
}