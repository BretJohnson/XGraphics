﻿using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using XGraphics.XamarinForms.Android;
using System;

[assembly: ExportRenderer(typeof(XGraphics.XamarinForms.XGraphics), typeof(XGraphicsViewRenderer))]

namespace XGraphics.XamarinForms.Android
{
    public class XGraphicsViewRenderer : ViewRenderer<XGraphics, global::Android.Views.View>
    {
        public XGraphicsViewRenderer(Context context) : base(context)
        {
        }

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

                    IXGraphicsView graphicsView = XGraphicsRenderer.DefaultRenderer.CreateGraphicsView(Context);
                    var nativeControl = (global::Android.Views.View)graphicsView.NativeControl;

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
