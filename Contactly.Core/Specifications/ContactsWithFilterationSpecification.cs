using Contactly.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contactly.Core.Specifications
{
    public class ContactsWithFilterationSpecification : BaseSpecification<Contact>
    {
        public ContactsWithFilterationSpecification(ContactSpecParams contactParams)
            : base(c =>
            (string.IsNullOrEmpty(contactParams.Search)
            || c.Name.ToLower().Contains(contactParams.Search.Trim())
            || c.Phone.Contains(contactParams.Search.Trim())
            || c.Address.ToLower().Contains(contactParams.Search.Trim()))
            )
        {
            ApplyPaging(contactParams.PageSize , contactParams.PageSize * (contactParams.PageIndex - 1));

        }
    }
}
