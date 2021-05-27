using System;
using System.Collections.Generic;

namespace Indigox.UUM.Naming.Strategies
{
    internal class AccountNumSeq
    {
        private Dictionary<string, int> seeks = new Dictionary<string, int>(250);

        /// <remarks>
        /// 获取 account 序列数字，从 1 开始
        /// </remarks>
        public int Get(string account)
        {
            if (!seeks.ContainsKey(account)) seeks[account] = 1;
            int seek = seeks[account];
            seeks[account]++;
            return seek;
        }

        /// <remarks>
        /// 在 account 后面添加两位数字，从 02 开始
        /// </remarks>
        public string GetSuffix(string account)
        {
            int seek = Get(account);
            string suffix;
            if (seek <= 1) suffix = "";
            else if (seek < 10) suffix = "0" + seek;
            else suffix = seek.ToString();
            return suffix;
        }
    }
}