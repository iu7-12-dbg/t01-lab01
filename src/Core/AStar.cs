using System;
using System.Collections.Generic;

namespace Core
{
    public class AStar<TEdge> : Dijkstra<TEdge>
        where TEdge : IGraphEdge
    {
        public AStar(VertexManager<TEdge> vertexManager) : base(vertexManager)
        {}

        protected override void InitializeStartNode<TPathManager>(Vertex<TEdge> start, TPathManager pathManager)
        {
            base.InitializeStartNode(start, pathManager);
            start.H = pathManager.Estimate(start.Index);
            start.F = start.G+start.H;
        }

        protected override bool Step<TPathManager>(TPathManager pathManager)
        {
            // get the best node, i.e. a node with the minimum 'f'
            Vertex<TEdge> best = Manager.GetBest();
            // check if this node is the one we are searching for
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
                        double g = best.G+pathManager.Evaluate(best.Index, neighborIndex, it);
                        // check if new path is better than the older one
                        if (neighbor.G>g)
                        {
                            // so, new path is better
                            // assign corresponding values to the node
                            double d = neighbor.F;
                            neighbor.G = g;
                            neighbor.F = neighbor.G+neighbor.H;
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
                    // XXX: correct for Dijkstra and A* with non over-estimated consistent heuristics
                    // (euclidian heuristics is always consistent)
                    // ... otherwise, need a special check here
                    // XXX: add support for non-euclidian heuristics
                    // continue iterating on neighbours
                    continue;
                }
                else
                {
                    // so, this neighbor node is not in the opened or closed lists
                    // put neighbor node to the opened list
                    Vertex<TEdge> neighbor = Manager.CreateVertex(neighborIndex);
                    // fill the corresponding node parameters
                    neighbor.G = best.G+pathManager.Evaluate(best.Index, neighborIndex, it);
                    neighbor.H = pathManager.Estimate(neighbor.Index);
                    neighbor.F = neighbor.G+neighbor.H;
                    // assign best node as its parent
                    Manager.SetParent(neighbor, best, it.Current);
                    // add start node to the opened list
                    Manager.AddOpen(neighbor);
                    // continue iterating on neighbours
                    continue;
                }
            }
            // this iteration haven't got the goal node, therefore return failure
            return false;
        }

        public override bool Find<TPathManager>(TPathManager pathManager)
        {
            bool found = false;
            Begin(pathManager);
            for (int i = 0; !Manager.IsOpenEmpty(); i++)
            {
                if (pathManager.IsLimitReached(i))
                    break;
                if (Step(pathManager))
                {
                    found = true;
                    break;
                }
            }
            End(pathManager);
            return found;
        }
    }
}
