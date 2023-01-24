using Microsoft.AspNetCore.Identity;

namespace Indigo.Entities.Concrets
{
    public class AppUser : IdentityUser
    {
        public string FirstName;
        public string LastName;
    }
}
