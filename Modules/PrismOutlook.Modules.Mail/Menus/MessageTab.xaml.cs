using Infragistics.Windows.Ribbon;
using PrismOutlook.Core;

namespace PrismOutlook.Modules.Mail.Menus
{
    /// <summary>
    /// Interaction logic for MessageTab.xaml
    /// </summary>
    public partial class MessageTab : ISupportDataContext
    {
        public MessageTab()
        {
            InitializeComponent();
            SetResourceReference(StyleProperty, typeof(RibbonTabItem));
        }
    }
}
