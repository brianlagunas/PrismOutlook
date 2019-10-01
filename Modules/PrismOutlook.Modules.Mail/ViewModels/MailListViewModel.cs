using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismOutlook.Business;
using PrismOutlook.Core;
using PrismOutlook.Services.Interfaces;
using System;
using System.Collections.ObjectModel;

namespace PrismOutlook.Modules.Mail.ViewModels
{
    public class MailListViewModel : ViewModelBase, IDialogAware, IRegionManagerAware
    {
        private ObservableCollection<MailMessage> _messages;
        private readonly IMailService _mailService;
        private readonly IRegionDialogService _regionDialogService;

        public ObservableCollection<MailMessage> Messages
        {
            get { return _messages; }
            set { SetProperty(ref _messages, value); }
        }

        private MailMessage _selectedMessage;
        public MailMessage SelectedMessage
        {
            get { return _selectedMessage; }
            set { SetProperty(ref _selectedMessage, value); }
        }

        private DelegateCommand<string> _messageCommand;

        public event Action<IDialogResult> RequestClose;

        public DelegateCommand<string> MessageCommand =>
            _messageCommand ?? (_messageCommand = new DelegateCommand<string>(ExecuteMessageCommand));

        public string Title => "Testing";

        public IRegionManager RegionManager { get; set; }

        void ExecuteMessageCommand(string parameter)
        {
            _regionDialogService.ShowRibbonDialog("MessageView");
        }

        public MailListViewModel(IMailService mailService, IRegionDialogService regionDialogService)
        {
            _mailService = mailService;
            _regionDialogService = regionDialogService;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            var folder = navigationContext.Parameters.GetValue<string>(FolderParameters.FolderKey);

            switch (folder)
            {
                case FolderParameters.Inbox:
                    {
                        Messages = new ObservableCollection<MailMessage>(_mailService.GetInboxItems());
                        break;
                    }
                case FolderParameters.Sent:
                    {
                        Messages = new ObservableCollection<MailMessage>(_mailService.GetSentItems());
                        break;
                    }
                case FolderParameters.Deleted:
                    {
                        Messages = new ObservableCollection<MailMessage>(_mailService.GetDeletedItems());
                        break;
                    }
                default:
                    break;
            }            
        }



        public bool CanCloseDialog()
        {
            var result = System.Windows.MessageBox.Show("Close Mail List", "Close?", System.Windows.MessageBoxButton.YesNo);
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
