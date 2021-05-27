using System;
using Indigox.UUM.Naming.Model;
using Indigox.UUM.Naming.Util;

namespace Indigox.UUM.Naming.Strategies
{
    /// <summary>
    /// 姓全拼，名简写，先名后姓
    /// </summary>
    public class GivenNameInitalsAndLastNameStrategy : BaseNameStrategy, INameStrategy
    {
        public string Naming( string name )
        {
            AnylazeName( name );
            string surnamePy = PinYinConverter.GetPinYin( lastName );
            string namePy = PinYinConverter.GetInitial( givenName );
            return namePy + surnamePy;
        }
    }
}