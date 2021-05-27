using System;
using System.Collections.Generic;
using Indigox.UUM.Naming.Factory;
using Indigox.UUM.Naming.Model;

namespace Indigox.UUM.Naming.Service
{
    public class NameService
    {
        private List<INameStrategy> nameStrategies;
        private INameManager nameManager;

        public NameService()
            : this( NameStrategyManager.Instance, new ADNameManager() )
        {
        }

        /// <summary>
        /// only used for tests.
        /// </summary>
        public NameService( NameStrategyManager nameStrategyManager, INameManager nameManager )
        {
            this.nameStrategies = nameStrategyManager.GetNameStrategies();
            this.nameManager = nameManager;
        }

        public string Naming( string chineseName )
        {
            string name = string.Empty;
            INameStrategy strategy;

            for ( int i = 0; i < nameStrategies.Count; )
            {
                strategy = nameStrategies[ i ];
                name = strategy.Naming( chineseName ).ToLower();
                /***
                 * 修改日期2019-01-06
                 * 修改人：曾勇
                 * 修改原因：因为人名有英文，所以生成的name为空，不做后续校验
                 ***/
                bool nameNotExisted = false;
                if(name.Length > 0)
                {
                    //nameNotExisted = true;
                    nameNotExisted = !nameManager.Contains(name);
                }
                if ( nameNotExisted )
                {
                    break;
                }
                else
                {
                    if ( !strategy.IsReusable )
                    {
                        i++;
                    }
                }
            }
            return name;
        }
    }
}