using static CUPOS_FRONT.Models.Requests;

namespace Web.Models
{
    public class rolesList
    {
        public List<ReqRoles> rolsList { get; set; }
    }

    public class Roles
    {
        public int rolId { get; set; }
        public string name { get; set; }
        public string position { get; set; }
        public string description { get; set; }
        public bool estate { get; set; }
    }
}
