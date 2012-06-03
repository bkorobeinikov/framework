using System;

namespace Bobasoft
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public class DependencyAttribute : Attribute
    {
        //======================================================
        #region _Constructors_

        public DependencyAttribute(string name)
        {
            Name = name;
        }

        #endregion

        //======================================================
        #region _Public properties_

        public string Name { get; protected set; }        

        #endregion
    }
}