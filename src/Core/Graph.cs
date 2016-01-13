using System;
using System.Collections.Generic;
using System.Diagnostics;
using Core.Math;

namespace Core
{
    public interface IGraph<TEdge>
        where TEdge : IGraphEdge
    {
        bool IsAccessible(int vertexId);
        void SetAccessible(int vertexId, bool value);
        bool ValidateVertexId(int vertexId);
        IGraphEdgeIterator<TEdge> GetIterator(int vertexId);
        int GetNeighborId(int vertexId, IGraphEdgeIterator<TEdge> i);
        // XXX: could be used with precalculated edge weights
        //double GetEdgeWeight(IGraphEdgeIterator<TEdge> i);
        GraphVertex GetVertex(int vertexId);
        void SetInvalidVertexId(out int vertexId);
        int GetVertexId(GraphVertex vertex);
    }
    
    public interface IGraphEdge
    {
        int VertexId { get; set; }
        // see IGraph.GetEdgeWeight
        //double PathDistance { get; set; }
    }

    public class GraphVertex
    {
        public int Id; // index of this vertex in array
        public Vector<GraphEdge> Edges;
        public Vector2 Location;
        // user data
        public ulong SceneObjectId;
    }

    public struct GraphEdge : IGraphEdge
    {
        public int VertexId { get; set; }
        // see IGraph.GetEdgeWeight
        //public double PathDistance { get; set; }
    }

    public interface IGraphEdgeIterator<TEdge>
        where TEdge : IGraphEdge
    {
        TEdge Current { get; }
        bool End { get; }
        void Next();
    }

    public class Graph : IGraph<GraphEdge>
    {
        public struct EdgeIterator : IGraphEdgeIterator<GraphEdge>
        {
            private GraphVertex vertex;
            private int index;

            public EdgeIterator(GraphVertex vertex)
            {
                this.vertex = vertex;
                index = 0;
            }

            public GraphEdge Current { get { return vertex.Edges[index]; } }
            
            public bool End
            { get { return index>=vertex.Edges.Count; } }

            public void Next()
            { index++; }
        }
        
        public int VertexCount { get; private set; }
        public int EdgeCount { get; private set; }
        private Vector<GraphVertex> vertices;
        private Vector<int> freeIndices;
	    private Vector<bool> vertexStates; // for alternative paths
        private Vector<GraphEdge> emptyEdgeContainer = new Vector<GraphEdge>();
        private const int InitialEdgeListSize = 4;

        public Graph(int reservedVertexCount)
        {
            vertices = new Vector<GraphVertex>(reservedVertexCount);
            vertexStates = new Vector<bool>(reservedVertexCount);
            freeIndices = new Vector<int>(reservedVertexCount);
        }

        private GraphVertex AllocateVertex()
        {
            GraphVertex vertex;
            if (freeIndices.Count>0)
            {
                var id = freeIndices.Pop();
                vertex = vertices[id];
                vertex.Id = id;
            }
            else
            {
                vertex = new GraphVertex();
                vertex.Id = vertices.Count;
                vertex.Edges = emptyEdgeContainer;
                vertices.Push(vertex);
            }
            return vertex;
        }

        private void DeleteVertex(GraphVertex vertex)
        {
            Debug.Assert(ValidateVertexId(vertex.Id));
            if (vertex.Id==vertices.Count-1)
                vertices.Count--;
            else
            {
                freeIndices.Push(vertex.Id);
                SetInvalidVertexId(out vertex.Id);
                vertex.Edges.Count = 0;
            }
        }

        public int AddVertex(Vector2 location, ulong sceneObjectId)
        {
            var vertex = AllocateVertex();
            VertexCount++;
            vertex.SceneObjectId = sceneObjectId;
            vertex.Location = location;
            vertices[vertex.Id] = vertex;
            return vertex.Id;
        }

        public void RemoveVertex(int vertexId)
        {
            Debug.Assert(ValidateVertexId(vertexId));
            DeleteVertex(vertices[vertexId]);
            VertexCount--;
        }
        
        public bool AddEdge(int id1, int id2)
        {
            Debug.Assert(ValidateVertexId(id1));
            Debug.Assert(ValidateVertexId(id2));
            var v1 = vertices[id1];
            var v2 = vertices[id2];
            if (v1.Edges==emptyEdgeContainer)
                v1.Edges = new Vector<GraphEdge>(InitialEdgeListSize);
            if (v2.Edges==emptyEdgeContainer)
                v2.Edges = new Vector<GraphEdge>(InitialEdgeListSize);
            var distance = v1.Location.Distance(v2.Location);
            v1.Edges.Push(new GraphEdge {VertexId = id2});//, PathDistance = distance});
            v2.Edges.Push(new GraphEdge {VertexId = id1});//, PathDistance = distance});
            EdgeCount++;
            return true;
        }

        private bool RemoveEdge(GraphVertex v1, int id2)
        {
            for (int i = 0; i<v1.Edges.Count; i++)
            {
                if (v1.Edges[i].VertexId==id2)
                {
                    if (i!=v1.Edges.Count-1)
                        v1.Edges[i] = v1.Edges[v1.Edges.Count-1];
                    v1.Edges.Count--;
                    return true;
                }
            }
            return false;
        }

        public bool RemoveEdge(int id1, int id2)
        {
            Debug.Assert(ValidateVertexId(id1));
            Debug.Assert(ValidateVertexId(id2));
            bool removed = RemoveEdge(vertices[id1], id2);
            removed |= RemoveEdge(vertices[id2], id1);
            if (removed)
                EdgeCount--;
            return removed;
        }
        
        public bool IsAccessible(int vertexId)
        {
            Debug.Assert(ValidateVertexId(vertexId));
            return vertexStates[vertexId];
        }
        public void SetAccessible(int vertexId, bool value)
        {
            Debug.Assert(ValidateVertexId(vertexId));
            vertexStates[vertexId] = value;
        }
        public bool ValidateVertexId(int vertexId)
        { return vertexId<VertexCount && vertices[vertexId].Id==vertexId; }
        public IGraphEdgeIterator<GraphEdge> GetIterator(int vertexId)
        { return new EdgeIterator(GetVertex(vertexId)); }
        public int GetNeighborId(int vertexId, IGraphEdgeIterator<GraphEdge> i)
        { return i.Current.VertexId; }
        // see IGraph.GetEdgeWeight
        //public double GetEdgeWeight(IGraphEdgeIterator<CEdge> i)
        //{ return i.Current.PathDistance; }
        public GraphVertex GetVertex(int vertexId)
        {
            Debug.Assert(ValidateVertexId(vertexId));
            return vertices[vertexId];
        }
        public void SetInvalidVertexId(out int vertexId)
        {
            vertexId = Int32.MaxValue;
            Debug.Assert(!ValidateVertexId(vertexId));
        }
        public int GetVertexId(GraphVertex vertex)
        {
            Debug.Assert(ValidateVertexId(vertex.Id));
            return vertex.Id;
        }
    }
}
