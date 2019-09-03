using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XGraphics.XamarinForms.iOS;

[assembly: ExportRenderer(typeof(XGraphics.XamarinForms.XGraphics), typeof(XGraphicsViewRenderer))]

namespace XGraphics.XamarinForms.iOS
{
    public class XGraphicsViewRenderer : ViewRenderer<XGraphics, UIView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<XGraphics> e)
        {
            if (e.NewElement != null)
            {
                XGraphics xGraphics = e.NewElement;

                // create the native view
                if (Control == null)
                {
                    XGraphicsRenderer? xGraphicsRenderer = XGraphicsRenderer.DefaultRenderer;
                    if (xGraphicsRenderer == null)
                        throw new InvalidOperationException("XGraphicsRenderer.DefaultRender isn't set; set it in your Xamarin.Forms platform startup code");

                    IXGraphicsView graphicsView = XGraphicsRenderer.DefaultRenderer.CreateGraphicsView();
                    var nativeControl = (UIView)graphicsView.NativeControl;

                    // TODO: Handle thread safety issues if necessary, like if the object goes away while it's rendering on its separate thread
                    graphicsView.Content = xGraphics;

                    SetNativeControl(nativeControl);
                }

                // start the rendering
                //SetupRenderLoop(false);
            }

            base.OnElementChanged(e);
        }
    }
}
