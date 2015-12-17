using System;
using System.Drawing;
using Core.Math;

namespace AStarDemo.Tools
{
    internal class Add : ITool
    {
        public delegate SceneObject CreateObjectDelegate(Vector2 pos);
        public delegate void AdjustObjectDelegate(SceneObject obj, Vector2 pos, bool release);

        public ToolId Id { get { return ToolId.Add; } }
        public Bitmap Icons { get { return Properties.Resources.Add; } }

        private CreateObjectDelegate createObject;
        private AdjustObjectDelegate adjustObject;
        private SceneObject obj;

        public Add(CreateObjectDelegate createObject, AdjustObjectDelegate adjustObject)
        {
            this.createObject = createObject;
            this.adjustObject = adjustObject;
        }

        public void Begin(Vector2 pos)
        { obj = createObject(pos); }
        public void Update(Vector2 pos)
        {
            if (obj!=null)
                adjustObject(obj, pos, false);
        }
        public void End(Vector2 pos)
        {
            if (obj!=null)
            {
                adjustObject(obj, pos, true);
                obj = null;
            }
        }
        public void Draw(Renderer r, Graphics g)
        {
            if (obj!=null)
                obj.Draw(r, g);
        }
    }
}
