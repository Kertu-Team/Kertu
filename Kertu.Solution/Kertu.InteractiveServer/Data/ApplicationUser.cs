using Kertu.InteractiveServer.Data.Models.Elements;
using Microsoft.AspNetCore.Identity;

namespace Kertu.InteractiveServer.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public List<Element> UserElements { get; set; } = [];
    }
}