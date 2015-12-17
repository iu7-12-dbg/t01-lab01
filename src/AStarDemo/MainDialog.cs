using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Core.Math;

namespace AStarDemo
{
    public partial class MainDialog : Form
    {
        private class ToolContainer
        {
            public ITool Tool;
            public Button Button;

            public ToolContainer(ITool tool, Button btn)
            {
                Tool = tool;
                Button = btn;
            }
        }
        private ToolId currentTool = ToolId.Select;
        private bool toolActive;
        private Dictionary<ToolId, ToolContainer> tools;

        private bool trackingOffset;
        private Vector2 prevPos;
        private const double ScaleDelta = 1.25f;

        public MainDialog()
        {
            KeyPreview = true;
            InitializeComponent();
            InitializeObjectSelector();
            InitializeLocationToggle();
            tools = new Dictionary<ToolId, ToolContainer>();
            InitializeToolButton(new ToolContainer(new Tools.Add(CreateObject, AdjustObject), btnAdd));
            InitializeToolButton(new ToolContainer(new Tools.Select(), btnSelect));
            SetCurrentTool(currentTool, true);
            MouseWheel += OnMouseWheel;
            Root.Scene.Objects.Add(new SceneObjects.Background(Color.White));
            Root.Renderer.RenderingOutput = pbDrawingSurface;
        }

        private SceneObject CreateObject(Vector2 pos)
        {
            switch (btnObject.ImageIndex)
            {
            case 0: // vertex
                return new SceneObjects.Vertex {Location = pos, ShowLocation = true};
            case 1: // edge
            {
                var treshold = 3/Root.Renderer.Scale;
                foreach (var obj in Root.Scene.Query(new Box2(pos, treshold)))
                {
                    var v = obj as SceneObjects.Vertex;
                    if (v!=null) // snap to vertices only
                        return new SceneObjects.Edge(new Edge(v.Location, v.Location));
                }
                return null;
            }
            default: // unknown
                return null;
            }
        }

        private void AdjustObject(SceneObject obj, Vector2 pos, bool release)
        {
            switch (btnObject.ImageIndex)
            {
            case 0: // vertex
            {
                Debug.Assert(obj is SceneObjects.Vertex);
                var vertex = (SceneObjects.Vertex)obj;
                vertex.Location = pos;
                if (release)
                {
                    vertex.Color = Color.DarkGray;
                    vertex.ShowLocation = false;
                    Root.Scene.Objects.Add(vertex);
                }
                break;
            }
            case 1: // edge
            {
                Debug.Assert(obj is SceneObjects.Edge);
                var edge = (SceneObjects.Edge)obj;
                var treshold = 3/Root.Renderer.Scale;
                bool snap = false;
                foreach (var qobj in Root.Scene.Query(new Box2(pos, treshold)))
                {
                    var v = qobj as SceneObjects.Vertex;
                    if (v!=null) // snap to vertices only
                    {
                        pos = v.Location;
                        snap = true;
                        break;
                    }
                }
                edge.B = pos;
                if (release)
                {
                    if (snap)
                    {
                        edge.Color = Color.LightGray;
                        Root.Scene.Objects.Add(edge);
                    }
                    else // drop invalid edge, but dispose it first
                        edge.Dispose();
                }
                break;
            }
            default: // unknown
                return;
            }
        }

        private void InitializeObjectSelector()
        {
            btnObject.ImageList = new ImageList();
            btnObject.ImageList.Images.AddStrip(Properties.Resources.Objects);
            btnObject.ImageIndex = 0;
            btnObject.Text = String.Empty;
            btnObject.Click += (sender, e) =>
            {
                // XXX: implement object type switching
                btnObject.ImageIndex = (btnObject.ImageIndex+1)%2;
            };
        }

        private void InitializeLocationToggle()
        {
            btnLocToggle.ImageList = new ImageList();
            btnLocToggle.ImageList.Images.AddStrip(Properties.Resources.Location);
            btnLocToggle.ImageIndex = 1; // i.e. disabled
            btnLocToggle.Text = String.Empty;
            btnLocToggle.Click += (sender, e) =>
            {
                btnLocToggle.ImageIndex = (btnLocToggle.ImageIndex+1)%2;
                Root.Scene.Options.ShowObjectLocations = btnLocToggle.ImageIndex==0;
                RedrawScene();
            };
        }

        private void InitializeToolButton(ToolContainer toolContainer)
        {
            var list = new ImageList();
            list.Images.AddStrip(toolContainer.Tool.Icons);
            toolContainer.Button.ImageList = list;
            toolContainer.Button.Text = string.Empty;
            toolContainer.Button.Click += (sender, e) => SetCurrentTool(toolContainer.Tool.Id);
            tools.Add(toolContainer.Tool.Id, toolContainer);
        }

        private bool SetCurrentTool(ToolId id, bool force = false)
        {
            if (!force && currentTool==id)
                return false;
            currentTool = id;
            Root.Scene.CurrentTool = GetCurrentTool();
            foreach (var tc in tools.Values)
                tc.Button.ImageIndex = (int)ToolState.Inactive;
            tools[id].Button.ImageIndex = (int)ToolState.Active;
            return true;
        }

        private ITool GetCurrentTool()
        { return tools[currentTool].Tool; }

        private void OnMouseWheel(object sender, MouseEventArgs args)
        {
            var coef = args.Delta>0 ? ScaleDelta : 1/ScaleDelta;
            var newFactor = (Root.Renderer.Scale*coef).Clamp(0.01, 1000);
            Root.Renderer.Scale = newFactor;
            RedrawScene();
        }

        private void RedrawScene()
        { pbDrawingSurface.Invalidate(); }

        private Vector2 ScreenToWorld(Point screenPos)
        {
            var invScale = 1/Root.Renderer.Scale;
            var invOffset = -Root.Renderer.Offset;
            var clSize = Root.Renderer.RenderingOutput.ClientSize;
            screenPos.X -= clSize.Width/2;
            screenPos.Y -= clSize.Height/2;
            var pos = new Vector2(screenPos.X, -screenPos.Y);
            pos *= invScale;
            pos += invOffset;
            return pos;
        }

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
                toolActive = true;
                GetCurrentTool().Begin(ScreenToWorld(args.Location));
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
                if (toolActive)
                {
                    GetCurrentTool().Update(ScreenToWorld(args.Location));
                    RedrawScene();
                }
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
                if (toolActive)
                {
                    GetCurrentTool().End(ScreenToWorld(args.Location));
                    RedrawScene();
                }
                break;
            }
        }
    }
}
