using System;
using System.Collections.Generic;
using System.Text;

namespace Indigox.UUM.Util
{
    public class SurnameFirstAndNameInitialAndNumSuffix : BaseNameStrategy, INameStrategy
    {
        private int seek = 1;

        public string ChineseName { get; set; }
        public List<string> CompundSurname { get; set; }

        public SurnameFirstAndNameInitialAndNumSuffix()
        {
            isRecyclable = true;
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
            return surnamePy + namePy+(seek++);
        }
    }
}
