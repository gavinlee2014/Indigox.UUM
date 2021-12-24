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
using System.Globalization;
using Indigox.Settings.Service;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Indigox.UUM.Application.WebReference;
using ExchangeAddressList = Indigox.Common.ExchangeManager.AddressList;
using ExchangeGroup = Indigox.Common.ExchangeManager.Group;
using Indigox.Common.ExchangeManager;
using System.Collections.Generic;

namespace Indigox.UUM.Application.Sync.WebServices.HR
{
    [WebServiceBinding(Name = "ImportHRUserService", Namespace = Consts.Namespace_HR + "user/")]
    public class ImportHRUserService : IImportHRUserService
    {
        private bool EmployeeExists(string nativeID)
        {
            IRepository repository = RepositoryFactory.Instance.CreateRepository<HREmployee>();
            Query query = new Query();
            query.Specifications = Specification.Equal("ID", nativeID);
            var employee = repository.First(query);
            return employee != null;
        }

        private HREmployee UpdateEmployee(HREmployee employee, string organizationalUnitID, string accountName, string name, string fullName, string displayName, string idCard, string email, string title, string mobile, string telephone, string fax, double orderNum, string description, string otherContact, string portrait, string mailDatabase, HRPropertyChangeCollection extendProperties)
        {
            Log.Error("HR Update Employee with ID:" + employee.ID + ", name :" + name);

            employee.ParentID = organizationalUnitID;
            employee.Name = name;
            employee.IdCard = idCard;
            //employee.Email = email;
            employee.Title = title;
            employee.Mobile = mobile;
            employee.Tel = telephone;
            employee.Fax = fax;
            employee.Enabled = true;
            employee.EmployeeFlag = true;
            employee.State = HRState.Changed;
            employee.Synchronized = false;
            employee.ModifyTime = DateTime.Now;
            employee.HasPolyphone = PinYinConverter.HasPolyphone(name);
            if ((employee.QuitDate == null) || (employee.QuitDate.Ticks < DateTime.Parse("1753-01-01").Ticks) || (employee.QuitDate.Ticks > DateTime.Parse("9999-12-31").Ticks))
            {
                employee.QuitDate = DateTime.Parse("9999-12-31");
            }
            //employee.OrderNum = orderNum;
            employee.ExtendProperties = extendProperties == null ? new Dictionary<string, string>() : extendProperties.ToDictionary();

            if (!string.IsNullOrEmpty(portrait))
            {
                employee.Portrait = SaveProfile(employee.ID, portrait);
            }

            employee.Description = "修改用户" + employee.Name;

            return employee;
        }

        private void SyncToUUM(HREmployee employee)
        {
            var syncMode = new HRSyncMode();
            if (syncMode.IsAutomaticSync)
            {
                var employeeService = new HREmployeeService();
                employeeService.Sync(employee);
                IRepository<HREmployee> repository = RepositoryFactory.Instance.CreateRepository<HREmployee>();
                employee.Synchronized = true;
                repository.Update(employee);

            }
        }
        private void SyncEnableToUUM(HREmployee employee)
        {
            var syncMode = new HRSyncMode();
            if (syncMode.IsAutomaticSync)
            {
                var employeeService = new HREmployeeService();
                employeeService.Enable(employee);
                IRepository<HREmployee> repository = RepositoryFactory.Instance.CreateRepository<HREmployee>();
                employee.Synchronized = true;
                repository.Update(employee);

            }
        }
        private void SyncDisableToUUM(HREmployee employee)
        {
            var syncMode = new HRSyncMode();
            if (syncMode.IsAutomaticSync)
            {
                var employeeService = new HREmployeeService();
                employeeService.Disable(employee);
                IRepository<HREmployee> repository = RepositoryFactory.Instance.CreateRepository<HREmployee>();
                employee.Synchronized = true;
                repository.Update(employee);

            }
        }

