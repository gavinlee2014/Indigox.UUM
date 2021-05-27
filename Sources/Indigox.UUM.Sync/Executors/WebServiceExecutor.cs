using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Services.Protocols;
using Indigox.UUM.Sync.Interface;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.Model;
using Indigox.Common.Logging;

namespace Indigox.UUM.Sync.Executors
{
    public class WebServiceExecutor : ISyncExecutor
    {
        public string WebServiceType { get; set; }
        public string Method { get; set; }
        public string Url { get; set; }

        public virtual void Execute( ISyncContext context )
        {
            InvokeWebService( context );
        }

        protected virtual object InvokeWebService( ISyncContext context )
        {
            Type serviceType = Type.GetType( WebServiceType );

            SoapHttpClientProtocol service = (SoapHttpClientProtocol)Activator.CreateInstance( serviceType, Url );
            if ( service == null )
            {
                throw new ApplicationException( string.Format( "无法创建类 {0} 的实例。", serviceType.FullName ) );
            }

            MethodInfo method = serviceType.GetMethod( Method );
            if ( method == null )
            {
                throw new ApplicationException( string.Format( "在类 {1} 的实例中找不到方法 {0}。", Method, serviceType.FullName ) );
            }

            object[] methodArgs = GetArguments( context, method );

            return method.Invoke( service, methodArgs );
        }

        private object[] GetArguments( ISyncContext context, MethodInfo method )
        {
            ParameterInfo[] methodArgDefines = method.GetParameters();

            object[] methodArgs = new object[ methodArgDefines.Length ];

            for ( int i = 0; i < methodArgDefines.Length; i++ )
            {
                ParameterInfo methodArgDefine = methodArgDefines[ i ];
                object value = context.Get( methodArgDefine.Name );

                if ( value is IDictionary<string, object> )
                {
                    var v = (IDictionary<string, object>)value;
                    var newVal = new Dictionary<string, object>();
                    foreach (var key in v.Keys)
                    {
                        if (v[key] is IDictionary<string, string>)
                        {
                            newVal.Add(key, new PropertyChangeCollection((IDictionary<string, string>)v[key]));
                        }
                        else
                        {
                            newVal.Add(key, v[key]);
                        }
                    }
                    methodArgs[ i ] = new PropertyChangeCollection(newVal);
                }
                else if (value is IDictionary<string, string>)
                {
                    methodArgs[i] = new PropertyChangeCollection((IDictionary<string, string>)value);

                }
                else if (value is ExternalObjectDescriptor)
                {
                    ExternalObjectDescriptor externalDescriptor = value as ExternalObjectDescriptor;
                    methodArgs[i] = externalDescriptor.GetExternalID();
                }
                else
                {
                    methodArgs[i] = value;
                }
            }
            return methodArgs;
        }
    }
}