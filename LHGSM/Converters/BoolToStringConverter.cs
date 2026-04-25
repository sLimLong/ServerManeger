using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace LHGSM.Converters;

public class BoolToStringConverter : IValueConverter
{
    public static readonly BoolToStringConverter Instance = new();
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isRunning)
        {
            return isRunning ? "● RUNNING" : "○ STOPPED";
        }
        return "● UNKNOWN";
    }
    
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}