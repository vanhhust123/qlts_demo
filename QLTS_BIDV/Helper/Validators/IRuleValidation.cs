using System;
using System.Collections.Generic;
using System.Text;

namespace QLTS_BIDV.Helper.Validators
{
    public interface IRuleValidation<T>
    {
        string ErrorMessage { get; set; }
        bool Check(T value);
    }
}
