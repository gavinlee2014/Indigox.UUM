using System;
using System.Reflection;
using Indigox.Common.Utilities;
using Indigox.UUM.Sync.Interfaces;

namespace Indigox.UUM.Application.Utils
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
                    factory = ReflectUtil.InvokeMethod( Factories, "Get", new Type[] { typeof( Assembly ) }, new object[] { typeof( ISyncTask ).Assembly } );
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

        private static readonly Type ISessionType = Type.GetType( "NHibernate.ISession, NHibernate" );

        public void InitCurrentSession()
        {
            //==> factory.InitCurrentSession();
            object factory = Factory;
            object hbFactory = ReflectUtil.GetProperty( factory, "CurrentSessionFactory" );
            object session = ReflectUtil.InvokeMethod( hbFactory, "OpenSession", Type.EmptyTypes, new object[] { } );
            ReflectUtil.InvokeMethod( factory, "BindSession", new Type[] { ISessionType }, new object[] { session } );
        }

        public void FlushSession()
        {
            //==> factory.FlushSession();
            ReflectUtil.InvokeMethod( Factory, "FlushSession", Type.EmptyTypes, new object[] { } );
        }
    }
}