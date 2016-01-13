using System;
using System.Drawing;
using Core.Math;

namespace AStarDemo
{
    internal enum ToolState
    {
        Inactive = 0,
        Disabled = 1,
        Active = 2,
    }

    internal enum ToolId
    {
        Add,
        Select,
        PathFinder,
    }

    internal interface ITool
    {
        ToolId Id { get; }
        Bitmap Icons { get; }
        void Begin(Vector2 pos);
        void Update(Vector2 pos);
        void End(Vector2 pos);
        void Draw(Renderer r, Graphics g);
    }
}
