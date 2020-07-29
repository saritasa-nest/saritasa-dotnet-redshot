using Eto.Forms.Controls.SkiaSharp.WinForms.Native;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;

namespace Eto.Forms.Controls.SkiaSharp.WinForms
{
    public class SKControlGlWinForms : global::SkiaSharp.Views.Desktop.SKGLControl
    {
        private SKSurface surface;
        private GRContext grContext;
        private GRBackendRenderTarget renderTarget;

        public void Execute(Action<SKSurface> surfaceAction)
        {
            if (grContext == null)
            {
                var glInterface = GRGlInterface.CreateNativeGlInterface();
                grContext = GRContext.Create(GRBackend.OpenGL, glInterface);
            }

            renderTarget = CreateRenderTarget();
            // update to the latest dimensions
            renderTarget = new GRBackendRenderTarget(Width - 1, Height - 1, renderTarget.SampleCount, renderTarget.StencilBits, renderTarget.GetGlFramebufferInfo());

            if (surface == null)
            {
                surface = SKSurface.Create(grContext, renderTarget, SKColorType.Rgba8888);
            }

            surfaceAction.Invoke(surface);

            // start drawing
            OnPaintSurface(new SKPaintGLSurfaceEventArgs(surface, renderTarget));

            surface.Canvas.Flush();

            Refresh();
        }

        public SKControlGlWinForms()
        {
            
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            // update the control
            SwapBuffers();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            // clean up
            if (grContext != null)
            {
                grContext.Dispose();
                grContext = null;
            }
        }

        public static GRBackendRenderTarget CreateRenderTarget()
        {
            Gles.glGetIntegerv(Gles.GL_FRAMEBUFFER_BINDING, out int framebuffer);
            Gles.glGetIntegerv(Gles.GL_STENCIL_BITS, out int stencil);
            Gles.glGetIntegerv(Gles.GL_SAMPLES, out int samples);

            int bufferWidth = 0;
            int bufferHeight = 0;

            return new GRBackendRenderTarget(bufferWidth, bufferHeight, samples, stencil, new GRGlFramebufferInfo((uint)framebuffer, GRPixelConfig.Rgba8888.ToGlSizedFormat()));
        }
    }
}
