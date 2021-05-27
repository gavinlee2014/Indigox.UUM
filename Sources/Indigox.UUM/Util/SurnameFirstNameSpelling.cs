using System;
using System.Collections.Generic;
using System.Text;

namespace Indigox.UUM.Util
{
    public class SurnameFirstNameSpelling : BaseNameStrategy, INameStrategy
    {
        public string ChineseName { get; set; }
        public List<string> CompundSurname { get; set; }

        public SurnameFirstNameSpelling()
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
            string namePy = PinYinConverter.GetPinYin(name);
            return surnamePy+namePy;
        }

    }
}
