using System.Collections.Generic;
using UnityEngine;

public class VertexController : MonoBehaviour
{
    [SerializeField]
    private Material _selectedMaterial;
    [SerializeField]
    private Material _visitedMaterial;
    [SerializeField]
    private List<VertexController> _neighbourVertices;
    [SerializeField]
    private float _moveDistance = 1f;
    [SerializeField]
    private float _speed = 1f;

    private Vector3 _initialPosition;
    private float _timePassed;
    private MeshRenderer _meshRenderer;
    private Material _startMaterial;

    public bool IsAvailable { get; set; }
    public Vertex Vertex { get; private set; }
    public List<VertexController> NeighbourVertices { get => _neighbourVertices; set => _neighbourVertices = value; }

    private void Awake()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _startMaterial = _meshRenderer.material;
        _initialPosition = transform.position;
    }

    private void Update()
    {
        if (IsAvailable)
        {
            MoveUpAndDown();
        }
        else if (transform.position != _initialPosition)
        {
            var step = _speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _initialPosition, step);
        }
    }

    private void OnDrawGizmos()
    {
        if (_neighbourVertices == null || _neighbourVertices.Count < 2)
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

        foreach (var neighbour in NeighbourVertices)
        {
            if (!Vertex.IsConnectedTo(neighbour.Vertex))
            {
                neighbour.IsAvailable = true;
            }
        }
    }

    public void Deselect()
    {
        _meshRenderer.material = _visitedMaterial;

        foreach (var neighbour in NeighbourVertices)
        {
            if (neighbour.IsAvailable)
            {
                neighbour.IsAvailable = false;
            }
        }
    }

    public void Reset()
    {
        _meshRenderer.material = _startMaterial;
    }

    private void MoveUpAndDown()
    {
        _timePassed += Time.deltaTime;
        float newYPosition = _initialPosition.y + Mathf.Sin(_timePassed * _speed) * _moveDistance;
        transform.position = new Vector3(_initialPosition.x, newYPosition, _initialPosition.z);
    }
}