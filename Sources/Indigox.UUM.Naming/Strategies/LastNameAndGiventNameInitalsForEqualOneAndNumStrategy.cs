using System;
using Indigox.UUM.Naming.Model;

namespace Indigox.UUM.Naming.Strategies
{
    /// <summary>
    /// 姓全拼，名长度等于1时，名全拼，否则名简写，加数字后缀,从02开始
    /// </summary>
    public class LastNameAndGiventNameInitalsForEqualOneAndNumStrategy : BaseNameStrategy, INameStrategy
    {
        private AccountNumSeq accountNumSeq = new AccountNumSeq();

        public string Naming(string name)
        {
            INameStrategy strategy = new LastNameAndGiventNameInitalsForEqualOneStrategy();
            string account = strategy.Naming(name);
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