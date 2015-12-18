using System;
using System.Diagnostics;

namespace Core
{
    using IndexType = Int32;
    using DistanceType = Double;
    
    public interface IPathManagerParams
    {
        DistanceType MaxRange { get; }
        int MaxIterationCount { get; }
        int MaxVisitedVertexCount { get; }
    }

    public interface IPathManager<TEdge>
        where TEdge : IGraphEdge
    {
        void Initialize();
        void Setup(IGraph<TEdge> graph, VertexManager<TEdge> vertexManager, Vector<IndexType> path,
            IndexType startIndex, IndexType targetIndex, IPathManagerParams pmParams);
        // compute G value
        DistanceType Evaluate(IndexType index1, IndexType index2, IGraphEdgeIterator<TEdge> i);
        // returns H value
        DistanceType Estimate(IndexType index);
        void InitializePath();
        void CreatePath(Vertex<TEdge> vertex);
        IndexType StartIndex { get; }
        IndexType TargetIndex { get; }
        bool IsTargetReached(IndexType index);
        bool IsLimitReached(int iterationCount);
        bool IsAccessible(IndexType index);
        IGraphEdgeIterator<TEdge> GetEdges(IndexType index);
        IndexType GetValue(IGraphEdgeIterator<TEdge> i);
        void End();
    }

    public abstract class PathManagerBase<TEdge> : IPathManager<TEdge>
        where TEdge : IGraphEdge
    {
        protected IGraph<TEdge> Graph;
        protected VertexManager<TEdge> Manager;
        protected Vector<IndexType> Path;
        //
        public virtual IndexType StartIndex { get; protected set; }
        public virtual IndexType TargetIndex { get; protected set; }
        protected DistanceType MaxRange;
        protected int MaxIterationCount;
        protected int MaxVisitedVertexCount;
        protected IndexType BestIndex;

        public virtual void Initialize()
        {}
        public virtual void Setup(IGraph<TEdge> graph, VertexManager<TEdge> vertexManager, Vector<IndexType> path,
            IndexType startId, IndexType targetId, IPathManagerParams pmParams)
        {
            Graph = graph;
            Manager = vertexManager;
            Path = path;
            StartIndex = startId;
            TargetIndex = targetId;
            MaxRange = pmParams.MaxRange;
            MaxIterationCount = pmParams.MaxIterationCount;
            MaxVisitedVertexCount = pmParams.MaxVisitedVertexCount;
        }
        public virtual DistanceType Evaluate(IndexType index1, IndexType index2, IGraphEdgeIterator<TEdge> i)
        {
            Debug.Assert(Graph!=null);
            // see IGraph.GetEdgeWeight
            //return Graph.GetEdgeWeight(i);
            return Graph.GetVertex(index1).Location.Distance(Graph.GetVertex(i.Current.VertexId).Location);
        }
        public virtual DistanceType Estimate(IndexType index)
        {
            Debug.Assert(Graph!=null);
            return 0;
        }
        public virtual void InitializePath()
        {
            if (Path!=null)
                Path.Count = 0;
        }
        public virtual void CreatePath(Vertex<TEdge> vertex)
        {
            Debug.Assert(Manager!=null);
            if (Path!=null)
                Manager.GetVertexPath(Path, vertex);
        }
        public virtual bool IsTargetReached(IndexType index)
        { return index==TargetIndex; }
        public virtual bool IsLimitReached(int iterationCount)
        {
            Debug.Assert(Manager!=null);
	        return Manager.GetBest().F>=MaxRange || iterationCount>=MaxIterationCount ||
                Manager.VisitedVertexCount>=MaxVisitedVertexCount;
        }
        public virtual bool IsAccessible(IndexType index)
        {
            Debug.Assert(Graph!=null);
	        return Graph.IsAccessible(index);
        }
        public virtual IGraphEdgeIterator<TEdge> GetEdges(IndexType index)
        {
            BestIndex = index;
	        return Graph.GetIterator(index);
        }
        public virtual IndexType GetValue(IGraphEdgeIterator<TEdge> i)
        { return Graph.GetNeighborId(BestIndex, i); }
        public virtual void End()
        {}
    }

    public class PathManager<TEdge> : PathManagerBase<TEdge>
        where TEdge : IGraphEdge
    {
        protected GraphVertex TargetVertex;
        
        public override void Setup(IGraph<TEdge> graph, VertexManager<TEdge> vertexManager, Vector<IndexType> path,
            IndexType startIndex, IndexType targetIndex, IPathManagerParams pmParams)
        {
            base.Setup(graph, vertexManager, path, startIndex, targetIndex, pmParams);
            TargetVertex = graph.GetVertex(TargetIndex);
        }
        public override DistanceType Evaluate(IndexType index1, IndexType index2, IGraphEdgeIterator<TEdge> i)
        {
            Debug.Assert(Graph!=null);
            // XXX: see IGraph.GetEdgeWeight
            //return i.Current.PathDistance;
            return Graph.GetVertex(index1).Location.Distance(Graph.GetVertex(i.Current.VertexId).Location);
        }
        public override DistanceType Estimate(IndexType index)
        {
            Debug.Assert(Graph!=null);
            return TargetVertex.Location.Distance(Graph.GetVertex(index).Location);
        }
        public override bool IsLimitReached(int iterationCount)
        { return base.IsLimitReached(iterationCount); }
    }
}
