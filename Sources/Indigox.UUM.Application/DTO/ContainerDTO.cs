using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigox.UUM.Application.DTO
{
    public abstract class ContainerDTO : PrincipalDTO
    {
        public ContainerDTO()
        {
            this.Members = new List<SimplePrincipalDTO>();
        }

        public IList<SimplePrincipalDTO> Members { get; set; }


    }
}
