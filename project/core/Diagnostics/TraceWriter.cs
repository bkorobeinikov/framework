using System.Diagnostics;

namespace Bobasoft
{
    public class TraceWriter : ITraceWriter
    {
        //======================================================
        #region _Public methods_

        public void Trace(TraceEntry entry)
        {
            switch (entry.Level)
            {
                case TraceLevel.Verbose:
                    System.Diagnostics.Trace.Write(entry.Message);
                    break;
                case TraceLevel.Info:
                    System.Diagnostics.Trace.TraceInformation(entry.Message);
                    break;
                case TraceLevel.Warning:
                    System.Diagnostics.Trace.TraceWarning(entry.Message);
                    break;
                case TraceLevel.Error:
                    if (entry.Exception != null)
                        System.Diagnostics.Trace.TraceError("{0} : {1}", entry.Message, entry.Exception);
                    else
                        System.Diagnostics.Trace.TraceError(entry.Message);
                    break;
            }
        }

        #endregion
    }
}