        private HREmployee CreateEmployee(string nativeID, string organizationalUnitID, string accountName, string name, string fullName, string displayName, string idCard, string email, string title, string mobile, string telephone, string fax, double orderNum, string description, string otherContact, string portrait, string mailDatabase, HRPropertyChangeCollection extendProperties)
        {
            Log.Error("HR Create Employee with ID:" + nativeID + ", name :" + name);
            HREmployee employee = new HREmployee();
            var nameService = new NameService();
            var emailSuffix = EmailSettingService.Instance.GetSuffixByHRID(organizationalUnitID);
            accountName = nameService.Naming(name);
            email = string.Format("{0}@{1}", accountName, emailSuffix);

            employee.ID = nativeID;
            employee.ParentID = organizationalUnitID;
            employee.AccountName = accountName;
            employee.Name = name;
            employee.IdCard = idCard;
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
            if ((employee.QuitDate == null) || (employee.QuitDate.Ticks < DateTime.Parse("1753-01-01").Ticks) || (employee.QuitDate.Ticks > DateTime.Parse("9999-12-31").Ticks))
            {
                employee.QuitDate = DateTime.Parse("9999-12-31");
            }
            employee.OrderNum = orderNum;
            employee.ExtendProperties = extendProperties == null ? new Dictionary<string, string>() : extendProperties.ToDictionary();

            if (!string.IsNullOrEmpty(portrait))
            {
                employee.Portrait = SaveProfile(nativeID, portrait);
            }

            employee.Description = "新建用户" + employee.Name;

            return employee;
        }

        public string SyncUser(string nativeID, string organizationalUnitID, string accountName, string name, string fullName, string displayName, string idCard, string email, string title, string mobile, string telephone, string fax, double orderNum, string description, string otherContact, string portrait, string mailDatabase, HRPropertyChangeCollection extendProperties)
        {
            return Create(nativeID, organizationalUnitID, accountName, name, fullName, displayName, idCard, email, title, mobile, telephone, fax, orderNum, description, otherContact, portrait, mailDatabase, extendProperties);
        }

        public string Create(string nativeID, string organizationalUnitID, string accountName, string name, string fullName, string displayName, string idCard, string email, string title, string mobile, string telephone, string fax, double orderNum, string description, string otherContact, string portrait, string mailDatabase, HRPropertyChangeCollection extendProperties)
        {
            Log.Debug(String.Format("Message: HR Create Employee with ID:{0}-{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}", nativeID, organizationalUnitID, accountName, name, fullName, displayName, idCard, email, title, mobile, telephone, fax, orderNum, description, otherContact, mailDatabase));
            String extendString = "";
            if (extendProperties != null)
            {
                IDictionary<String, String> extend = extendProperties.ToDictionary();
                foreach (var key in extend.Keys)
                {
                    extendString += key + ":" + extend[key] + " ";
                }

            }
            Log.Debug(String.Format("Message: HR Create Employee with ID:{0} extend:{1}", nativeID, extendString));

            IRepository<HREmployee> repository = RepositoryFactory.Instance.CreateRepository<HREmployee>();
            if (!EmployeeExists(nativeID))
            {
                try
                {
                    var employee = CreateEmployee(nativeID, organizationalUnitID, accountName, name, fullName, displayName, idCard, email, title, mobile, telephone, fax, orderNum, description, otherContact, portrait, mailDatabase, extendProperties);
                    repository.Add(employee);
                    accountName = employee.AccountName;
                    email = employee.Email;
                    SyncToUUM(employee);
                }
                catch (Exception e)
                {
                    Log.Error(e.StackTrace);
                    throw new Exception("同步（创建）人员失败，编号:" + nativeID + ", error:" + e.Message);
                }

            }
            else
            {
                try
                {
                    HREmployee employee = repository.Get(nativeID);
                    UpdateEmployee(employee, organizationalUnitID, accountName, name, fullName, displayName, idCard, email, title, mobile, telephone, fax, orderNum, description, otherContact, portrait, mailDatabase, extendProperties);

                    Log.Debug(String.Format("Message: after Update Employee with ID:{0}-{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}", employee.ID, employee.ParentID, employee.AccountName, employee.Name, employee.DisplayName, employee.IdCard, employee.Email, employee.Title, employee.Mobile, employee.Tel, employee.Fax, employee.OrderNum, employee.MailDatabase, employee.QuitDate));
                    String extendString2 = "";
                    if (employee.ExtendProperties != null)
                    {
                        foreach (var key in employee.ExtendProperties.Keys)
                        {
                            extendString2 += key + ":" + employee.ExtendProperties[key] + " ";
                        }

                    }
                    Log.Debug(String.Format("Message: after Update Employee with ID:{0} extend:{1}", employee.ID, extendString2));

                    repository.Update(employee);
                    accountName = employee.AccountName;
                    email = employee.Email;
                    SyncToUUM(employee);
                }
                catch (Exception e)
                {
                    Log.Error(e.StackTrace);
                    throw new Exception("同步（修改）人员失败，编号:" + nativeID + ", error:" + e.Message);
                }

            }
            return email;

        }

