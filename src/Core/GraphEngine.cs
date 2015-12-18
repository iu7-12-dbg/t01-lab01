using System;
using System.Diagnostics;

namespace Core
{
    using IndexType = Int32;
    using DistanceType = Double;
    
    public class GraphEngine<TEdge>
        where TEdge : IGraphEdge
    {
        private VertexManager<TEdge> vertexManager;
        private AStar<TEdge> algorithm;
        private Stopwatch pathTimer;

        public GraphEngine(int maxVertexCount)
        {
            vertexManager = new VertexManager<TEdge>(maxVertexCount, 8*1024);
            vertexManager.SetMinBucketValue(0);
            vertexManager.SetMaxBucketValue(2000);
            algorithm = new AStar<TEdge>(vertexManager);
            pathTimer = new Stopwatch();
        }

        public bool Search(IGraph<TEdge> graph, IndexType srcId, IndexType dstId,
            Vector<IndexType> path, IPathManagerParams pmParams)
        {
            pathTimer.Restart();
            var pathManager = new PathManager<TEdge>();
            pathManager.Setup(graph, algorithm.Manager, path, srcId, dstId, pmParams);
            bool found = algorithm.Find(pathManager);
            pathTimer.Stop();
            Debug.WriteLine("GraphEngine.Search: {0:2}ms", pathTimer.Elapsed.TotalMilliseconds);
            return found;
        }
    }
}
