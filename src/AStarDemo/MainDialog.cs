using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Core.Math;

namespace AStarDemo
{
    public partial class MainDialog : Form
    {
        private bool trackingOffset;
        private Vector2 prevPos;
        private const double ScaleDelta = 1.25f;

        public MainDialog()
        {
            KeyPreview = true;
            InitializeComponent();
            MouseWheel += OnMouseWheel;
            Root.Scene.Objects.Add(new SceneObjects.Background(Color.White));
            Root.Renderer.RenderingOutput = pbDrawingSurface;
        }

        private void OnMouseWheel(object sender, MouseEventArgs args)
        {
            var coef = args.Delta>0 ? ScaleDelta : 1/ScaleDelta;
            var newFactor = (Root.Renderer.Scale*coef).Clamp(0.01, 1000);
            Root.Renderer.Scale = newFactor;
            RedrawScene();
        }

        private void RedrawScene()
        { pbDrawingSurface.Invalidate(); }

        private void pbDrawingSurface_MouseDown(object sender, MouseEventArgs args)
        {
            switch (args.Button)
            {
            case MouseButtons.Right:
                // reserved for context menu
                break;
            case MouseButtons.Middle:
                trackingOffset = true;
                prevPos = new Vector2(args.Location.X, -args.Location.Y);
                break;
            case MouseButtons.Left:
                // XXX: notify tool
                break;
            }
        }

        private void pbDrawingSurface_MouseMove(object sender, MouseEventArgs args)
        {
            switch (args.Button)
            {
            case MouseButtons.Right:
                // reserved for context menu
                break;
            case MouseButtons.Middle:
                if (trackingOffset)
                {
                    var newPos = new Vector2(args.Location.X, -args.Location.Y);
                    var offset = (newPos-prevPos)/Root.Renderer.Scale;
                    prevPos = newPos;
                    var newOffset = Root.Renderer.Offset + Vector2.Rotate(offset, -Root.Renderer.Turn);
                    Root.Renderer.Offset = newOffset;
                    RedrawScene();
                }
                break;
            case MouseButtons.Left:
                // XXX: notify tool
                break;
            }
        }

        private void pbDrawingSurface_MouseUp(object sender, MouseEventArgs args)
        {
            switch (args.Button)
            {
            case MouseButtons.Right:
                // show context menu?
                break;
            case MouseButtons.Middle:
                trackingOffset = false;
                break;
            case MouseButtons.Left:
                // XXX: notify tool
                break;
            }
        }
    }
}
