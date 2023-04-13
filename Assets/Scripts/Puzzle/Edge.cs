using ScarabPuzzle.Vertices;
using UnityEngine;

namespace ScarabPuzzle.Edges
{
    public class Edge
    {
        public int ID { get; private set; }
        public Vertex VertexA { get; private set; }
        public Vertex VertexB { get; private set; }

        public Vector3 VertexAPosition { get; private set; }
        public Vector3 VertexBPosition { get; private set; }

        public Edge(int id, Vertex vertexA, Vertex vertexB, Vector3 aPosition, Vector3 bPosition)
        {
            ID = id;
            VertexA = vertexA;
            VertexB = vertexB;
            VertexAPosition = aPosition;
            VertexBPosition = bPosition;
        }

        public bool Connects(Vertex vertex1, Vertex vertex2)
        {
            return (VertexA == vertex1 && VertexB == vertex2) || (VertexA == vertex2 && VertexB == vertex1);
        }
    }
}