using System;
using System.Collections.Generic;
using System.Text;

namespace QLTS_BIDV.Helper.Validators.Rules
{
    public class RequiredRuleValidation : IRuleValidation<string>
    {

        public RequiredRuleValidation(string mess = "")
        {
            this.ErrorMessage = mess;
        }

        public string ErrorMessage { get; set; }

        public bool Check(string value)
        {
            if (value == "" || value is null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
