using System;
using System.Reflection;
using Indigox.Common.Utilities;

namespace Indigox.UUM.Sync.Web
{
    [Obsolete( "临时方案" )]
    internal class NHibernateFactoryInvoker
    {
        protected NHibernateFactoryInvoker()
        {
        }

        private const string SessionFactoriesClassName = "Indigox.Common.NHibernateFactories.SessionFactories, Indigox.Common.NHibernateFactories";

        private static NHibernateFactoryInvoker instance;
        private static object factories;
        private static object factory;

        private static object Factories
        {
            get
            {
                if ( factories == null )
                {
                    //==> factories = SessionFactories.Instance;
                    factories = ReflectUtil.GetStaticProperty( Type.GetType( SessionFactoriesClassName, true ), "Instance" );
                }
                return factories;
            }
        }

        private static object Factory
        {
            get
            {
                if ( factory == null )
                {
                    //==> factory = factories.Get( typeof( Setting ).Assembly );
                    factory = ReflectUtil.InvokeMethod( Factories, "Get", new Type[] { typeof( Assembly ) }, new object[] { Assembly.GetCallingAssembly() } );
                }
                return factory;
            }
        }

        public static NHibernateFactoryInvoker Instance
        {
            get
            {
                if ( instance == null )
                {
                    instance = new NHibernateFactoryInvoker();
                }
                return instance;
            }
        }

        public void InitCurrentSession()
        {
            //==> factory.InitCurrentSession();
            ReflectUtil.InvokeMethod( Factory, "InitCurrentSession", Type.EmptyTypes, new object[] { } );
        }

        public void FlushSession()
        {
            //==> factory.FlushSession();
            ReflectUtil.InvokeMethod( Factory, "FlushSession", Type.EmptyTypes, new object[] { } );
        }

        public void TryCommitTransaction()
        {
            //==> factory.TryCommitTransaction();
            ReflectUtil.InvokeMethod( Factory, "TryCommitTransaction", Type.EmptyTypes, new object[] { } );
        }
    }
}