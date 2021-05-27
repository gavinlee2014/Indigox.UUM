using System;
using Indigox.UUM.Naming.Util;

namespace Indigox.UUM.Naming.Strategies
{
    public abstract class BaseNameStrategy
    {
        protected string lastName;
        protected string givenName;

        protected void AnylazeName( string name )
        {
            int lastNameLength = SurnameManager.GetLastNameLength( name );

            lastName = name.Substring( 0, lastNameLength );
            givenName = name.Substring( lastNameLength );
        }

        public virtual bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}