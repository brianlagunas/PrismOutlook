using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismOutlook.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrismOutlook.Modules.Mail.ViewModels
{
    public class MessageViewModel : BindableBase, IDialogAware, IRegionManagerAware
    {
        private DelegateCommand _messageCommand;
        public DelegateCommand MessageCommand =>
            _messageCommand ?? (_messageCommand = new DelegateCommand(ExecuteMessageCommand));

        void ExecuteMessageCommand()
        {
            RegionManager.RequestNavigate(RegionNames.ContentRegion, "MailList");

            //RequestClose?.Invoke(new DialogResult());
        }

        public MessageViewModel(IRegionManager regionManager)
        {

        }

        public string Title => "Mail Message";

        private IRegionManager _regionManager;
        public IRegionManager RegionManager
        {
            get { return _regionManager; }
            set
            {
                SetProperty(ref _regionManager, value);
            }
        }

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            var result = System.Windows.MessageBox.Show("Close Message View", "Close?", System.Windows.MessageBoxButton.YesNo);
            if (result == System.Windows.MessageBoxResult.Yes)
                return true;

            return false;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {

        }
    }
}
