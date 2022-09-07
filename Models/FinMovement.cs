using System;
using ReactiveUI;

namespace FinBalance2.Models
{
    public class FinMovement : ReactiveObject
    {
        private decimal mValue;
        public int? Id { get; set; }
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
