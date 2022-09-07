using System.Collections;
using FinBalance2.Models;

namespace FinBalance2.ViewModels
{
    public class SortIncomeExpenseStatus : IComparer
    {
        int IComparer.Compare(object? x, object? y)
        {
            FinMovement val1 = x as FinMovement ?? new FinMovement() { Value = 0 };
            FinMovement val2 = y as FinMovement ?? new FinMovement() { Value = 0 };

            if (val1.Value > val2.Value) return 1;
            if (val2.Value > val1.Value) return -1;
            else return 0;
        }
    }

    public class SortByDate : IComparer
    {
        int IComparer.Compare(object? x, object? y)
        {
            FinMovement val1 = x as FinMovement ?? new FinMovement() { Value = 0 };
            FinMovement val2 = y as FinMovement ?? new FinMovement() { Value = 0 };

            if (val1.Date > val2.Date) return 1;
            if (val2.Date > val1.Date) return -1;
            else return 0;
        }
    }
}