using System;

namespace Bobasoft.Cloud.Azure
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class CloudIgnoreAttribute : Attribute
    {
    }
}