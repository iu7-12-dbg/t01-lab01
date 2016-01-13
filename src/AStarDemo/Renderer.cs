using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Core.Math;

namespace AStarDemo
{
    internal sealed class Renderer : IDisposable
    {
        public bool AntialiasingEnabled;
        // camera parameters
        public Vector2 Offset;
        public Vector2 TurnOrigin;
        public Angle Turn;
        public double Scale;
        // ~
        public readonly Font Font;
        public readonly Brush FontBackgroundBrush;
        public event Action Rendered;
        private Func<Scene> getScene;
        private Pen redLinePen;
        private Pen greenLinePen;
        private Control drawingSurface;
        private readonly Matrix transform;
        private readonly Stopwatch timer;
        private const float FontBackgroundAlpha = 0.8f;
        private const int FontSize = 8;

        public Renderer(Func<Scene> sceneGetter)
        {
            timer = new Stopwatch();
            Font = new Font("Consolas", FontSize, GraphicsUnit.Point);
            FontBackgroundBrush = new SolidBrush(Color.FromArgb((int)(255*FontBackgroundAlpha), Color.White));
            AntialiasingEnabled = false;
            Offset = Vector2.Origin;
            TurnOrigin = Vector2.Origin;
            Turn = 0.0f;
            Scale = 1.0f;
            transform = new Matrix();
            getScene = sceneGetter;
            redLinePen = new Pen(Color.Red);
            greenLinePen = new Pen(Color.Lime);
        }

        public void Dispose()
        {
            redLinePen.Dispose();
            redLinePen = null;
            greenLinePen.Dispose();
            greenLinePen = null;
        }
        
        public Control RenderingOutput
        {
            get { return drawingSurface; }
            set
            {
                if (drawingSurface!=null)
                    drawingSurface.Paint -= OnRender;
                drawingSurface = value;
                drawingSurface.Paint += OnRender;
            }
        }

        public Matrix Transform
        {
            get
            {
                transform.Reset();
                // 1. scale relative to world origin
                transform.Scale((float)Scale, (float)Scale);
                // 2. translate to turn origin and turn
                transform.Translate((float)(TurnOrigin.X), (float)(TurnOrigin.Y));
                transform.Rotate((float)Turn.Degrees);
                transform.Translate((float)(-TurnOrigin.X), (float)(-TurnOrigin.Y));
                // 3. apply translation
                transform.Translate((float)(Offset.X), (float)(Offset.Y));
                return transform;
            }
        }
        
        private void DefaultTransform(Graphics graphics, PointF center)
        {
            var matrix = graphics.Transform;
            DefaultTransform(matrix, center);
            graphics.Transform = matrix;
        }

        private void DefaultTransform(Matrix m, PointF center)
        {
            // invert Y axis and center world origin
            m.Reset();
            m.Scale(+1.0F, -1.0F);
            m.Translate(center.X, center.Y);
        }

        private void OnRendered()
        {
            if (Rendered != null)
                Rendered();
        }

        private void OnRender(object sender, PaintEventArgs args)
        {
            Render(getScene(), args.Graphics);
            OnRendered();
        }

        private PointF GetDefaultOrigin()
        {
            var clSize = drawingSurface.ClientSize;
            return new PointF(+clSize.Width/2.0f, -clSize.Height/2.0f);
        }

        private void Render(Scene scene, Graphics graphics)
        {
            timer.Restart();
            var center = GetDefaultOrigin();
            DefaultTransform(graphics, center);
            graphics.MultiplyTransform(Transform);
            graphics.SmoothingMode = AntialiasingEnabled ? SmoothingMode.AntiAlias : SmoothingMode.Default;
            var currentTransform = graphics.Transform;
            foreach (var sceneObject in scene.Objects.Values)
            {
                if (sceneObject.Visible)
                    sceneObject.Draw(this, graphics);
            }
            DrawCartesianOrigin(graphics);
            if (scene.CurrentTool!=null)
                scene.CurrentTool.Draw(this, graphics);
            graphics.ResetTransform();
            timer.Stop();
            DrawStats(graphics, timer.Elapsed.TotalMilliseconds);
        }

        private void DrawStats(Graphics graphics, double frameMilliseconds)
        {
            const float statsOffset = 8;
            var info = String.Format("Frame time: {0} ms", frameMilliseconds.ToString("#0.00"));
            graphics.CompositingMode = CompositingMode.SourceOver;
            var infoSize = graphics.MeasureString(info, Font);
            graphics.FillRectangle(FontBackgroundBrush, statsOffset, statsOffset, infoSize.Width, infoSize.Height);
            graphics.DrawString(info, Font, Brushes.Black, statsOffset, statsOffset);
        }

        private void DrawCartesianOrigin(Graphics graphics)
        {
            const float axisLength = 32;
            const float axisArrowLength = 8;
            var invScale = (float)(1/Scale);
            var len = axisLength*invScale;
            var arrow = axisArrowLength*invScale;
            redLinePen.Width = invScale;
            graphics.DrawLine(redLinePen, 0, 0, len, 0);
            graphics.DrawLine(redLinePen, len-arrow, +arrow/4, len, 0);
            graphics.DrawLine(redLinePen, len-arrow, -arrow/4, len, 0);
            greenLinePen.Width = invScale;
            graphics.DrawLine(greenLinePen, 0, 0, 0, len);
            graphics.DrawLine(greenLinePen, +arrow/4, len-arrow, 0, len);
            graphics.DrawLine(greenLinePen, -arrow/4, len-arrow, 0, len);
        }
    }
}
