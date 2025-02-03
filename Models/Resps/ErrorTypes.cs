using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Resps
{
    public enum ErrorTypes
    {
        TokenExpired = 0, Unknown = 1, ServerUnavaliable = 2, WrongEmailOrPassword = 3, Unauthorized = 4, BodyContentNull = 5,
    }
}
