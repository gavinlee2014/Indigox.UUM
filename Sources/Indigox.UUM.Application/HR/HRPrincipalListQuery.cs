using System;
using System.Collections.Generic;
using Indigox.Common.NHibernateFactories;
using Indigox.UUM.HR.Model;
using NHibernate;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;
using Indigox.UUM.Application.DTO;
using Indigox.Common.Logging;

namespace Indigox.UUM.Application.HR
{
    public class HRPrincipalListQuery : Indigox.Web.CQRS.GenericListQuery<HRPrincipalDTO>
    {
        public string name { get; set; }
        public int state { get; set; }        
        public int type { get; set; }

        public override IList<HRPrincipalDTO> List()
        {
            ISession session = SessionFactories.Instance.Get(typeof(HRPrincipalDTO).Assembly).GetCurrentSession();
            {
                string sql = GetSql();
                Log.Debug(sql);
                ISQLQuery query = session.CreateSQLQuery(sql);

                query.AddEntity(typeof(HRPrincipalDTO));

                return query.List<HRPrincipalDTO>();
            }
        }
        

        public override int Size()
        {
            ISession session = SessionFactories.Instance.Get(typeof(HRPrincipalDTO).Assembly).GetCurrentSession();
            {
                string sql = "select * from v_HRPrincipal" +GetQuery();               

              //  Log.Debug(sql);
                ISQLQuery query = session.CreateSQLQuery(sql);

                query.AddEntity(typeof(HRPrincipalDTO));

                return query.List<HRPrincipalDTO>().Count;
            }
        }

        private string GetSql() {
            /*
             * 修改时间：2018-08-23
             * 修改人：曾勇
             * 修改内容：列表按照modifytime倒序进行排序
             */
            string sql = String.Format(@"
                select * from (select  row_number() OVER (ORDER BY modifytime desc) i, t.* from v_HRPrincipal as t
                 {0} ) as tt where tt.i>{1} and tt.i<={2}
            ", GetQuery(), FirstResult, FirstResult+FetchSize);
            return sql;
        }

        private string GetQuery()
        {
            string query = "";
            if (!string.IsNullOrEmpty(name))
            {
                query += " where name like '%" + name + "%' ";
            }
            if (state != -1)
            {
                if (query != "") query += " and ";
                else query += " where ";
                query += " state=" + state;
            }
            return query;
        }
    }
}