using System;
using System.Globalization;
using System.Windows.Data;

namespace TrieWpf
{
    public class FoundItemToSelectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var item = value as FoundListItem;
            if (item == null) return Binding.DoNothing;

            return new SelectionPosition { StartIndex = (int)item.Position.CharPos, Length = item.Text.Length };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
