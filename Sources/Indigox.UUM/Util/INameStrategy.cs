using System;
using System.Collections.Generic;
using System.Text;

namespace Indigox.UUM.Util
{
    public interface INameStrategy
    {
        string ChineseName { get; set; }
        List<string> CompundSurname { get; set; }

        bool GetRecyclable();
        string Naming();
    }
}
