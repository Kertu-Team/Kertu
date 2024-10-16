using Microsoft.AspNetCore.Identity;

namespace Kertu.InteractiveServer.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public List<KertuElement> userKertuElements { get; set; } = new List<KertuElement>();
    }

}
