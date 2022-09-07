using Avalonia.Data;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using System;
using System.Globalization;

namespace FinBalance2.ViewModels
{
    public class ComparisonConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            return value?.Equals(parameter);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            return value?.Equals(true) == true ? parameter : BindingOperations.DoNothing;
        }
    }

    public class IncomeAndExpensesConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            decimal val = System.Convert.ToDecimal(value);
            if (val != 0)
            {
                PathIcon icon = new()
                {
                    Foreground = val > 0 ? new SolidColorBrush(Color.FromRgb(0, 255, 0)) : new SolidColorBrush(Color.FromRgb(255, 0, 0)),
                    [!PathIcon.DataProperty] = val > 0 ? new DynamicResourceExtension("arrow_up_regular") : new DynamicResourceExtension("arrow_down_regular")
                };
                return icon;
            }
            else
                return new PathIcon() { };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BalanceValueStyleConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            decimal val = System.Convert.ToDecimal(value);

            if (val > 0) return Brushes.Green;
            if (val < 0) return Brushes.Red;
            else return Brushes.White;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EditDateConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            DateTimeOffset? dt = new DateTimeOffset( value as DateTime? ?? DateTime.Today );
            return dt;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            DateTimeOffset dto = value as DateTimeOffset? ?? DateTime.Today;
            return dto.DateTime;
        }
    }

    public class EditValueConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return (double?)(value as decimal?);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return System.Convert.ToDecimal(value as double?);
        }
    }
}
