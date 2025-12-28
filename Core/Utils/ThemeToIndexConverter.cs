using System.Globalization;
using System.Windows.Data;
using WhereMyWinCursor.Core.Config.UI;

namespace WhereMyWinCursor.Core.Utils;

internal sealed class ThemeToIndexConverter : IValueConverter {
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (value is ThemeEnum.Dark) {
            return 1;
        }

        if (value is ThemeEnum.Light) {
            return 2;
        }

        return 0;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (value is 1) {
            return ThemeEnum.Dark;
        }

        if (value is 2) {
            return ThemeEnum.Light;
        }

        return ThemeEnum.System;
    }
}