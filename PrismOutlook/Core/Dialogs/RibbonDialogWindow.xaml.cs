using Prism.Services.Dialogs;
using System.Windows;

namespace PrismOutlook.Core.Dialogs
{
    /// <summary>
    /// Interaction logic for RibbonDialogWindow.xaml
    /// </summary>
    public partial class RibbonDialogWindow : IDialogWindow
    {
        public RibbonDialogWindow()
        {
            InitializeComponent();
        }

        public IDialogResult Result { get; set; }
    }

}
