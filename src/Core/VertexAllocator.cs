using System;
using System.Diagnostics;

namespace Core
{
    public class VertexAllocator<TEdge>
        where TEdge : IGraphEdge
    {
        protected Vector<Vertex<TEdge>> Vertices;

        public VertexAllocator(int reserveSize)
        { Vertices = new Vector<Vertex<TEdge>>(reserveSize); }
        public void Initialize()
        {}
        public int VisitedVertexCount { get { return Vertices.Count; } }
        public Vertex<TEdge> CreateVertex()
        {
            Debug.Assert(Vertices.Count<Vertices.Capacity-1);
            var v = new Vertex<TEdge>();
            Vertices.Push(v);
            return v;
        }
    }
}
