using System.Windows;
using XGraphics;
using XGraphics.SkiaRenderer;

namespace WPFDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            XGraphicsRenderer.DefaultRenderer = new SkiaXGraphicsRenderer();
        }
    }
}
