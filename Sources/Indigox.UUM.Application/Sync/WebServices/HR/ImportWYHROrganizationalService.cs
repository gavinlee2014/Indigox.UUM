using System;
using System.Collections.Generic;
using System.Web.Services;
using Indigox.Common.Data;
using Indigox.Common.Data.Interface;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.DomainModels.Specifications;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Application.DTO;
using Indigox.UUM.Application.OrganizationalPerson;
using Indigox.UUM.Application.SyncTask;
using Indigox.UUM.Factory;
using Indigox.UUM.HR.Model;
using Indigox.UUM.HR.Service;
using Indigox.UUM.HR.Setting;
using Indigox.UUM.Service;
using Indigox.UUM.Sync.Interface;
using Indigox.UUM.Sync.Interface.Client;

namespace Indigox.UUM.Application.Sync.WebServices.HR
{
    [WebServiceBinding(Name = "ImportOrganizationalUnitService", Namespace = Consts.Namespace_HR)]
    public class ImportWYHROrganizationalService : WebService, IImportWYOrganizationalUnitService
    {
        private bool OrganizationalExists(string nativeID)
        {
            IRepository repository = RepositoryFactory.Instance.CreateRepository<HROrganizational>();
            Query query = new Query();
            query.Specifications = Specification.Equal("ID", nativeID);
            var employee = repository.First(query);
            return employee != null;
        }

        private void SyncToUUM(HROrganizational organizational)
        {
            var syncMode = new HRSyncMode();
            if (syncMode.IsAutomaticSync)
            {
                var employeeService = new HROrganizationalService();
                employeeService.Sync(organizational);
            }
        }

        private HROrganizational CreateOrganizational(string nativeID, string parentOrganizationalUnitID, string name)
        {
            string organizationalUnitType = "";
            if (nativeID.Length <= 2)
            {
                organizationalUnitType = "Corporation";
            }
            else if (nativeID.Length == 4)
            {
                organizationalUnitType = "Company";
            }
            else if (nativeID.Length == 6 || nativeID.Length == 8)
            {
                organizationalUnitType = "Department";
            }
            else if (nativeID.Length == 10 || nativeID.Length == 12)
            {
                organizationalUnitType = "Section";
            }

            var org = new HROrganizational();
            org.ID = nativeID;
            org.ParentID = parentOrganizationalUnitID;
            org.Name = name;
            org.Type = organizationalUnitType;
            org.Synchronized = false;
            org.ModifyTime = DateTime.Now;
            org.State = HRState.Created;
            org.Description = "新建部门: " + name;
            return org;
        }

        public string SyncOrganizationalUnit(string nativeID, string parentOrganizationalUnitID, string name)
        {
            if (!OrganizationalExists(nativeID))
            {
                IRepository repository = RepositoryFactory.Instance.CreateRepository<HROrganizational>();
                var org = CreateOrganizational(nativeID, parentOrganizationalUnitID, name);
                repository.Add(org);

                SyncToUUM(org);
            }
            return nativeID;
        }


        public string Create(string nativeID, string parentOrganizationalUnitID, string name)
        {
            if (OrganizationalExists(nativeID))
            {
                throw new Exception("已存在编号为：" + nativeID + "的部门，不能重复新建!");
            }

            IRepository repository = RepositoryFactory.Instance.CreateRepository<HROrganizational>();
            var organization = CreateOrganizational(nativeID, parentOrganizationalUnitID, name);
            repository.Add(organization);

            SyncToUUM(organization);
            return nativeID;
        }

        public void Delete(string organizationalUnitID)
        {
            IRepository repository = RepositoryFactory.Instance.CreateRepository<HROrganizational>();
            Query query = new Query();
            query.Specifications = Specification.Equal("ID", organizationalUnitID);
            object orgObj = repository.First(query);
            if (orgObj != null)
            {
                HROrganizational org = orgObj as HROrganizational;
                if (!org.Synchronized && org.State == HRState.Created)
                {
                    repository.Remove(org);
                }
                else
                {
                    org.Description = "删除部门:" + org.Name;
                    org.State = HRState.Deleted;
                    org.Synchronized = false;
                    org.ModifyTime = DateTime.Now;
                    repository.Update(org);
                }

                SyncToUUM(org);
            }
        }

