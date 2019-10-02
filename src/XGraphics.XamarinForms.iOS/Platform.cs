namespace XGraphics.XamarinForms.iOS
{
    public class Platform
    {
        public static void Init(XGraphicsRenderer? defaultRenderer)
        {
            if (defaultRenderer != null)
                XGraphicsRenderer.DefaultRenderer = defaultRenderer;
        }
    }
}
