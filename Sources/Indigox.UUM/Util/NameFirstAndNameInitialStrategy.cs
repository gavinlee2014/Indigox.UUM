using System;
using System.Collections.Generic;
using System.Text;

namespace Indigox.UUM.Util
{
    public class NameFirstAndNameInitialStrategy : BaseNameStrategy, INameStrategy
    {
        public string ChineseName { get; set; }
        public List<string> CompundSurname { get; set; }

        public NameFirstAndNameInitialStrategy()
        {
            isRecyclable = false;
        }
        public string Naming()
        {
            if (string.IsNullOrEmpty(ChineseName) || CompundSurname == null)
            {
                throw new ArgumentNullException("ChineseName, CompundSurname不能为空！");
            }
            AnylazeName(ChineseName, CompundSurname);
            string surnamePy = PinYinConverter.GetPinYin(surname);
            string namePy = PinYinConverter.GetInitial(name);
            return namePy+surnamePy;
        }
    }
}
