using System;
using System.Data.Services.Client;
using System.Linq;
using System.Xml.Linq;
using Bobasoft.Serialization;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Bobasoft.Cloud.Azure.AzureStorage
{
    public class AzureTableServiceContext : TableServiceContext
    {
        //======================================================
        #region _Constructors_

        public AzureTableServiceContext(string baseAddress, StorageCredentials credentials, IConverter converter)
            : base(baseAddress, credentials)
        {
            _converter = converter;
            WritingEntity += OnWritingEntity;
            ReadingEntity += OnReadingEntity;
        }

        #endregion

        //======================================================
        #region _Private, protected, internal methods_
        
        private void OnWritingEntity(object sender, ReadingWritingEntityEventArgs e)
        {
            // e.Data gives you the XElement for the Serialization of the Entity 
            //Using XLinq  , you can  add/Remove properties to the element Payload  
            var xnEntityProperties = XName.Get("properties", e.Data.GetNamespaceOfPrefix("m").NamespaceName);
            XElement xPayload = null;

            var dNamespace = DataNamespace;

            foreach (var property in e.Entity.GetType().GetProperties())
            {
                if (property.HasAttribute<CloudIgnoreAttribute>())
                {
                    if (xPayload == null)
                    {
                        xPayload = e.Data.Descendants().First(xe => xe.Name == xnEntityProperties);
                    }                    

                    //The XName of the property we are going to remove from the payload
                    var xPropertyName = XName.Get(property.Name, dNamespace);
                    //Get the Property of the entity  you don't want sent to the server
                    var xElement = xPayload.Descendants().FirstOrDefault(xe => xe.Name == xPropertyName);
                    //Remove this property from the Payload sent to the server 
                    if (xElement != null)
                        xElement.Remove();
                }

                if (property.HasAttribute<CloudIncludeAttribute>())
                {
                    if (xPayload == null)
                    {
                        xPayload = e.Data.Descendants().First(xe => xe.Name == xnEntityProperties);
                    }

                    var xPropertyName = XName.Get(property.Name, dNamespace);
                    var xDummyPropertyName = XName.Get(string.Format("dummy{0}", property.Name), dNamespace);

                    var xElement = xPayload.Descendants().FirstOrDefault(xe => xe.Name == xPropertyName);
                    if (xElement != null)
                        xElement.Remove();
                    
                    if (_converter == null)
                        throw new InvalidOperationException("There no converter specified.");

                    xElement = new XElement(xDummyPropertyName, _converter.Serialize(property.GetValue(e.Entity, null)));
                    xPayload.Add(xElement);
                }
            }
        }

        private void OnReadingEntity(object sender, ReadingWritingEntityEventArgs e)
        {
            // e.Data gives you the XElement for the Serialization of the Entity 
            //Using XLinq  , you can  add/Remove properties to the element Payload  
            var xnEntityProperties = XName.Get("properties", e.Data.GetNamespaceOfPrefix("m").NamespaceName);
            XElement xPayload = null;

            //var dNamespace = e.Data.GetNamespaceOfPrefix("d").NamespaceName;
            var dNamespace = DataNamespace;

            foreach (var property in e.Entity.GetType().GetProperties())
            {
                if (property.HasAttribute<CloudIncludeAttribute>())
                {
                    if (xPayload == null)
                    {
                        xPayload = e.Data.Descendants().First(xe => xe.Name == xnEntityProperties);
                    }

                    var xDummyPropertyName = XName.Get(string.Format("dummy{0}", property.Name), dNamespace);

                    var xElement = xPayload.Descendants().FirstOrDefault(xe => xe.Name == xDummyPropertyName);
                    if (xElement != null)
                    {
                        if (_converter == null)
                            throw new InvalidOperationException("There no converter specified.");

                        property.SetValue(e.Entity, _converter.Deserialize(xElement.Value, property.PropertyType), null);
                    }
                }
            }
        }

        #endregion

        //======================================================
        #region _Private, protected, internal fields_

        private readonly IConverter _converter;        

        #endregion
    }
}