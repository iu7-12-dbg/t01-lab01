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
        public int GroupId;

        public abstract void Draw(Graphics g);
        public abstract Box2 GetBBox();
    }
}
