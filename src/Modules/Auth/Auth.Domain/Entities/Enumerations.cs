using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Domain.Entities
{
    public enum Gender
    {
        Male,
        Female,
        Other,
        PerferNotToSay
    };

    public enum Status
    {
        SUCCESS,
        FAILED,
        LOCKED,
    };
}
