using System;
using Indigox.UUM.Naming.Model;
using Indigox.UUM.Naming.Util;

namespace Indigox.UUM.Naming.Strategies
{
    /// <summary>
    /// 姓全拼，名全拼，先姓后名
    /// </summary>
    public class LastNameAndGivenNameStrategy : BaseNameStrategy, INameStrategy
    {
        public string Naming( string name )
        {
            AnylazeName( name );
            string surnamePy = PinYinConverter.GetPinYin( lastName );
            string namePy = PinYinConverter.GetPinYin( givenName );
            return surnamePy + namePy;
        }
    }
}