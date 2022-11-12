using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismOutlook.Business;
using PrismOutlook.Core;
using PrismOutlook.Services.Interfaces;
using System;
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

        private DelegateCommand _throwExceptionCommand;
        private readonly IDialogService _dialogService;

        public DelegateCommand ThrowExceptionCommand =>
            _throwExceptionCommand ?? (_throwExceptionCommand = new DelegateCommand(ExecuteThrowExceptionCommand));

        public MailListViewModel(IMailService mailService, IRegionDialogService regionDialogService, IDialogService dialogService) :
            base(mailService, regionDialogService)
        {
            _dialogService = dialogService;
        }

        void ExecuteNewMessageCommand()
        {
            var parameters = new DialogParameters
            {
                { "id", 0 },
                { MailParameters.MessageMode, MessageMode.New }
            };

            RegionDialogService.Show("MessageView", parameters, (result) =>
            {
                if (_currentFolder == FolderParameters.Sent)
                    Messages.Add(result.Parameters.GetValue<MailMessage>("messageSent"));
            });
        }

        void ExecuteThrowExceptionCommand()
        {
            try
            {
                throw new System.Exception(" This is an exception that was thrown in code.");
            }
            catch (Exception ex)
            {
                var parameters = new DialogParameters()
                {
                    { "message", ex.Message}
                };

                _dialogService.ShowDialog("ErrorDialog", parameters, (result) =>
                {
                    //todo: handle callback if needed
                });
            }
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
