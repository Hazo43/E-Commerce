using System.ComponentModel.DataAnnotations;

namespace AdminDashboard.Models.Roles
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = " Role Name Is Required")]
        [StringLength( 256 , ErrorMessage = " Role Name Size Can't Be More Than 256 Char. ")]
        public string Name { get; set; } = string.Empty;
    }
}