        public void Delete(string userID)
        {
            IRepository repository = RepositoryFactory.Instance.CreateRepository<HREmployee>();
            Query query = new Query();
            query.Specifications = Specification.Equal("ID", userID);
            object EmployeeObj = repository.First(query);
            if (EmployeeObj != null)
            {
                HREmployee employee = EmployeeObj as HREmployee;
                if (!employee.Synchronized && employee.State == HRState.Created)
                {
                    repository.Remove(employee);
                }
                else
                {
                    employee.Description = "删除用户" + employee.Name;
                    employee.State = HRState.Deleted;
                    employee.Synchronized = false;
                    employee.ModifyTime = DateTime.Now;
                    repository.Update(employee);
                }

                SyncToUUM(employee);
            }
        }

        public void Disable(string userID, string quitDate)
        {
            Log.Error("HR同步禁用用户:" + userID);
            IRepository repository = RepositoryFactory.Instance.CreateRepository<HREmployee>();
            Query query = new Query();
            query.Specifications = Specification.Equal("ID", userID);
            object EmployeeObj = repository.First(query);
            
            if (EmployeeObj != null)
            {
                Log.Error("HR同步禁用用户:" + userID + " | 用户存在");
                HREmployee employee = EmployeeObj as HREmployee;
                if (!employee.Synchronized && employee.State == HRState.Created)
                {
                    employee.Enabled = false;
                }
                else
                {
                    employee.Description = "禁用用户" + employee.Name;
                    employee.Enabled = false;
                    employee.Synchronized = false;
                    employee.State = HRState.Disabled;
                    DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                    dtFormat.ShortDatePattern = "yyyy-mm-dd";
                    employee.QuitDate = Convert.ToDateTime(quitDate, dtFormat);
                    employee.ModifyTime = DateTime.Now;

                    //禁用用户时，增加离职日期属性
                    /**
                    IDatabase db = new DatabaseFactory().CreateDatabase("UUM");
                    IRecordSet rs = db.QueryText("SELECT quitdate FROM OpusOneEmployee where serialnumber = '" + userID + "'");
                    if (rs.Records.Count > 0)
                    {
                        if (rs.Records[0].GetValue("quitdate") != null)
                        {
                            employee.QuitDate = rs.Records[0].GetDateTime("quitdate");
                        }
                        else
                        {
                            Log.Error("The user(" + userID + ") does not have quitdate field.");
                        }
                    }
                    else
                    {
                        Log.Error("There is no user(" + userID+") exist in OpusOneEmployee.");
                    }
                    */
                }
                repository.Update(employee);

                Log.Error("HR同步禁用用户:" + userID + " | 禁用成功");
                SyncDisableToUUM(employee);
            }
        }


        public void Enable(string userID, string organizationalUnitID, string accountName, string name, string fullName, string displayName, string email, string title, string mobile, string telephone, string fax, double orderNum, string description, string otherContact, string portrait, string mailDatabase)
        {
            IRepository repository = RepositoryFactory.Instance.CreateRepository<HREmployee>();
            Query query = new Query();
            query.Specifications = Specification.Equal("ID", userID);
            object EmployeeObj = repository.First(query);
            if (EmployeeObj != null)
            {
                HREmployee employee = EmployeeObj as HREmployee;
                if (!employee.Synchronized && employee.State == HRState.Created)
                {
                    employee.Enabled = true;
                }
                else
                {
                    employee.Description = "启用用户" + employee.Name;
                    employee.Enabled = true;
                    employee.Synchronized = false;
                    employee.State = HRState.Enabled;
                    employee.ModifyTime = DateTime.Now;
                }
                repository.Update(employee);
                
                SyncEnableToUUM(employee);
            }
        }