        public void ChangeProperty(string organizationalUnitID, string parentOrganizationalUnitID, string name)
        {
            IRepository repository = RepositoryFactory.Instance.CreateRepository<HROrganizational>();
            Query query = new Query();
            query.Specifications = Specification.Equal("ID", organizationalUnitID);
            var oorgObj = repository.First(query);
            if (oorgObj != null)
            {
                string description = "修改部门属性：";
                HROrganizational org = oorgObj as HROrganizational;

                if (org.ParentID != parentOrganizationalUnitID)
                {
                    org.ParentID = parentOrganizationalUnitID;
                    description += "父部门：" + parentOrganizationalUnitID + " ,";
                }
                if (org.Name != name)
                {
                    org.Name = name;
                    description += "名称：" + name + " ,";
                }

                if (!(!org.Synchronized && org.State == HRState.Created))
                {
                    org.State = HRState.Changed;
                    org.Description = description.TrimEnd(',');
                }
                org.Synchronized = false;
                org.ModifyTime = DateTime.Now;
                repository.Update(org);

                SyncToUUM(org);
            }
        }

        public void Synchronization(int orgLevel, int syncNumber)
        {

            int iCount = 0;

            IRepository<IOrganizationalPerson> repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalPerson>();
            PrincipalService service = new PrincipalService();

            IDatabase database = new DatabaseFactory().CreateDatabase("UUM");
            IRecordSet recordset = database.QueryText("select name,displayname,accountname,email,mobile,organization,title from t_new_user where stat = 0");
            for (int i = 0; i < recordset.Records.Count; i++)
            {
                string name = recordset.Records[i].GetString("name");
                string displayName = recordset.Records[i].GetString("displayname");
                string accountName = recordset.Records[i].GetString("accountname");
                string email = recordset.Records[i].GetString("email");
                string mobile = recordset.Records[i].GetString("mobile");
                string organization = recordset.Records[i].GetString("organization");
                string title = recordset.Records[i].GetString("title");


                CreateOrganizationalPersonCommand cmd = new CreateOrganizationalPersonCommand();
                cmd.ID = new OrganizationalPersonFactory().GetNextID();
                cmd.Name = name;
                cmd.DisplayName = displayName;
                cmd.AccountName = accountName;
                cmd.Email = email;
                cmd.Mobile = mobile;
                cmd.Organization = organization;
                cmd.OrderNum = 1.001;
                cmd.MailDatabase = "AM-WuyeDB01";
                cmd.Telephone = mobile;
                cmd.Fax = "";
                cmd.MemberOfGroups = new List<SimplePrincipalDTO>();
                cmd.MemberOfOrganizationalRoles = new List<SimplePrincipalDTO>();
                cmd.Profile = new List<ProfileDTO>();
                cmd.OtherContact = "";
                cmd.Title = title;

                cmd.Execute();

                string sql = "update t_new_user set stat = 1 where email ='" + email + "'";
                database.ExecuteText(sql);

                iCount++;
                if (iCount >= syncNumber)
                {
                    break;
                }
            }

        }

        private void Test3(int orgLevel, int syncNumber)
        {
            int iCount = 0;

            IRepository<IOrganizationalPerson> repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalPerson>();

            IDatabase database = new DatabaseFactory().CreateDatabase("UUM");
            IRecordSet recordset = database.QueryText("select organization from t_new_user where stat = 0");
            for (int i = 0; i < recordset.Records.Count; i++)
            {
                string id = recordset.Records[i].GetString("organization");
                IOrganizationalPerson item = repository.Get(id);
                PrincipalService service = new PrincipalService();
                service.Disable(item);

                string sql = "update t_new_user set stat = 1 where organization ='" + id + "'";
                database.ExecuteText(sql);

                iCount++;
                if (iCount >= syncNumber)
                {
                    break;
                }

            }

        }



