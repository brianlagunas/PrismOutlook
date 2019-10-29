using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismOutlook.Business;
using PrismOutlook.Core;
using PrismOutlook.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Linq;

namespace PrismOutlook.Modules.Mail.ViewModels
{
    public class MailListViewModel : MessageViewModelBase
    {
        private ObservableCollection<MailMessage> _messages = new ObservableCollection<MailMessage>();
        private string _currentFolder = FolderParameters.Inbox;

        public ObservableCollection<MailMessage> Messages
        {
            get { return _messages; }
            set { SetProperty(ref _messages, value); }
        }

        private DelegateCommand _newMessageCommand;
        public DelegateCommand NewMessageCommand =>
            _newMessageCommand ?? (_newMessageCommand = new DelegateCommand(ExecuteNewMessageCommand));

        public MailListViewModel(IMailService mailService, IRegionDialogService regionDialogService) :
            base(mailService, regionDialogService)
        { }

        void ExecuteNewMessageCommand()
        {
            var parameters = new DialogParameters();
            parameters.Add("id", 0);

            RegionDialogService.Show("MessageView", parameters, (result) =>
            {
                if (_currentFolder == FolderParameters.Sent)
                    Messages.Add(result.Parameters.GetValue<MailMessage>("messageSent"));
            });
        }

        protected override void ExecuteDeleteMessage()
        {
            base.ExecuteDeleteMessage();

            Messages.Remove(Message);
        }

        protected override void HandleMessageCallBack(IDialogResult result)
        {
            var mode = result.Parameters.GetValue<MessageMode>(MailParameters.MessageMode);
            if (mode == MessageMode.Delete)
            {
                var messageId = result.Parameters.GetValue<int>(MailParameters.MessageId);

                var messageToDelete = Messages.Where(x => x.Id == messageId).FirstOrDefault();
                if (messageToDelete != null)
                    Messages.Remove(messageToDelete);
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            _currentFolder = navigationContext.Parameters.GetValue<string>(FolderParameters.FolderKey);
            LoadMessages(_currentFolder);
        }

        void LoadMessages(string folder)
        {
            switch (folder)
            {
                case FolderParameters.Inbox:
                    {
                        Messages = new ObservableCollection<MailMessage>(MailService.GetInboxItems());
                        break;
                    }
                case FolderParameters.Sent:
                    {
                        Messages = new ObservableCollection<MailMessage>(MailService.GetSentItems());
                        break;
                    }
                case FolderParameters.Deleted:
                    {
                        Messages = new ObservableCollection<MailMessage>(MailService.GetDeletedItems());
                        break;
                    }
                default:
                    break;
            }

            Message = Messages.FirstOrDefault();
        }
    }
}