        public void ChangeProperty(string userID, HRPropertyChangeCollection propertyChanges)
        {
            Log.Debug("Message: HR Update Employee with ID:" + userID );
            String extendString = "";
            if (propertyChanges != null)
            {
                IDictionary<String, String> extend = propertyChanges.ToDictionary();
                foreach (var key in extend.Keys)
                {
                    extendString += key + ":" + extend[key] + " ";
                }

            }
            Log.Debug(String.Format("Message: HR ChangeProperty Employee with ID:{0} extend:{1}", userID, extendString));

            IRepository repository = RepositoryFactory.Instance.CreateRepository<HREmployee>();
            Query query = new Query();
            query.Specifications = Specification.Equal("ID", userID);
            var employeeObj = repository.First(query);

            if (employeeObj != null)
            {
                Log.Error("修改用户:" + userID);
                string description = "修改用户属性：";
                HREmployee employee = employeeObj as HREmployee;
                foreach (var v in propertyChanges.PropertyChanges)
                {
                    string fieldName = v.Name;
                    if (fieldName != "Portrait")
                    {
                        description += fieldName + "：" + v.Value + " ，";
                    }
                    else
                    {
                        description += fieldName + "：  ，";
                    }
                    if (fieldName.Equals("DisplayName", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    switch (fieldName)
                    {
                        case "ParentID":
                            employee.ParentID = v.Value.ToString();
                            break;

                        case "Name":
                            employee.Name = v.Value.ToString();
                            //var nameService = new NameService();
                            //var emailSetting = new HREmployeeEmailSetting();
                            //employee.AccountName = nameService.Naming(employee.Name);
                            //employee.Email = string.Format("{0}@{1}", employee.AccountName, emailSetting.EmailSuffix);
                            //employee.HasPolyphone = PinYinConverter.HasPolyphone(employee.Name);
                            break;

                        case "Tel":
                            employee.Tel = Convert.ToString(v.Value);
                            break;

                        case "Fax":
                            employee.Fax = Convert.ToString(v.Value);
                            break;

                        case "Mobile":
                            employee.Mobile = Convert.ToString(v.Value);
                            break;

                        case "Title":
                            employee.Title = Convert.ToString(v.Value);
                            break;
                        case "IdCard":
                            employee.IdCard = Convert.ToString(v.Value);
                            break;
                        case "OrderNum":
                            employee.OrderNum = Convert.ToDouble(v.Value);
                            break;
                        /**
                        case "EmployeeFlag":
                            employee.EmployeeFlag = v.Value.ToString() == "1";
                            break;
                        */

                        case "Portrait":
                            string portrait = v.Value.ToString();
                            if (!string.IsNullOrEmpty(portrait))
                            {
                                employee.Portrait = SaveProfile(userID, portrait);
                            }
                            break;
                        /*
                         * 修改日期：2018-09-21
                         * 修改人：曾勇
                         * 修改內容：ChangeProperty时增加对QuitDate字段的响应
                         */
                        case "Quitdate":
                            employee.QuitDate = Convert.ToDateTime(v.Value);
                            break;
                        default:
                            if(employee.ExtendProperties.ContainsKey(fieldName))
                            {
                                employee.ExtendProperties[fieldName] = Convert.ToString(v.Value);
                            }
                            else
                            {
                                employee.ExtendProperties.Add(fieldName, Convert.ToString(v.Value));
                            }
                            break;
                    }
                }
                if (!(!employee.Synchronized && (employee.State == HRState.Created || employee.State == HRState.Disabled || employee.State == HRState.Enabled)))
                {
                    employee.State = HRState.Changed;
                    employee.Description = propertyChanges.PropertyChanges.Count > 0 ? description.TrimEnd('，') : description;
                }
                employee.Synchronized = false;
                employee.ModifyTime = DateTime.Now;
                repository.Update(employee);

                SyncToUUM(employee);
            }
        }

        private string SaveProfile(string id, string profile)
        {
            SettingService settingService = new SettingService();
            string path = settingService.GetValue("File.SavePath"); //+ "\\profile";
            //Create if not exist
            Directory.CreateDirectory(path);

            //string site = settingService.GetValue("File.FileSiteUrl") + settingService.GetValue("File.RootUrl") + "/profile/";
            //site = site.Replace("//", "/").Replace("http:/", "http://");
            ////Log.Error("profile is : " + profile);
            ////Log.Error("path is : " + path);
            ////Log.Error("site is : " + site);
            ////Log.Error("file name is : " + fileName);
            //byte[] bytes = new byte[profile.Length / 2];
            ////Log.Error("byte's length is : " + bytes.Length.ToString());
            //for (int i = 0; i < bytes.Length; i++)
            //{
            //    string strTemp = profile.Substring(i * 2, 2);
            //    bytes[i] = Convert.ToByte(strTemp, 16);

            //}
            string content = profile;
            ImageFormat format = ImageFormat.Jpeg;
            String extension = ".jpg";
            if (profile.IndexOf(",") > 0)
            {
                String[] strings = profile.Split(',');
                switch (strings[0])
                {//check image's extension
                    case "data:image/jpeg;base64":
                        extension = ".jpeg";
                        format = ImageFormat.Jpeg;
                        break;
                    case "data:image/png;base64":
                        extension = ".png";
                        format = ImageFormat.Png;
                        break;
                    case "data:image/gif;base64":
                        extension = ".gif";
                        format = ImageFormat.Gif;
                        break;
                    case "data:image/tiff;base64":
                        extension = ".tiff";
                        format = ImageFormat.Tiff;
                        break;
                    case "data:image/bmp;base64":
                        extension = ".bmp";
                        format = ImageFormat.Bmp;
                        break;
                    case "data:image/jpg;base64":
                    default:
                        extension = ".jpg";
                        format = ImageFormat.Jpeg;
                        break;
                }
                content = strings[1];
            }
            string fileName =  Convert.ToInt32(id.Trim()) + "_" + DateTime.Now.Millisecond.ToString() + extension;

            byte[] bytes = Convert.FromBase64String(content);

            Image img;
            using (MemoryStream ms = new MemoryStream(bytes))//建立内存的流  
            {
                //img = Image.FromStream(ms);//把内存的流转换成图片格式
                //img.Save(path + "\\" + fileName, ImageFormat.Jpeg);// 保存图片
                using (img = Image.FromStream(ms))
                {
                    img.Save(path + "\\" + fileName, format);
                }
            }
            

            //string url = site + fileName;
            //return url;

            return fileName;
        }

        public void TestSync(int arg1, int arg2)
        {
            moveAddressList(arg1, arg2);
        }

        private void createOrg(int arg1, int arg2)
        {
            int iCount = 0;
            IDatabase database = new DatabaseFactory().CreateDatabase("UUM");
            IRecordSet recordset = database.QueryText("select ID from t_org_tmp where stat = 0 and cj = " + arg2.ToString());
            for (int i = 0; i < recordset.Records.Count; i++)
            {
                string id = recordset.Records[i].GetString("ID");
                HROrganizationalService service = new HROrganizationalService();
                //Log.Error("创建部门：" + id);
                service.Sync(id);
                string sql = "update t_org_tmp set stat = 1 where id ='" + id + "'";
                database.ExecuteText(sql);

                iCount++;
                if (iCount >= arg1)
                {
                    break;
                }
            }
        }

        private void syncOrgChange(int arg1, int arg2)
        {
            int iCount = 0;
            IDatabase database = new DatabaseFactory().CreateDatabase("UUM");
            IRecordSet recordset = database.QueryText("select ID from t_org_change where stat = 0");
            for (int i = 0; i < recordset.Records.Count; i++)
            {
                string id = recordset.Records[i].GetString("ID");
                HROrganizationalService service = new HROrganizationalService();
                //Log.Error("修改部门：" + id );
                service.Sync(id);
                string sql = "update t_org_change set stat = 1 where id ='" + id + "'";
                database.ExecuteText(sql);


                iCount++;
                if (iCount >= arg1)
                {
                    break;
                }
            }
        }

        private void syncUserChange(int arg1, int arg2)
        {
            int iCount = 0;
            IDatabase database = new DatabaseFactory().CreateDatabase("UUM");
            IRecordSet recordset = database.QueryText("select ID from t_org_change where stat = 0");
            for (int i = 0; i < recordset.Records.Count; i++)
            {
                string id = recordset.Records[i].GetString("ID");
                HREmployeeService service = new HREmployeeService();
                //Log.Error("修改人员：" + id);
                service.Sync(id);
                string sql = "update t_org_change set stat = 1 where id ='" + id + "'";
                database.ExecuteText(sql);


                iCount++;
                if (iCount >= arg1)
                {
                    break;
                }
            }
        }

        private void delORG(int arg1, int arg2)
        {
            int iCount = 0;
            IDatabase database = new DatabaseFactory().CreateDatabase("UUM");
            IRecordSet recordset = database.QueryText("select id from t_del_org where stat = 0");
            for (int i = 0; i < recordset.Records.Count; i++)
            {
                string id = recordset.Records[i].GetString("ID");
                HROrganizationalService service = new HROrganizationalService();
                //Log.Error("删除部门：" + id);
                service.Delete(id);
                string sql = "update t_del_org set stat = 1 where id ='" + id + "'";
                database.ExecuteText(sql);


                iCount++;
                if (iCount >= arg1)
                {
                    break;
                }
            }
        }

        private void updateHR(int arg1, int arg2)
        {            
            int iCount = 0;
            IDatabase database = new DatabaseFactory().CreateDatabase("UUM");
            IRecordSet recordset = database.QueryText("select hrid, ID, AccountName,Email from t_hr_map where stat = 0");
            for (int i = 0; i < recordset.Records.Count; i++)
            {
                string hrid = recordset.Records[i].GetString("hrid");
                string id = recordset.Records[i].GetString("ID");
                string accountName = recordset.Records[i].GetString("AccountName");
                string email = recordset.Records[i].GetString("Email");
                //Log.Error("同步HR：" + hrid + " | " + id);
                ZydcWebServiceImplService service = new ZydcWebServiceImplService();
                service.pushInfoByJobno(hrid, accountName, email, id);
                string sql = "update t_hr_map set stat = 1 where id ='" + id + "'";
                database.ExecuteText(sql);


                iCount++;
                if (iCount >= arg1)
                {
                    break;
                }
            }
        }
        
        private void updateAddressList(int arg1, int arg2)
        {
            int iCount = 0;

            ExchangeManagerService service = new ExchangeManagerService();
            IDatabase database = new DatabaseFactory().CreateDatabase("UUM");
            IRecordSet recordset = database.QueryText("select id,pid,name from t_org_mail where stat = 0 order by pid");
            for (int i = 0; i < recordset.Records.Count; i++)
            {
                string id = recordset.Records[i].GetString("id");
                string pid = recordset.Records[i].GetString("pid");
                string name = recordset.Records[i].GetString("name");
                ExchangeAddressList parentAddressList = getParentAddressList(service, pid);
                //Log.Error("获取AddressList, id = " + id + " pid = " + pid);
                if(parentAddressList != null)
                {
                    ExchangeAddressList addressList = service.GetAddressList(name, parentAddressList);
                    string sql = "update t_org_mail set stat = 1, addrid = '" + addressList.ID +"', displayname='" + addressList.DisplayName + "'  where id ='" + id + "'";
                    database.ExecuteText(sql);
                }

                iCount++;
                if (iCount >= arg1)
                {
                    break;
                }
            }
        }

        private void moveAddressList(int arg1,int arg2)
        {
            int iCount = 0;

            ExchangeManagerService service = new ExchangeManagerService();
            IDatabase database = new DatabaseFactory().CreateDatabase("UUM");
            IRecordSet recordset = database.QueryText("select eid,epid from t_ex_tmp where stat = 0 order by id");
            for (int i = 0; i < recordset.Records.Count; i++)
            {
                string id = recordset.Records[i].GetString("eid");
                string pid = recordset.Records[i].GetString("epid");
                Log.Error("MoveAddressList :" + id + "|" + pid);

                ExchangeAddressList addressList = service.GetAddressList(id);
                ExchangeAddressList paddressList = service.GetAddressList(pid);
                service.MoveAddressList(addressList, paddressList);

                string sql = "update t_ex_tmp set stat = 1 where eid ='" + id + "'";
                database.ExecuteText(sql);

                iCount++;
                if (iCount >= arg1)
                {
                    break;
                }
            }

        }

        private ExchangeAddressList getParentAddressList(ExchangeManagerService service, string pid)
        {
            ExchangeAddressList parent = null;
            IDatabase database = new DatabaseFactory().CreateDatabase("UUM");
            string sql = "select addrid  from t_org_mail where id = '" + pid + "'";
            IRecordSet recordset = database.QueryText(sql);
            if (recordset.Records.Count > 0)
            {
                string addrid = recordset.Records[0].GetString("addrid");
                if (!string.IsNullOrEmpty(addrid))
                {
                    parent = service.GetAddressList(addrid);
                }
            }

            return parent;
        }

        public void Disable(string userID)
        {
            Disable(userID, DateTime.Now.ToString("yyyy-MM-dd"));
        }
    }
}