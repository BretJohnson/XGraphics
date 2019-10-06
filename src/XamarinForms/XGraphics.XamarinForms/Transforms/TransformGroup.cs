// This file is generated from ITransformGroup.cs. Update the source file to change its contents.
using System.Collections.Generic;
using System.Linq;
using XGraphics.Transforms;
using Xamarin.Forms;

namespace XGraphics.XamarinForms.Transforms
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