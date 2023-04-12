using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleController : MonoBehaviour
{
    [SerializeField] 
    private List<VertexController> _vertexControllers;
    [SerializeField]
    private LineRenderer _lineRenderer;

    private VertexController _selectedVertexController;
    private int _currentIndex = 0;
    private Vector3 _lastAddedPoint;

    public Graph Graph { get; private set; }
    public Path CurrentPath { get; private set; }

    private void Awake()
    {
        Graph = new Graph();
        CurrentPath = new Path();

        for (int i = 0; i < _vertexControllers.Count; i++)
        {
            VertexController vertexController = _vertexControllers[i];
            vertexController.Initialize();
            Graph.AddVertex(vertexController.Vertex);
        }
    }

    private void Start()
    {
        Graph.GenerateEdges(_vertexControllers);
    }

    public void SelectVertex(VertexController vertexController)
    {
        if (_selectedVertexController == null)
        {
            _selectedVertexController = vertexController;
            _selectedVertexController.Select();
            AddLinePoint(0, _selectedVertexController.transform.position);
            return;
        }

        Edge edge = Graph.GetEdge(_selectedVertexController.Vertex, vertexController.Vertex);

        if (edge != null && !CurrentPath.Contains(edge))
        {
            CurrentPath.AddEdge(edge);
            _selectedVertexController.Deselect();
            vertexController.Select();
            _selectedVertexController = vertexController;

            if (_lastAddedPoint == edge.VertexAPosition)
            {
                AddLinePoint(_currentIndex, edge.VertexBPosition);
            }
            else
            {
                AddLinePoint(_currentIndex, edge.VertexAPosition);
            }
           
            // Check if the puzzle is solved
            if (IsPuzzleSolved())
            {
                // Display a message and trigger a visual effect
                Debug.Log("Puzzle solved!");
            }
        }
    }

    public void DeselectVertex()
    {
        if (_selectedVertexController != null)
        {
            _selectedVertexController.Deselect();
            _selectedVertexController = null;
        }
    }

    public void Reset()
    {
        CurrentPath.Reset();
        DeselectVertex();
        _lineRenderer.positionCount = 0;
        _currentIndex = 0;

        foreach (var vertex in _vertexControllers)
        {
            vertex.Reset();
        }
    }

    private void AddLinePoint(int id,Vector3 position)
    {
        _lineRenderer.positionCount = id + 1;
        _lineRenderer.SetPosition(id, position);
        _currentIndex++;
        _lastAddedPoint = position;
    }

    private bool IsPuzzleSolved()
    {
        Debug.Log(CurrentPath.Edges.Count + " vs " + (Graph.Edges.Count - 1));
        return CurrentPath.Edges.Count == Graph.Edges.Count - 1;
    }
}