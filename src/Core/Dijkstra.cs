using System;

namespace Core
{
    public class Dijkstra<TEdge>
        where TEdge : IGraphEdge
    {
        protected bool SearchStarted = false;

        protected Dijkstra(VertexManager<TEdge> vertexManager)
        { Manager = vertexManager; }
        protected virtual void Begin<TPathManager>(TPathManager pathManager)
            where TPathManager : IPathManager<TEdge>
        {
            if (SearchStarted)
                throw new InvalidOperationException("Recursive graph engine usage is not allowed");
            SearchStarted = true;
            // initialize data structures before we started path search
            Manager.Initialize();
            // initialize path manager before we started path search
            pathManager.Initialize();
            // create a node
            Vertex<TEdge> start = Manager.CreateVertex(pathManager.StartIndex);
            // assign correspoding values to the created node
            InitializeStartNode(start, pathManager);
            // assign null parent to the start node
            Manager.SetParent(start, null);
            // add start node to the opened list
            Manager.AddOpen(start);
        }
        protected virtual void InitializeStartNode<TPathManager>(Vertex<TEdge> start, TPathManager pathManager)
            where TPathManager : IPathManager<TEdge>
        { start.F = 0; }
        protected virtual bool Step<TPathManager>(TPathManager pathManager)
            where TPathManager : IPathManager<TEdge>
        {
            // get the best node, i.e. a node with the minimum 'f'
            Vertex<TEdge> best = Manager.GetBest();
            if (pathManager.IsTargetReached(best.Index))
            {
                // we reached the goal, so we have to create a path
                pathManager.InitializePath();
                pathManager.CreatePath(best);
                // and return success
                return true;
            }
            // put best node to the closed list
            Manager.AddBestClosed();
            // and remove this node from the opened one
            Manager.RemoveBestOpen();
            // iterating on the best node neighbours
            for (var it = pathManager.GetEdges(best.Index); !it.End; it.Next())
            {
                int neighborIndex = pathManager.GetValue(it);
                // check if neighbor is accessible
                if (!pathManager.IsAccessible(neighborIndex))
                    continue;
                // check if neighbor is visited, i.e. is in the opened or closed lists
                if (Manager.IsVisited(neighborIndex))
                {
                    // so, this neighbor node has been already visited
                    // therefore get the pointer to this node
                    Vertex<TEdge> neighbor = Manager.GetVertex(neighborIndex);
                    // check if this node is in the opened list
                    if (Manager.IsOpen(neighbor))
                    {
                        // compute 'g' for the node
                        double f = best.F+pathManager.Evaluate(best.Index, neighborIndex, it);
                        // check if new path is better than the older one
                        if (neighbor.F>f)
                        {
                            // so, new path is better
                            // assign corresponding values to the node
                            double d = neighbor.F;
                            neighbor.F = f;
                            // assign correct parent to the node to be able to retreive a path
                            Manager.SetParent(neighbor, best, it.Current);
                            // notify data storage about node decreasing value
                            Manager.DecreaseOpen(neighbor, d);
                            // continue iterating on neighbours
                            continue;
                        }
                        // so, new path is worse
                        // continue iterating on neighbours
                        continue;
                    }
                    // continue iterating on neighbours
                    continue;
                }
                else
                {
                    // so, this neighbor node is not in the opened or closed lists
                    // put neighbor node to the opened list
                    Vertex<TEdge> neighbor = Manager.CreateVertex(neighborIndex);
                    // fill the corresponding node parameters
                    neighbor.F = best.F+pathManager.Evaluate(best.Index, neighborIndex, it);
                    // assign best node as its parent
                    Manager.SetParent(neighbor, best, it.Current);
                    // add start node to the open list
                    Manager.AddOpen(neighbor);
                    // continue iterating on neighbours
                    continue;
                }
            }
            // this iteration haven't got the goal node, therefore return failure
            return false;
        }
        protected virtual void End<TPathManager>(TPathManager pathManager)
            where TPathManager : IPathManager<TEdge>
        {
            // finalize path manager after we finished path search
            pathManager.End();
            SearchStarted = false;
        }

        public virtual bool Find<TPathManager>(TPathManager pathManager)
            where TPathManager : IPathManager<TEdge>
        {
            Begin(pathManager);
            for (int i = 0; !Manager.IsOpenEmpty(); i++)
            {
                if (pathManager.IsLimitReached(i))
                {
                    End(pathManager);
                    return false;
                }
                if (Step(pathManager))
                {
                    End(pathManager);
                    return true;
                }
            }
            End(pathManager);
            return false;
        }
        public VertexManager<TEdge> Manager { get; protected set; }
    }
}
