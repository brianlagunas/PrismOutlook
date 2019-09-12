using Infragistics.Controls.Menus;
using Infragistics.Windows.OutlookBar;
using PrismOutlook.Business;
using PrismOutlook.Core;
using PrismOutlook.Modules.Mail.ViewModels;
using System.Linq;

namespace PrismOutlook.Modules.Mail.Menus
{
    /// <summary>
    /// Interaction logic for MailGroup.xaml
    /// </summary>
    public partial class MailGroup : OutlookBarGroup, IOutlookBarGroup
    {
        public MailGroup()
        {
            InitializeComponent();

            _dataTree.Loaded += DataTree_Loaded;
        }

        private void DataTree_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _dataTree.Loaded -= DataTree_Loaded;

            var parentNode = _dataTree.Nodes[0];
            var nodeToSelect = parentNode.Nodes[0];
            nodeToSelect.IsSelected = true;            
        }

        public string DefaultNavigationPath
        {
            get
            {
                var item = _dataTree.SelectionSettings.SelectedNodes[0] as XamDataTreeNode;
                if (item != null)
                    return ((NavigationItem)item.Data).NavigationPath;

                return $"MailList?{FolderParameters.FolderKey}={FolderParameters.Inbox}";
            }
        }
    }
}
