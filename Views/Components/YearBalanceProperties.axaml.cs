using Avalonia.Controls;
using System;

namespace FinBalance2.Views.Components
{
    public partial class YearBalanceProperties : UserControl, IPeriodProperties
    {
        public YearBalanceProperties()
        {
            InitializeComponent();
            dpYear.SelectedDate = new DateTimeOffset(DateTime.Today);
        }

        public DateTime BeginDate => new DateTime(dpYear.SelectedDate?.Year ?? DateTime.Today.Year, 1, 1);
        public DateTime EndDate => new DateTime(dpYear.SelectedDate?.Year ?? DateTime.Today.Year, 12, 31);
    }
}
