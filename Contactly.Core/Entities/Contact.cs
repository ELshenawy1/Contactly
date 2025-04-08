using Microsoft.EntityFrameworkCore;

namespace Contactly.Core.Entities
{
    public class Contact
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public ConnectionTracker ConnectionTracker { get; set; }
    }
}
