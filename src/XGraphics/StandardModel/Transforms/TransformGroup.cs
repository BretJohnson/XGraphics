// This file is generated from ITransformGroup.cs. Update the source file to change its contents.

using System.Collections.Generic;
using System.Linq;
using XGraphics.Transforms;

namespace XGraphics.StandardModel.Transforms
{
    public class TransformGroup : Transform, ITransformGroup
    {
        public TransformGroup()
        {
            Children = new XGraphicsCollection<Transform>();
        }

        public XGraphicsCollection<Transform> Children { get; set; } = null;

        IEnumerable<ITransform> ITransformGroup.Children => Children;
    }
}