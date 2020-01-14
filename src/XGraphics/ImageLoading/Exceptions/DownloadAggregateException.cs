using System;
using System.Collections.Generic;

namespace XGraphics
{
    public class DownloadAggregateException : AggregateException
    {
        public DownloadAggregateException()
        {
        }

        public DownloadAggregateException(IEnumerable<Exception> exceptions) : base(exceptions)
        {
        }
    }
}
