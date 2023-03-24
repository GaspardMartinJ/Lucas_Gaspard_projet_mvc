using Microsoft.AspNetCore.Identity;

namespace Lucas_Gaspard_projet_mvc.Data.Model
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string? Type { get; set; }
    }
}
