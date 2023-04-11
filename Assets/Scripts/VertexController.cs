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

    private void OnMouseDown()
    {
        PuzzleController puzzleController = FindObjectOfType<PuzzleController>();
        if (puzzleController != null)
        {
            puzzleController.SelectVertex(this);
        }
    }

    public void Initialize(int id)
    {
        Vertex = new Vertex(id);
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

    // Add any other necessary methods or functionality for VertexController
}