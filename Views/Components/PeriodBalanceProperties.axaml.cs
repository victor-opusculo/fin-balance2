using Avalonia.Controls;
using System;

namespace FinBalance2.Views.Components
{
    public partial class PeriodBalanceProperties : UserControl, IPeriodProperties
    {
        public PeriodBalanceProperties()
        {
            InitializeComponent();
            dpBegin.SelectedDate = DateTime.Today;
            dpEnd.SelectedDate = DateTime.Today;
        }

        public DateTime BeginDate => dpBegin.SelectedDate?.DateTime ?? DateTime.Today;
        public DateTime EndDate => dpEnd.SelectedDate?.DateTime ?? DateTime.Today;
    }
}
