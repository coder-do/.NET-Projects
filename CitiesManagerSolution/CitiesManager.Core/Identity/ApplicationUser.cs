using Microsoft.AspNetCore.Identity;

namespace CitiesManager.Core.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? RefreshToken { get; set; } = string.Empty;
        public string? PersonName { get; set; }
        public DateTime RefreshTokenExpirationDateTime { get; set; }
    }
}

