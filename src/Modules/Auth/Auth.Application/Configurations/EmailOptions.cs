using Shared.Common.Configurations;
using Shared.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Configurations
{
    public sealed class EmailOptions : BaseServiceConfig
    {
        public required string Host { get; set; }
        public required int Port { get; set; }
        public required string Pass { get; set; }
        public required string From { get; set; }
        public int VerificationExpiryHours { get; set; } = 24;

        public override string ToString()
        {
            return Methods.GetToString(base.ToString(), Host, Port, Pass, From, VerificationExpiryHours);
        }
    }
}
