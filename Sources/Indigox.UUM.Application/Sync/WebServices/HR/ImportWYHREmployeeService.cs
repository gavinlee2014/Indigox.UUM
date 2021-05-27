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
using Indigox.UUM.Naming.Util;
using Indigox.Common.Data;
using Indigox.Common.Data.Interface;
using Indigox.Common.Logging;

namespace Indigox.UUM.Application.Sync.WebServices.HR
{
    [WebServiceBinding( Name = "ImportUserService", Namespace = Consts.Namespace_HR )]
    public class ImportWYHREmployeeService : WebService, IImportWYUserService
    {
        private bool EmployeeExists( string nativeID )
        {            
            IRepository repository = RepositoryFactory.Instance.CreateRepository<HREmployee>();
            Query query = new Query();
            query.Specifications = Specification.Equal( "ID", nativeID );
            var employee = repository.First( query );
            return employee != null;
        }

        private string GetRealOrgID(string orgID)
        {
            string realOrgID = orgID;
            IDatabase database = new DatabaseFactory().CreateDatabase("UUM");
            string sql = "select name from HROrganizational where id = '" + orgID + "'";
            IRecordSet recordset = database.QueryText(sql);
            string orgName = "";
            if(recordset.Records.Count > 0)
            {
                orgName = recordset.Records[0].GetString("name");
            }

            if("安管部,工程部,环境部,甲方挂靠,客服部,综合部".IndexOf(orgName) != -1 && orgName != "")
            {
                realOrgID = orgID.Substring(0, orgID.Length - 2);
            }

            return realOrgID;
        }

        private void SyncToUUM( HREmployee employee )
        {
            var syncMode = new HRSyncMode();
            if ( syncMode.IsAutomaticSync )
            {
                var employeeService = new HREmployeeService();
                employeeService.Sync( employee );
            }
        }

        private HREmployee CreateEmployee(string nativeID, string organizationalUnitID, string name, string title, string mobile, string telephone, string fax, string portrait)
        {
            HREmployee employee = new HREmployee();
            var nameService = new NameService();
            var emailSuffix = EmailSettingService.Instance.GetSuffix("");
            string accountName = nameService.Naming( name );
            string email = string.Format( "{0}@{1}", accountName, emailSuffix );

            employee.ID = nativeID;
            employee.ParentID = GetRealOrgID(organizationalUnitID);
            employee.AccountName = accountName;
            employee.Name = name;
            employee.Email = email;
            employee.Title = title;
            employee.Mobile = mobile;
            employee.Tel = telephone;
            employee.Fax = fax;
            employee.Enabled = true;
            employee.EmployeeFlag = true;
            employee.State = HRState.Created;
            employee.Synchronized = false;
            employee.ModifyTime = DateTime.Now;
            employee.HasPolyphone = PinYinConverter.HasPolyphone(name);
            //"AM-WuyeDB02"
            employee.MailDatabase = "AM-WuyeDB0" + new Random().Next(1, 3);

            //string prefix = new HREmployeeProfilePrefixSetting().ProfilePrefix;
            //prefix = prefix.EndsWith("/") ? prefix : prefix + "/";
            //employee.Portrait = prefix + portrait;

            employee.Description = "新建用户" + employee.Name;

            return employee;
        }

        public string SyncUser(string nativeID, string organizationalUnitID, string name, string title, string mobile, string telephone, string fax, string portrait)
        {
            if ( !EmployeeExists( nativeID ) )
            {
                IRepository repository = RepositoryFactory.Instance.CreateRepository<HREmployee>();
                var employee = CreateEmployee(nativeID, organizationalUnitID, name, title, mobile, telephone, fax, portrait);
                repository.Add( employee );

                SyncToUUM( employee );
            }
            return nativeID;
        }

        public string Create(string nativeID, string organizationalUnitID, string name, string title, string mobile, string telephone, string fax, string portrait)
        {
            if (EmployeeExists(nativeID))
            {
                throw new Exception("已存在ID为：" + nativeID + "的用户，创建失败");
            }

            IRepository repository = RepositoryFactory.Instance.CreateRepository<HREmployee>();
            var employee = CreateEmployee(nativeID, organizationalUnitID, name, title, mobile, telephone, fax, portrait);
            repository.Add( employee );

            SyncToUUM( employee );
            return nativeID;
        }

