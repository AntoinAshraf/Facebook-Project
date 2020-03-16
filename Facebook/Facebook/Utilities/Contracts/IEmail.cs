using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Contracts
{
    public interface IEmail
    {
        void SendRecoveryPasswordEmail(string ReseverEmail, int RecoveryCode, string CallBackUrl);
        void SendAccountActivationEmail(string ReseverEmail, string CallBackUrl);
    }
}
