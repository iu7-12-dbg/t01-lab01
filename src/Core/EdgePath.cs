namespace Core
{
    public class EdgePath<TEdge> : VertexPath<TEdge>
        where TEdge : IGraphEdge
    {
        public override void SetParent(Vertex<TEdge> neighbor, Vertex<TEdge> parent)
        { base.SetParent(neighbor, parent); }
        public override void SetParent(Vertex<TEdge> neighbor, Vertex<TEdge> parent, TEdge edge)
        {
            base.SetParent(neighbor, parent);
            neighbor.Edge = edge;
        }
        public void GetEdgePath(Vector<TEdge> path, Vertex<TEdge> best, bool reverseOrder = false)
        {
            Vertex<TEdge> t1 = best, t2 = best.Back;
            int i;
            for (i = 1; t2!=null; t1 = t2, t2 = t2.Back, i++);
            int n = path.Count;
            i--;
            path.Count = n+i;
            t2 = best;
            if (!reverseOrder)
            {
                i = path.Count;
                for (; t2.Back!=null; t2 = t2.Back, i--)
                    path[i] = t2.Edge;
            }
            else
            {
                i = n;
                for (; t2.Back!=null; t2 = t2.Back, i++)
                    path[i] = t2.Edge;
            }
        }
    }
}
