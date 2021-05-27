using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.Common.DomainModels.Factory;
using Indigox.UUM.NHibernateImpl.Model;
using Indigox.Common.StateContainer;
using Indigox.UUM.Sync.Mail;
using Indigox.Common.DomainModels.Queries;

namespace Indigox.UUM.Service
{
    public class OperationLogService
    {
        public static void LogOperation(string operation, string detailInformation)
        {
            string userName;
            try
            {
                userName = StateContext.Current.Session.User.Name;
            }
            catch (Exception e)
            {
                userName = "System";
            }
            OperationLog log = new OperationLog();
            log.Operator = userName;
            log.Operation = operation;
            log.OperationTime = DateTime.Now;
            log.DetailInformation = detailInformation;
            RepositoryFactory.Instance.CreateRepository<OperationLog>().Add(log);
            emailAdmin(log);
        }

        private static void emailAdmin(OperationLog log)
        {
            MailService mailService = new MailService();
            List<string> sysAdminEmails = new List<string>();
            var repository = RepositoryFactory.Instance.CreateRepository<Indigox.UUM.Sync.Model.SysConfiguration>();
            var list = repository.Find(new Query());
            foreach (var sysConfiguration in list)
            {
                if (sysConfiguration.Enabled == true && !string.IsNullOrEmpty(sysConfiguration.Email))
                {
                    sysAdminEmails.Add(sysConfiguration.Email);
                }
            }

            if (sysAdminEmails.Count > 0)
            {
                mailService.SendMail(sysAdminEmails, log.Operator + " " + log.Operation, log.DetailInformation.Replace("，", "<br>"));
            }
        }
    }
}
