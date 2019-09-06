using Infragistics.Windows.OutlookBar;
using PrismOutlook.Core;

namespace PrismOutlook.Modules.Contacts.Menus
{
    /// <summary>
    /// Interaction logic for ContactsGroup.xaml
    /// </summary>
    public partial class ContactsGroup : OutlookBarGroup, IOutlookBarGroup
    {
        public ContactsGroup()
        {
            InitializeComponent();
        }

        public string DefaultNavigationPath => "ViewA";
    }
}
