using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DripChip
{
    public class AuthenticationOptions
    {
        public const string ISSUER = "DripChipAPI";
        public const string AUDIENCE = "DripChipClient";
        const string KEY = "superLongUltraSecretHardImpossibleKey";

        public static SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
