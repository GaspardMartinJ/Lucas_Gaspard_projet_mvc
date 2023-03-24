using Microsoft.AspNetCore.Identity;

namespace Lucas_Gaspard_projet_mvc.Data.Model
{
    public class Administator : IdentityRole
    {
        [PersonalData]
        public string Name { get; set; }
    }
}
