namespace AStarDemo
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using Core.Math;

    internal abstract class SceneObject : IDisposable
    {
        public int ZOrder;
        public Color Color;
        public bool Visible = true;
        public bool Selected;
        public readonly ulong Id;
        public int GroupId;
        private GCHandle handle;
        private bool disposed = false;

        public SceneObject()
        {
            handle = GCHandle.Alloc(this, GCHandleType.Weak);
            Id = unchecked((ulong)GCHandle.ToIntPtr(handle).ToInt64());
        }

        public abstract void Draw(Renderer r, Graphics g);
        public abstract Box2 GetBBox();
        
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    PureDispose();
                DisposeHelper.OnDispose<SceneObject>(disposing);
                disposed = true;
            }
        }

        protected virtual void PureDispose()
        { handle.Free(); }
        
        ~SceneObject()
        { Dispose(false); }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
    // XXX: move to core
    public static class DisposeHelper
    {
        [Conditional("DEBUG")]
        public static void OnDispose<T>(bool disposing)
        {
            if (disposing)
                return;
            if (!AppDomain.CurrentDomain.IsFinalizingForUnload() && !Environment.HasShutdownStarted)
                Debug.Fail("Non-disposed object finalization: "+typeof(T).FullName);
        }
    }
}
