using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace QLTS_BIDV.Helper.Validators.Rules
{
    public class EmailRuleValidation: IRuleValidation<string>
    {
        string Pattern { get; set; } = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
        public string ErrorMessage { get; set; } = "Email format is Valid";

        public EmailRuleValidation(string mess = "", string partern = "")
        {
            if (!(partern is null || partern == ""))
            {
                this.Pattern = partern;
            }
            if (!(mess is null || mess == ""))
            {
                this.ErrorMessage = mess;
            }
        }

        public bool Check(string value)
        {
            if (value is null) return false;
            if (Regex.Match(value, this.Pattern).Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
