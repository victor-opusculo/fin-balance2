using System;

namespace FinBalance2.Views.Components
{
    public interface IPeriodProperties
    {
        public DateTime BeginDate { get; }
        public DateTime EndDate { get; }
    }
}
