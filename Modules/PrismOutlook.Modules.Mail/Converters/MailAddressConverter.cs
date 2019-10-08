using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PrismOutlook.Modules.Mail.Converters
{
    public class MailAddressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<string> emails)
                return string.Join("; ", emails);

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var emailCollection = new ObservableCollection<string>();

            if (value == null && string.IsNullOrWhiteSpace(value.ToString()))
                return emailCollection;

            var emails = value.ToString().Replace(" ", "");
            var emailItems = emails.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            return emailCollection.AddRange(emailItems);
        }
    }
}
