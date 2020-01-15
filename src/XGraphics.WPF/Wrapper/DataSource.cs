namespace XGraphics.WPF.Wrapper
{
    public class DataSource
    {
        public XGraphics.DataSource WrappedDataSource { get; }

        public DataSource(XGraphics.DataSource wrappedDataSource)
        {
            WrappedDataSource = wrappedDataSource;
        }
    }
}
