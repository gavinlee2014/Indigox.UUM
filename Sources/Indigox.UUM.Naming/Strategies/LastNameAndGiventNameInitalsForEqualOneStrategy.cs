using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.UUM.Naming.Model;
using Indigox.UUM.Naming.Util;

namespace Indigox.UUM.Naming.Strategies
{
    /// <summary>
    /// 姓全拼，名长度大于1时，名简写，否则名全拼
    /// </summary>
    public class LastNameAndGiventNameInitalsForEqualOneStrategy : BaseNameStrategy, INameStrategy
    {
        public string Naming(string name)
        {
            AnylazeName(name);
            string lastNamePy = PinYinConverter.GetPinYin(lastName);
            string givenNamePy = PinYinConverter.GetPinYin(givenName);
            if (givenName.Length > 1)
            {
                givenNamePy = PinYinConverter.GetInitial(givenName);
            }
            return lastNamePy + givenNamePy;
        }
    }
}
