using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask
{
    public class AuthOptions
    {
        public const string ISSUER = "Server"; // издатель токена
        public const string AUDIENCE = "https://localhost:44325"; // потребитель токена
        const string KEY = "SecRet_kEYqeweAW123dq";   // ключ для шифрации
        public const int LIFETIME = 5; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
