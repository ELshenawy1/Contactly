namespace Contactly.Web.Models
{
    public class ContactSpecParams
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; }
        public string Search { get; set; } = "";
    }
}
