using QLTS_BIDV.Helper.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace QLTS_BIDV.Views
{
    public class BaseViewModel
    {
        private bool isBusy { get; set; } = false;
        public ValidatableObject<bool> IsBusy { set; get; } = new ValidatableObject<bool>();
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
