namespace Core
{
    public class Vertex<TEdge>
        where TEdge : IGraphEdge
    {
        public int Index;
        public int Open;
        // path builder data
        public Vertex<TEdge> Back;
        public TEdge Edge;
        // vertex manager data
        public Vertex<TEdge> Next;
        public Vertex<TEdge> Prev;
        public int PathId;
        public int BucketId;
        // A* data
        public double F; // = G+H
        public double G; // cost of the best path to this vertex
        public double H; // heuristic
    }
}
