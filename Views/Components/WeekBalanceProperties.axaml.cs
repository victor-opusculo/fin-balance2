using Avalonia.Controls;
using System;

namespace FinBalance2.Views.Components
{
    public partial class WeekBalanceProperties : UserControl, IPeriodProperties
    {
        public WeekBalanceProperties()
        {
            InitializeComponent();
            dpWeekOfDay.SelectedDate = new DateTimeOffset(DateTime.Today);
        }

        public DateTime BeginDate
        {
            get
            {
                DateTime selected = this.dpWeekOfDay.SelectedDate?.DateTime ?? DateTime.Today;
                int dayDiff = selected.DayOfWeek - DayOfWeek.Sunday;
                return selected.AddDays(-dayDiff);
            }
        }

        public DateTime EndDate
        {
            get
            {
                DateTime selected = this.dpWeekOfDay.SelectedDate?.DateTime ?? DateTime.Today;
                return selected.AddDays(6 - (int)selected.DayOfWeek);
            }
        }
    }
}
