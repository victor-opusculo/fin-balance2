using Avalonia.Controls;
using System;

namespace FinBalance2.Views.Components
{
    public partial class DayBalanceProperties : UserControl, IPeriodProperties
    {
        public DayBalanceProperties()
        {
            InitializeComponent();
            dpDay.SelectedDate = new DateTimeOffset(DateTime.Now);
        }

        public DateTime BeginDate => this.dpDay.SelectedDate?.DateTime ?? DateTime.Today;
        public DateTime EndDate => this.dpDay.SelectedDate?.DateTime ?? DateTime.Today;
    }
}
