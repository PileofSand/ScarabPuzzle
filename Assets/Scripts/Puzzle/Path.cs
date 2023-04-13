using ScarabPuzzle.Edges;
using System.Collections.Generic;

namespace ScarabPuzzle
{
    public class Path
    {
        public List<Edge> Edges { get; private set; }

        public Path()
        {
            Edges = new List<Edge>();
        }

        public void AddEdge(Edge edge)
        {
            Edges.Add(edge);
        }

        public void RemoveLastEdge()
        {
            if (Edges.Count > 0)
            {
                Edges.RemoveAt(Edges.Count - 1);
            }
        }

        public bool Contains(Edge edge)
        {
            return Edges.Contains(edge);
        }

        public void Reset()
        {
            Edges.Clear();
        }
    }
}