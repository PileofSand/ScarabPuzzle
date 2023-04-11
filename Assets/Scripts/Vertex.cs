using System.Collections.Generic;

public class Vertex
{
    public int ID { get; private set; }
    public List<Vertex> ConnectedVertices { get; private set; }

    public Vertex(int id)
    {
        ID = id;
        ConnectedVertices = new List<Vertex>();
    }

    public void ConnectTo(Vertex vertex)
    {
        if (!ConnectedVertices.Contains(vertex))
        {
            ConnectedVertices.Add(vertex);
        }
    }

    public void DisconnectFrom(Vertex vertex)
    {
        ConnectedVertices.Remove(vertex);
    }
}