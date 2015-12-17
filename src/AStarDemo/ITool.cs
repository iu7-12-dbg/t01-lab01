using System;
using System.Drawing;

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
    }

    internal interface ITool
    {
        ToolId Id { get; }
        Bitmap Icons { get; }
        void Begin(Graphics g, Point pos);
        void Update(Graphics g, Point pos);
        void End();
    }
}
