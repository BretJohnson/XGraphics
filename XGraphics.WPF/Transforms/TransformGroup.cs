using System.Collections.Generic;
using System.Linq;
using XGraphics.Transforms;
using System.Windows.Markup;

namespace XGraphics.WPF.Transforms
{
    [ContentProperty("Children")]
    public class TransformGroup : Transform, ITransformGroup
    {
        public TransformGroup()
        {
            Children = new GraphicsObjectCollection<Transform>();
            Children.Changed += OnSubobjectChanged;
        }

        IEnumerable<ITransform> ITransformGroup.Children => Children;
        public GraphicsObjectCollection<Transform> Children
        {
            get;
        }
    }
}