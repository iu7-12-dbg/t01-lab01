namespace AStarDemo
{
    using System.Drawing;
    using Core.Math;

    internal abstract class SceneObject
    {
        public int ZOrder;
        public Color Color;
        public bool Visible = true;
        public bool Selected;
        public int Id;
        public int GroupId;

        public abstract void Draw(Renderer r, Graphics g);
        public abstract Box2 GetBBox();
    }
}
