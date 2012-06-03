using System;

namespace Bobasoft.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class JobAttribute : Attribute
    {
        //======================================================
        #region _Constructor_

        public JobAttribute(JobOrder order)
        {
            _order = order;
        }

        #endregion

        //======================================================
        #region _Public properties_

        public JobOrder Order
        {
            get { return _order; }
        }

        #endregion

        //======================================================
        #region _Private, protected, internal fields_

        private readonly JobOrder _order;

        #endregion
    }
}