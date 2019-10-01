using PrismOutlook.Core;
using PrismOutlook.Modules.Mail.Menus;
using System.Windows.Controls;

namespace PrismOutlook.Modules.Mail.Views
{
    /// <summary>
    /// Interaction logic for MessageView
    /// </summary>
    [DependentView(RegionNames.RibbonRegion, typeof(HomeTab))]
    public partial class MessageView : UserControl, ISupportDataContext
    {
        public MessageView()
        {
            InitializeComponent();
        }
    }
}
