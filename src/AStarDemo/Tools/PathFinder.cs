using System;
using System.Drawing;
using Core.Math;
using System.Diagnostics;

namespace AStarDemo.Tools
{
    internal class PathFinder : ITool
    {
        public delegate SceneObjects.Vertex CaptureVertexDelegate(Vector2 pos);

        public ToolId Id { get { return ToolId.PathFinder; } }
        public Bitmap Icons { get { return null; } }


        private class PmParams : Core.IPathManagerParams
        {
            public int MaxIterationCount { get; set; }
            public double MaxRange { get; set; }
            public int MaxVisitedVertexCount { get; set; }
        }

        private CaptureVertexDelegate captureVertex;
        private SceneObjects.Vertex startVertex;
        private Core.Vector<int> path = new Core.Vector<int>();
        private bool found;
        private Core.Vector<SceneObjects.Edge> edgePath = new Core.Vector<SceneObjects.Edge>();
        private bool cached;

        public PathFinder(CaptureVertexDelegate captureVertex)
        { this.captureVertex = captureVertex; }

        public void Begin(Core.Math.Vector2 pos)
        {
            startVertex = captureVertex(pos);
            found = false;
            cached = false;
            edgePath.Count = 0;
        }

        public void Update(Core.Math.Vector2 pos)
        {}

        public void End(Core.Math.Vector2 pos)
        {
            if (startVertex==null)
                return;
            var endVertex = captureVertex(pos);
            if (endVertex==null)
                return;
            if (startVertex.Id==endVertex.Id)
                return;
            path.Count = 0;
            var pmParams = new PmParams();
            pmParams.MaxIterationCount = 1000;
            pmParams.MaxRange = Double.MaxValue;
            pmParams.MaxVisitedVertexCount = 100;
            found = Root.GraphEngine.Search(Root.Graph,
                startVertex.GraphVertexId, endVertex.GraphVertexId, path, pmParams);
            edgePath.EnsureCapacity(path.Count-1);
        }

        public void Draw(Renderer r, Graphics g)
        {
            if (!found)
                return;
            if (cached)
            {
                for (int i = 0; i<edgePath.Count; i++)
                {
                    edgePath[i].Selected = true;
                    edgePath[i].Draw(r, g);
                    edgePath[i].Selected = false;
                }
                return;
            }
            ulong lastVertexId = 0;
            int lastIndex = 0;
            for (int i = 0; i<path.Count-1; i++)
            {
                var gid1 = Root.Graph.GetVertex(path[i]).SceneObjectId;
                var gid2 = Root.Graph.GetVertex(path[i+1]).SceneObjectId;
                var id1 = Root.Scene.GetObject(gid1).Id;
                var id2 = Root.Scene.GetObject(gid2).Id;
                SceneObjects.Edge edge = null;
                foreach (var obj in Root.Scene.Objects.Values)
                {
                    edge = obj as SceneObjects.Edge;
                    if (edge==null)
                        continue;
                    var idA = edge.VertexA.Id;
                    var idB = edge.VertexB.Id;
                    if ((idA==id1 || idA==id2) && (idB==id1 || idB==id2))
                        break;
                    edge = null;
                }
                Debug.Assert(edge!=null);
                edgePath.Push(edge);
                lastVertexId = id2;
                lastIndex = i+1;
            }
            if (lastIndex!=path.Count-1)
            {
                var gid2 = Root.Graph.GetVertex(path[path.Count-1]).SceneObjectId;
                var id1 = lastVertexId;
                var id2 = Root.Scene.GetObject(gid2).Id;
                SceneObjects.Edge edge = null;
                foreach (var obj in Root.Scene.Objects.Values)
                {
                    edge = obj as SceneObjects.Edge;
                    if (edge==null)
                        continue;
                    var idA = edge.VertexA.Id;
                    var idB = edge.VertexB.Id;
                    if ((idA==id1 || idA==id2) && (idB==id1 || idB==id2))
                        break;
                    edge = null;
                }
                Debug.Assert(edge!=null);
                edgePath.Push(edge);
            }
            cached = true;
            Draw(r, g);
        }
    }
}
