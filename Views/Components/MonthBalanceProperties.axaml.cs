using Avalonia.Controls;
using System;

namespace FinBalance2.Views.Components
{
    public partial class MonthBalanceProperties : UserControl, IPeriodProperties
    {
        public MonthBalanceProperties()
        {
            InitializeComponent();
            dpMonth.SelectedDate = new DateTimeOffset(DateTime.Today);
        }

        public DateTime BeginDate
        {
            get
            {
                DateTime selected = dpMonth.SelectedDate?.DateTime ?? DateTime.Today;
                return new DateTime(selected.Year, selected.Month, 1);
            }
        }

        public DateTime EndDate
        {
            get
            {
                DateTime selected = dpMonth.SelectedDate?.DateTime ?? DateTime.Today;
                return new DateTime(selected.Year, selected.Month, DateTime.DaysInMonth(selected.Year, selected.Month));
            }
        }
    }
}
