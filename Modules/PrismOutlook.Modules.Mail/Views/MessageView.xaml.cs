using Infragistics.Controls.Editors;
using PrismOutlook.Core;
using PrismOutlook.Modules.Mail.Menus;
using System.Windows.Controls;

namespace PrismOutlook.Modules.Mail.Views
{
    /// <summary>
    /// Interaction logic for MessageView
    /// </summary>
    [DependentView(RegionNames.RibbonRegion, typeof(MessageTab))] 
    public partial class MessageView : UserControl, ISupportDataContext, ISupportRichText
    {
        public MessageView()
        {
            InitializeComponent();
        }

        public XamRichTextEditor RichTextEditor
        {
            get => _rte;
            set => throw new System.NotImplementedException();
        }
    }
}
