using System.Collections.Generic;

public class Graph
{
    public List<Vertex> Vertices { get; private set; }
    public List<Edge> Edges { get; private set; }

    public Graph()
    {
        Vertices = new List<Vertex>();
        Edges = new List<Edge>();
    }

    public void GenerateEdges(List<VertexController> vertexControllers)
    {
        int edgeID = 0;
        for (int i = 0; i < vertexControllers.Count; i++)
        {
            Vertex vertexA = vertexControllers[i].Vertex;
            VertexController vertexAController = vertexControllers[i];
            for (int j = 0; j < vertexControllers[i].NeighbourVertices.Count; j++)
            {
                Vertex vertexB = vertexAController.NeighbourVertices[j].Vertex;
                VertexController vertexBController = vertexAController.NeighbourVertices[j];
                if (!EdgeExists(vertexA, vertexB))
                {
                    Edge edge = new Edge(edgeID++, vertexA, vertexB, vertexAController.transform.position, vertexBController.transform.position);
                    AddEdge(edge);
                }
            }
        }
    }

    private bool EdgeExists(Vertex vertex1, Vertex vertex2)
    {
        return Edges.Exists(edge => edge.Connects(vertex1, vertex2));
    }

    public void AddVertex(Vertex vertex)
    {
        Vertices.Add(vertex);
    }

    public void AddEdge(Edge edge)
    {
        Edges.Add(edge);
    }

    public Edge GetEdge(Vertex vertex1, Vertex vertex2)
    {
        return Edges.Find(edge => edge.Connects(vertex1, vertex2));
    }
}