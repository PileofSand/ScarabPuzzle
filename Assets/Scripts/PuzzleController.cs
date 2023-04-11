using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleController : MonoBehaviour
{
    [SerializeField] private List<VertexController> vertexControllers;
    [SerializeField] private List<EdgeController> edgeControllers;
    [SerializeField] private Transform edgeParent;

    public Graph Graph { get; private set; }
    public Path CurrentPath { get; private set; }
    private VertexController selectedVertexController;
    public GameObject edgePrefab; // Assign an EdgeController prefab in the Unity editor.

    private void CreateEdgeControllers()
    {
        for (int i = 0; i < Graph.Edges.Count; i++)
        {
            Edge edge = Graph.Edges[i];
            GameObject edgeInstance = Instantiate(edgePrefab);
            EdgeController edgeController = edgeInstance.GetComponent<EdgeController>();
            edgeController.Edge = edge;
            edgeController.transform.parent = edgeParent;
            edgeControllers.Add(edgeController);
            // Set edgeInstance position and rotation based on connected vertices.
            // ...
        }
    }

    private void Awake()
    {
        Graph = new Graph();
        for (int i = 0; i < vertexControllers.Count; i++)
        {
            VertexController vertexController = vertexControllers[i];
            vertexController.Initialize(i);
            Graph.AddVertex(vertexController.Vertex);
        }

        CurrentPath = new Path();
    }

    private void Start()
    {
        // ...
        Graph.GenerateEdges(vertexControllers);
        CreateEdgeControllers();
    }

    public void SelectVertex(VertexController vertexController)
    {
        if (selectedVertexController == null)
        {
            selectedVertexController = vertexController;
            selectedVertexController.Select();
            return;
        }

        Edge edge = Graph.GetEdge(selectedVertexController.Vertex, vertexController.Vertex);

        if (edge != null && !CurrentPath.Contains(edge))
        {
            CurrentPath.AddEdge(edge);
            selectedVertexController.Deselect();
            vertexController.Select();
            selectedVertexController = vertexController;

            // Update visuals for edges
           var edgeController = edgeControllers.Find(x => x.Edge == edge);
            edgeController.UpdateVisuals();
            // Check if the puzzle is solved
            if (IsPuzzleSolved())
            {
                // Display a message and trigger a visual effect
                Debug.Log("Puzzle solved!");
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadScene();
        }
    }

    public void ReloadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void DeselectVertex()
    {
        if (selectedVertexController != null)
        {
            selectedVertexController.Deselect();
            selectedVertexController = null;
        }
    }

    public void Reset()
    {
        CurrentPath.Reset();
        DeselectVertex();

        // Update visuals for edges
        foreach (var edgeController in edgeControllers)
        {
            edgeController.Reset();
        }

        foreach (var edgeController in edgeControllers)
        {
            edgeController.Reset();
        }
    }

    private bool IsPuzzleSolved()
    {
        return CurrentPath.Edges.Count == Graph.Edges.Count - 1;
    }
}