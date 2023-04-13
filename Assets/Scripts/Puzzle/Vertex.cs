using System.Collections.Generic;

namespace ScarabPuzzle.Vertices
{
    public class Vertex
    {
        public List<Vertex> ConnectedVertices { get; private set; }

        public Vertex()
        {
            ConnectedVertices = new List<Vertex>();
        }

        public void ConnectTo(Vertex vertex)
        {
            if (!ConnectedVertices.Contains(vertex))
            {
                ConnectedVertices.Add(vertex);
            }
        }

        public bool IsConnectedTo(Vertex vertex)
        {
            return ConnectedVertices.Contains(vertex);
        }

        public void Reset()
        {
            ConnectedVertices.Clear();
        }
    }
}