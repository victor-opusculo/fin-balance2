using System;
using ReactiveUI;

namespace FinBalance2.Models
{
    public class FinMovement : ReactiveObject
    {
        private int? mId;
        private decimal mValue;

        public int? Id
        {
            get => mId;
            set { mId = value; this.RaisePropertyChanged("Id"); }
        }
        public string? Name { get; set; }
        public DateTime? Date { get; set; }
        public decimal Value 
        {
            get => mValue;
            set 
            {
                mValue = value;
                this.RaisePropertyChanged("Value"); 
            } 
        }
        public string? Notes { get; set; }
    }
}
