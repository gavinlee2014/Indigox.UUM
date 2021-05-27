using System;
using System.Collections.Generic;
using System.Text;

namespace Indigox.UUM.Util
{
    public abstract class BaseNameStrategy
    {        
        protected string surname;
        protected string name;
        protected bool isRecyclable;

        protected void AnylazeName(string chineseName,List<string> compoundSurname)
        {
            bool isCompund = false;
            foreach (var v in compoundSurname)
            {
                if (chineseName.StartsWith(v))
                {
                    surname = v;
                    name = chineseName.Substring(v.Length, chineseName.Length - v.Length);
                    isCompund = true;
                    break;
                }
            }
            if (!isCompund)
            {
                surname = chineseName.Substring(0, 1);
                name = chineseName.Substring(1, chineseName.Length - 1);
            }
        }
        public bool GetRecyclable()
        {
            return isRecyclable;
        }
    }
}
