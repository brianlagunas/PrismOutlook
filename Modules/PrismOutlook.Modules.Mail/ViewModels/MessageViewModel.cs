using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using PrismOutlook.Business;
using PrismOutlook.Core;
using PrismOutlook.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PrismOutlook.Modules.Mail.ViewModels
{
    public class MessageViewModel : BindableBase, IDialogAware
    {
        private DelegateCommand _sendMessageCommand;
        private readonly IMailService _mailService;

        private MailMessage _message;
        public MailMessage Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public DelegateCommand SendMessageCommand =>
            _sendMessageCommand ?? (_sendMessageCommand = new DelegateCommand(ExecuteSendMessageCommand));

        void ExecuteSendMessageCommand()
        {
            _mailService.SendMessage(Message);

            //todo: fix magic string
            IDialogParameters parameters = new DialogParameters();
            parameters.Add("messageSent", Message);

            RequestClose?.Invoke(new DialogResult(ButtonResult.Yes, parameters));
        }

        public MessageViewModel(IMailService mailService)
        {
            _mailService = mailService;
        }

        //TODO: use this
        public string Title => "Mail Message";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Message = new MailMessage() { From = "blagunas@infragistics.com" };

            var messageMode = parameters.GetValue<MessageMode>(MailParameters.MessageMode);
            if (messageMode != MessageMode.New)
            {
                var messageId = parameters.GetValue<int>(MailParameters.MessageId);
                var originalMessage = _mailService.GetMessage(messageId);

                Message.To = GetToEmails(messageMode, originalMessage);

                if (messageMode == MessageMode.Reply || messageMode == MessageMode.ReplyAll)
                    Message.CC = originalMessage.CC;

                Message.Subject = GetMessageSubject(messageMode, originalMessage);

                //TODO: append RTF with reply header
                Message.Body = originalMessage.Body;
            }
        }

        string GetMessageSubject(MessageMode mode, MailMessage originalMessage)
        {
            string prefix = string.Empty;

            switch (mode)
            {
                case MessageMode.Reply:
                case MessageMode.ReplyAll:
                    {
                        prefix = "RE:";
                        break;
                    }
                case MessageMode.Forward:
                    {
                        prefix = "FW:";
                        break;
                    }
            }

            return originalMessage.Subject.ToLower().StartsWith(prefix.ToLower()) ? originalMessage.Subject : $"{prefix} {originalMessage.Subject}";
        }

        ObservableCollection<string> GetToEmails(MessageMode mode, MailMessage message)
        {
            var toEmails = new ObservableCollection<string>();

            switch (mode)
            {
                case MessageMode.Reply:
                    {
                        toEmails.Add(message.From);
                        break;
                    }
                case MessageMode.ReplyAll:
                    {
                        //TODO: create user/account settings for sender email
                        toEmails.AddRange(message.To.Where( x => x != "blagunas@infragistics.com"));
                        toEmails.Add(message.From);
                        break;
                    }
                case MessageMode.Forward:
                    {
                        break;
                    }
            }

            return toEmails; ;
        }
    }
}
