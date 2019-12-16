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
        public static readonly BindableProperty ChildrenProperty = PropertyUtils.Create(nameof(Children), typeof(XGraphicsCollection<Transform>), typeof(TransformGroup), null);

        public TransformGroup()
        {
            Children = new XGraphicsCollection<Transform>();
        }

        public XGraphicsCollection<Transform> Children
        {
            get => (XGraphicsCollection<Transform>)GetValue(ChildrenProperty);
            set => SetValue(ChildrenProperty, value);
        }
        IEnumerable<ITransform> ITransformGroup.Children => Children;
    }
}