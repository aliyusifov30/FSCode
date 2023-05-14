using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FSCode.Application.HelperManager
{
    public sealed class RegexManager
    {
        public static bool CheckMailRegex(string email)
        {
            string mailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            string mailInput = email;

            if (mailInput != null)
            {
                Match match = null;
                try
                {
                    match = Regex.Match(mailInput, mailPattern);
                }
                catch (Exception e)
                {
                    return false;
                }
                if (match.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
}
