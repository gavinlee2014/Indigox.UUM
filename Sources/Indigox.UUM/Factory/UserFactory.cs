using System;
using Indigox.Common.Membership.Interfaces;

namespace Indigox.UUM.Factory
{
    public class UserFactory
    {
        private static readonly UserFactory instance = new UserFactory();

        public static UserFactory Instance
        {
            get { return instance; }
        }

        private UserFactory()
        {
        }

        public IMutableUser Create()
        {
            throw new NotImplementedException();
        }
    }
}