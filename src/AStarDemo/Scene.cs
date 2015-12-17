using System;
using System.Drawing;
using System.Collections.Generic;
using Core.Math;

namespace AStarDemo
{
    public sealed class SceneOptions
    {
        public bool ShowObjectLocations;
    }

    public sealed class SceneColors
    {
        public Color TempEdge = Color.DarkGray;
        public Color Edge = Color.CornflowerBlue;
        public Color TempVertex = Color.DarkGray;
        public Color Vertex = Color.Black;
        public Color SelectedObject = Color.Red;
        public Color SelectionBox = Color.Black;
    }

    internal sealed class Scene : IDisposable
    {
        private sealed class SceneObjectComparer : IComparer<SceneObject>
        {
            public int Compare(SceneObject x, SceneObject y)
            {
                if (x.ZOrder>y.ZOrder)
                    return 1;
                if (x.ZOrder<y.ZOrder)
                    return -1;
                if (x.Id>y.Id)
                    return 1;
                if (x.Id<y.Id)
                    return -1;
                return 0;
            }
        }

        public readonly SceneOptions Options;
        public readonly SceneColors Colors;
        public ITool CurrentTool;
        public readonly SortedSet<SceneObject> Objects;
        private readonly SceneObjectComparer comparer;
        private bool disposed = false;

        public Scene()
        {
            Options = new SceneOptions();
            Colors = new SceneColors();
            comparer = new SceneObjectComparer();
            Objects = new SortedSet<SceneObject>(comparer);
        }

        public void ProcessObjects<T>(Action<T> action)
            where T : SceneObject
        {
            foreach (var obj in Objects)
            {
                if (obj is T)
                    action((T)obj);
            }
        }

        public void ProcessObjectGroup(int groupId, Action<SceneObject> action)
        {
            foreach (var obj in Objects)
            {
                if (obj.GroupId==groupId)
                    action(obj);
            }
        }

        public IEnumerable<SceneObject> Query(Box2 box)
        {
            // XXX: could be optimized
            foreach (var obj in Objects)
            {
                if (box.Contains(obj.GetBBox()))
                    yield return obj;
            }
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    foreach (var obj in Objects)
                        obj.Dispose();
                    Objects.Clear();
                }
                DisposeHelper.OnDispose<Scene>(disposing);
                disposed = true;
            }
        }

        ~Scene()
        { Dispose(false); }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