        public void Delete( string userID )
        {
            IRepository repository = RepositoryFactory.Instance.CreateRepository<HREmployee>();
            Query query = new Query();
            query.Specifications = Specification.Equal( "ID", userID );
            object EmployeeObj = repository.First( query );
            if ( EmployeeObj != null )
            {
                HREmployee employee = EmployeeObj as HREmployee;
                if ( !employee.Synchronized && employee.State == HRState.Created )
                {
                    repository.Remove( employee );
                }
                else
                {
                    employee.Description = "删除用户" + employee.Name;
                    employee.State = HRState.Deleted;
                    employee.Synchronized = false;
                    employee.ModifyTime = DateTime.Now;
                    repository.Update( employee );
                }

                SyncToUUM( employee );
            }
        }

        public void Disable( string userID , DateTime quitDate)
        {
            IRepository repository = RepositoryFactory.Instance.CreateRepository<HREmployee>();
            Query query = new Query();
            query.Specifications = Specification.Equal( "ID", userID );
            object EmployeeObj = repository.First( query );
            if ( EmployeeObj != null )
            {
                HREmployee employee = EmployeeObj as HREmployee;
                if(employee.State == HRState.Undefined)
                {
                    employee.Description = "禁用用户" + employee.Name;
                    employee.Enabled = false;
                    employee.Synchronized = false;
                    employee.ModifyTime = DateTime.Now;
                    employee.QuitDate = quitDate;
                }
                else if ( !employee.Synchronized && employee.State == HRState.Created )
                {
                    employee.Enabled = false;
                }
                else
                {
                    employee.Description = "禁用用户"+employee.Name;
                    employee.Enabled = false;
                    employee.Synchronized = false;
                    employee.State = HRState.Disabled;
                    employee.ModifyTime = DateTime.Now;
                    employee.QuitDate = quitDate;
                }
                repository.Update( employee );

                SyncToUUM( employee );
            }
        }
                

        public void Enable(string userID)
        {
            IRepository repository = RepositoryFactory.Instance.CreateRepository<HREmployee>();
            Query query = new Query();
            query.Specifications = Specification.Equal( "ID", userID );
            object EmployeeObj = repository.First( query );
            if ( EmployeeObj != null )
            {
                HREmployee employee = EmployeeObj as HREmployee;
                if ( !employee.Synchronized && employee.State == HRState.Created )
                {
                    employee.Enabled = true;
                }
                else
                {
                    employee.Description = "启用用户"+employee.Name;
                    employee.Enabled = true;
                    employee.Synchronized = false;
                    employee.State = HRState.Enabled;
                    employee.ModifyTime = DateTime.Now;
                }
                repository.Update( employee );

                SyncToUUM( employee );
            }
        }

        public void ChangeProperty( string userID, string organizationalUnitID, string name, string title, string mobile, string telephone, string fax, string portrait)
        {
            IRepository repository = RepositoryFactory.Instance.CreateRepository<HREmployee>();
            Query query = new Query();
            query.Specifications = Specification.Equal( "ID", userID );
            var employeeObj = repository.First( query );
            string realOrgID = GetRealOrgID(organizationalUnitID);
            
            if ( employeeObj != null )
            {
                string description = "修改用户属性：";
                HREmployee employee = employeeObj as HREmployee;

                if(employee.ParentID != realOrgID)
                {
                    employee.ParentID = realOrgID;
                    description += "ParentID : " + realOrgID + ",";
                }
                if(employee.Name != name)
                {
                    employee.Name = name;
                    employee.HasPolyphone = PinYinConverter.HasPolyphone(employee.Name);
                    description += "Name : " + name + ",";
                }
                if(employee.Tel != telephone)
                {
                    employee.Tel = telephone;
                    description += "Tel : " + telephone + ",";
                }
                if(employee.Fax != fax)
                {
                    employee.Fax = fax;
                    description += "Fax : " + fax + ",";
                }
                if (employee.Mobile != mobile)
                {
                    employee.Mobile = mobile;
                    description += "Mobile : " + mobile + ",";
                }
                if(employee.Title != title)
                {
                    employee.Title = title;
                    description += "Title : " + title + ",";
                }
                

                if ( !( !employee.Synchronized && (employee.State == HRState.Created||employee.State==HRState.Disabled||employee.State==HRState.Enabled)))
                {
                    if (employee.State != HRState.Undefined)
                    {
                        employee.State = HRState.Changed;
                    }                    
                    employee.Description = description.TrimEnd(',') ;
                }
                employee.Synchronized = false;
                employee.ModifyTime = DateTime.Now;
                repository.Update( employee );

                SyncToUUM( employee );
            }
        }        
    }
}