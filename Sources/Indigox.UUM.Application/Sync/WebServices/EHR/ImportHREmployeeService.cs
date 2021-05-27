using System;
using System.Web.Services;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.DomainModels.Specifications;
using Indigox.UUM.HR.Model;
using Indigox.UUM.HR.Service;
using Indigox.UUM.HR.Setting;
using Indigox.UUM.Naming.Service;
using Indigox.UUM.Sync.Interface;
using Indigox.UUM.Sync.Interface.Client;
using Indigox.UUM.Application.WebReference;
using Indigox.UUM.Naming.Util;
using Indigox.Common.Data;
using Indigox.Common.Data.Interface;
using Indigox.Common.Logging;
using Indigox.Common.Membership.Interfaces;
using System.Collections.Generic;

namespace Indigox.UUM.Application.Sync.WebServices.EHR
{
    [WebServiceBinding( Name = "ImportUserService", Namespace = Consts.Namespace_HR )]
    public class ImportEHREmployeeService : WebService, IImportUserService
    {
        public string SyncUser(string nativeID, string organizationalUnitID, string accountName, string name, string fullName, string displayName, string email, string title, string mobile, string telephone, string fax, double orderNum, string description, string otherContact, string portrait, string mailDatabase, PropertyChangeCollection extendProperties)
        {
            return nativeID;
        }

        public string Create(string nativeID, string organizationalUnitID, string accountName, string name, string fullName, string displayName, string email, string title, string mobile, string telephone, string fax, double orderNum, string description, string otherContact, string portrait, string mailDatabase, PropertyChangeCollection extendProperties)
        {
            IDatabase database = new DatabaseFactory().CreateDatabase("UUM");
            string sqlStr = "select HRObjectID from HRMapping where PrincipalID = '" + nativeID + "'";
            IRecordSet recordset = database.QueryText(sqlStr);
            if(recordset.Records.Count> 0)
            {
                string jobno = recordset.Records[0].GetString("HRObjectID");
                ZydcWebServiceImplService service = new ZydcWebServiceImplService();
                service.pushInfoByJobno(jobno, accountName, email,nativeID);
                Log.Error("uum创建，回写HR系统数据：HRid：" + jobno + ", AccountName:" + accountName + ",UserId:" + nativeID);
            }
            
            
            return nativeID;
        }

        public void Delete( string userID )
        {
            
        }

        public void Disable( string userID )
        {
            
        }
                

        public void Enable(string userID, string organizationalUnitID, string accountName, string name, string fullName, string displayName, string email, string title, string mobile, string telephone, string fax, double orderNum, string description, string otherContact, string portrait, string mailDatabase)
        {
            
        }

        public void ChangeProperty( string userID, PropertyChangeCollection propertyChanges )
        {
            //Log.Error("EHR同步修改用户:" + userID);
            IDatabase database = new DatabaseFactory().CreateDatabase("UUM");
            string sqlStr = "select HRObjectID from HRMapping where PrincipalID = '" + userID + "'";
            //Log.Error(sqlStr);
            IRecordSet recordset = database.QueryText(sqlStr);
            if (recordset.Records.Count > 0)
            {
                string jobno = recordset.Records[0].GetString("HRObjectID");
                //Log.Error("EHR同步修改用户：HRID" + jobno);
                IOrganizationalPerson user = RepositoryFactory.Instance.CreateRepository<IOrganizationalPerson>().Get(userID);
                if(user!= null)
                {
                    ZydcWebServiceImplService service = new ZydcWebServiceImplService();
                    service.pushInfoByJobno(jobno, user.AccountName, user.Email, userID);
                    //Log.Error("uum修改，回写HR系统数据：HRid：" + jobno + ", AccountName:" + user.AccountName + ",UserId:" + userID);
                }                
            }
        }
    }
}