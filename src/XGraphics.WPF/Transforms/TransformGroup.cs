// This file is generated from ITransformGroup.cs. Update the source file to change its contents.

using System.Collections.Generic;
using System.Linq;
using XGraphics.Transforms;
using System.Windows;
using System.Windows.Markup;

namespace XGraphics.WPF.Transforms
{
    [ContentProperty("Children")]
    public class TransformGroup : Transform, ITransformGroup
    {
        public static readonly DependencyProperty ChildrenProperty = PropertyUtils.Create(nameof(Children), typeof(XGraphicsCollection<Transform>), typeof(TransformGroup), null);

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