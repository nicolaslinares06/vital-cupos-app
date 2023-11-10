using static CUPOS_FRONT.Models.Requests;

namespace Web.Models
{
    public class RolesList
    {
        public List<ReqRoles> rolsList { get; set; } = new List<ReqRoles>();
    }

    public class Roles
    {
        public int rolId { get; set; }
        public string name { get; set; } = string.Empty;
        public string position { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public bool estate { get; set; }
    }
}
