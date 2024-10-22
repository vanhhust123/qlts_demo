using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace QLTS_BIDV.Helper.Validators
{
    public class ValidatableObject<T>: INotifyPropertyChanged
    {
        // Khai báo các biến 
        public event PropertyChangedEventHandler PropertyChanged;
        // Được gọi khi thuộc tính thay đổi  
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // Trường dữ liệu này có valid không, và các trường này sẽ là private để triển khai getter setter
        private bool isValid { get; set; } = true;
        private List<IRuleValidation<T>> ruleValidations { get; set; } = new List<IRuleValidation<T>>();
        private string error { get; set; } = "";
        private T value { get; set; }

        // Thêm trường Control IsTouched ? 
        private bool isTouched { set; get; } = false;

        public bool IsTouched
        {
            get { return this.isTouched; }
            set
            {
                this.isTouched = value;
                this.OnPropertyChanged();
            }
        }

        // Impelement getter setter of fields 
        public bool IsValid
        {
            get { return this.isValid; }
            set
            {
                if (!Object.Equals(this.value, value))
                {
                    this.isValid = value;
                    this.OnPropertyChanged();
                }

            }
        }

        public T Value
        {
            get { return this.value; }
            set
            {
                this.value = value;
                OnPropertyChanged();
            }
        }

        public List<IRuleValidation<T>> RuleValidations
        {
            get { return this.ruleValidations; }
            set { this.ruleValidations = value; }
        }

        // Hiển thị lỗi của trường được binding
        public string Error
        {
            get { return this.error; }
            set
            {
                this.error = value;
                this.OnPropertyChanged();
            }
        }

        // Funciton ultilitys
        public void AddRuleValidation(IRuleValidation<T> rule)
        {
            this.ruleValidations.Add(rule);
        }

        // Validate trường dữ liệu được bind
        public bool Validate()
        {
            this.IsValid = true;
            if (!(this.RuleValidations is null || this.RuleValidations.Count == 0))
            {
                List<string> errors = this.RuleValidations.Where(v => !v.Check(this.Value)).
                    Select(x => x.ErrorMessage).ToList();

                if (errors.Any())
                {
                    this.Error = "";
                    errors.ForEach(e =>
                    {
                        this.Error += e + "\n";
                    });
                    this.IsValid = false;
                }
                else
                {
                    this.Error = "";
                    this.IsValid = true;
                }
            }

            return this.isValid;
        }
    }
}
