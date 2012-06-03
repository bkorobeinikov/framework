namespace Bobasoft
{
    public interface ITraceWriter
    {
        //======================================================
        #region _Methods_

        void Trace(TraceEntry entry);

        #endregion
    }
}