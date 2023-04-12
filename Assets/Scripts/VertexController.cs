using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class VertexController : MonoBehaviour
{
    [SerializeField] 
    private Material _selectedMaterial;
    [SerializeField] 
    private Material _visitedMaterial;
    [SerializeField]
    private List<VertexController> _neighbourVertices;

    private MeshRenderer _meshRenderer;
    private Material _startMaterial;

    public Vertex Vertex { get; private set; }
    public List<VertexController> NeighbourVertices { get => _neighbourVertices; set => _neighbourVertices = value; }


    private void Awake()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _startMaterial = _meshRenderer.material;
    }

    private void OnDrawGizmos()
    {
        if (_neighbourVertices == null || _neighbourVertices.Count < 2 )
        {
            return;
        }

        Gizmos.color = Color.green;

        for (int i = 0; i < _neighbourVertices.Count; i++)
        {
            Gizmos.DrawLine(transform.position, _neighbourVertices[i].transform.position);
        }
    }

    public void Initialize()
    {
        Vertex = new Vertex();
    }

    public void Select()
    {
        _meshRenderer.material = _selectedMaterial;
    }

    public void Deselect()
    {
        _meshRenderer.material = _visitedMaterial;
    }

    public void Reset()
    {
        _meshRenderer.material = _startMaterial;
    }
}