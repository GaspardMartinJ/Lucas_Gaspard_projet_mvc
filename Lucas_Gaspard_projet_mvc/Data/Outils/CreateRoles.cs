using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Lucas_Gaspard_projet_mvc.Data.Outils
{
    public class CreateRoles
    {
        public void UpdateData(IServiceProvider serviceProvider)
        {
            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                ApplicationDbContext context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                RoleManager<IdentityRole> rl2 = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                string[] roles = new string[] { "Administrator", "Manager" };

                foreach (string role in roles)
                {
                    var roleStore = new RoleStore<IdentityRole>(context);
                    if (!context.Roles.Any(r => r.Name == role))
                    {
                        rl2.CreateAsync(new IdentityRole(role)).Wait();
                    }
                }
            }
        }
    }
}
