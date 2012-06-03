using System;
using System.Diagnostics;

namespace Bobasoft
{
    public static class Diagnostics
    {
        //======================================================
        #region _Public methods_

        public static void Initialize(ITraceWriter writer)
        {
            _writer = writer;
        }

        public static void TraceInformation(string message, object values = null)
        {
            var entry = new TraceEntry(TraceLevel.Info, message, values);
            _writer.Trace(entry);
        }

        public static void TraceError(string message, object values = null)
        {
            var entry = new TraceEntry(TraceLevel.Error, message, values);
            _writer.Trace(entry);
        }

        public static void TraceWarning(string message, object values = null)
        {
            var entry = new TraceEntry(TraceLevel.Warning, message, values);
            _writer.Trace(entry);
        }

        public static void Trace(string message, Exception exception, object values = null)
        {
            var ex = exception.InnerException ?? exception;
            var entry = new TraceEntry(TraceLevel.Error, message, ex.Message, ex.StackTrace, values);
            _writer.Trace(entry);
        }

        public static void Trace(string message, object values = null)
        {
            var entry = new TraceEntry(TraceLevel.Verbose, message, values);
            _writer.Trace(entry);
        }

        #endregion

        //======================================================
        #region _Private, protected, internal fields_

        private static ITraceWriter _writer;

        #endregion
    }
}