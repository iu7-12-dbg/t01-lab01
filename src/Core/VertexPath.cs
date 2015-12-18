using System;

namespace Core
{
    public class VertexPath<TEdge>
        where TEdge : IGraphEdge
    {
        public virtual void Initialize() {}
        public virtual void SetParent(Vertex<TEdge> neighbor, Vertex<TEdge> parent)
        { neighbor.Back = parent; }
        public virtual void SetParent(Vertex<TEdge> neighbor, Vertex<TEdge> parent, TEdge edge)
        { SetParent(neighbor, parent); }
        public virtual void GetVertexPath(Vector<int> path, Vertex<TEdge> best)
        {
            Vertex<TEdge> t1 = best, t2 = best.Back;
            int i;
            for (i = 1; t2!=null; t1 = t2, t2 = t2.Back, i++);
            path.Count = i;
            t1 = best;
            path[--i] = best.Index;
            t2 = t1.Back;
            i = path.Count;
            for (i--; t2!=null; t2 = t2.Back, i--)
                path[i] = t2.Index;
        }
    }
}
