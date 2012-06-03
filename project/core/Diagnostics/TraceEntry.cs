using System;
using System.Diagnostics;
using Bobasoft.Collections;

namespace Bobasoft
{
    public class TraceEntry
    {
        //======================================================
        #region _Constructors_

        public TraceEntry(TraceLevel level, string message, object values = null)
        {
            Level = level;
            Message = message;
            Values = new DataValueDictionary(values);
            TraceTime = DateTime.UtcNow;
        }

        public TraceEntry(TraceLevel level, string message, string exception, string stackTrace, object values = null)
        {
            Level = level;
            Message = message;
            Exception = exception;
            StackTrace = stackTrace;
            Values = new DataValueDictionary(values);
            TraceTime = DateTime.UtcNow;
        }

        #endregion

        //======================================================
        #region _Public properties_

        public TraceLevel Level { get; protected set; }
        public string Message { get; protected set; }
        public DataValueDictionary Values { get; protected set; }

        public string Exception { get; internal protected set; }
        public string StackTrace { get; internal protected set; }

        public DateTime TraceTime { get; protected internal set; }

        #endregion
    }
}