        private void Test2(int orgLevel, int syncNumber)
        {
            int iCount = 0;

            IRepository<IOrganizationalPerson> repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalPerson>();
            PrincipalService service = new PrincipalService();

            IDatabase database = new DatabaseFactory().CreateDatabase("UUM");
            IRecordSet recordset = database.QueryText("select id from t_disabled_user where stat = 0");
            for (int i = 0; i < recordset.Records.Count; i++)
            {
                string id = recordset.Records[i].GetString("id");
                IOrganizationalPerson item = repository.Get(id);
                service.Disable(item);

                string sql = "update t_disabled_user set stat = 1 where id ='" + id + "'";
                database.ExecuteText(sql);

                iCount++;
                if (iCount >= syncNumber)
                {
                    break;
                }
            }
        }

        private void Test1(int orgLevel, int syncNumber)
        {
            int iCount = 0;
            IDatabase database = new DatabaseFactory().CreateDatabase("UUM");
            IRecordSet recordset = database.QueryText("select e.ID from HREmployee e, HROrganizational o where e.State = 1 and e.Synchronized = 0 and o.ID = e.ParentID  and e.ParentID not in ('0202','0203')");
            for (int i = 0; i < recordset.Records.Count; i++)
            {
                string employeeId = recordset.Records[i].GetString("ID");
                HREmployeeService service = new HREmployeeService();
                service.Sync(employeeId, null);

                iCount++;
                if (iCount >= syncNumber)
                {
                    break;
                }
            }
        }
        private void Test(int orgLevel, int syncNumber)
        {
            /**
             * 
            int iCount = 0;
            PrincipalService service = new PrincipalService();
            IRepository<IOrganizationalUnit> repository = RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>();
            IDatabase database = new DatabaseFactory().CreateDatabase("UUM");
            IRecordSet recordset = database.QueryText("select id,realparentid from t_real_org where stat = 0");
            for (int i = 0; i < recordset.Records.Count; i++)
            {
                string id = recordset.Records[i].GetString("id");
                string praentid = recordset.Records[i].GetString("realparentid");

                IOrganizationalObject item = Indigox.Common.Membership.Principal.GetPrincipalByID(id) as IOrganizationalObject;
                IOrganizationalUnit target = repository.Get(praentid);
                service.MoveTo(item, target);
                string sql = "update t_real_org set stat = 1 where id ='" + id + "'";
                database.ExecuteText(sql);

                iCount++;
                if (iCount >= syncNumber)
                {
                    break;
                }
            }   
             */

            int iCount = 0;
            IDatabase database = new DatabaseFactory().CreateDatabase("UUM");
            IRecordSet recordset = database.QueryText(
                string.Format(@"select ID from HROrganizational where LEN(id) = {0} and State = 0 and Synchronized = 0 and Email is not null and Name not in ('安管部','工程部','环境部','甲方挂靠','客服部','综合部');
", orgLevel));
            for (int i = 0; i < recordset.Records.Count; i++)
            {
                string orgID = recordset.Records[i].GetString("ID");
                HROrganizationalService service = new HROrganizationalService();
                service.Sync(orgID, null);
                iCount++;
                if (iCount >= syncNumber)
                {
                    break;
                }
            }
        }

        public void SyncError()
        {
            IDatabase database = new DatabaseFactory().CreateDatabase("UUM");
            IRecordSet recordset = database.QueryText("select ID From synctask where state = 2 and Tag = 'Exchange'");

            if (recordset.Records.Count > 0)
            {
                int id = recordset.Records[0].GetInt("ID");
                RetrySyncTaskCommand command = new RetrySyncTaskCommand();
                command.ID = id;
                command.Execute();
            }           
        }
    }
}