using System;
using Indigox.UUM.Naming.Model;
using Indigox.UUM.Naming.Util;

namespace Indigox.UUM.Naming.Strategies
{
    /// <summary>
    /// 姓全拼，名简写，先姓后名，加数字后缀
    /// </summary>
    public class LastNameAndGivenNameInitalsAndNumStrategy : BaseNameStrategy, INameStrategy
    {
        private AccountNumSeq accountNumSeq = new AccountNumSeq();

        public string Naming(string name)
        {
            AnylazeName(name);
            string surnamePy = PinYinConverter.GetPinYin(lastName);
            string namePy = PinYinConverter.GetInitial(givenName);
            string account = surnamePy + namePy;
            return account + accountNumSeq.GetSuffix(account);
        }

        public override bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}