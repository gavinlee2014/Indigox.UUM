using System;
using Indigox.UUM.Sync.Interfaces;

namespace Indigox.UUM.Sync.Tests.Model
{
    /*
     * 1、只能序列化 public 类
     * 2、必须实现无参数的构造函数
     */

    public class SerializableExecutor : ISyncExecutor
    {
        private string webServiceName;
        private string method;
        private string url;

        public string WebServiceName
        {
            get { return webServiceName; }
            set { webServiceName = value; }
        }

        public string Method
        {
            get { return method; }
            set { method = value; }
        }

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public SerializableExecutor()
        {
        }

        public SerializableExecutor( string webServiceName, string method, string url )
        {
            this.webServiceName = webServiceName;
            this.method = method;
            this.url = url;
        }

        public void Execute( ISyncContext context )
        {
        }
    }
}