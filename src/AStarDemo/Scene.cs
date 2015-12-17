using System;
using System.Collections.Generic;

namespace AStarDemo
{
    internal sealed class Scene
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

        public readonly SortedSet<SceneObject> Objects;
        private readonly SceneObjectComparer comparer;

        public Scene()
        {
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
    }
}
