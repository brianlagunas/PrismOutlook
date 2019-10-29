using Prism.Services.Dialogs;
using PrismOutlook.Core;
using PrismOutlook.Services.Interfaces;
using System;

namespace PrismOutlook.Modules.Mail.ViewModels
{
    public class MessageReadOnlyViewModel : MessageViewModelBase, IDialogAware
    {
        public event Action<IDialogResult> RequestClose;

        public string Title => "";

        public MessageReadOnlyViewModel(IMailService mailService, IRegionDialogService regionDialogService) :
            base (mailService, regionDialogService)
        {

        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            var messageId = parameters.GetValue<int>(MailParameters.MessageId);
            if (messageId != 0)
                Message = MailService.GetMessage(messageId);
        }

        protected override void ExecuteDeleteMessage()
        {
            base.ExecuteDeleteMessage();

            var p = new DialogParameters();
            p.Add(MailParameters.MessageMode, MessageMode.Delete);
            p.Add(MailParameters.MessageId, Message.Id);

            var result = new DialogResult(ButtonResult.OK, p);

            RequestClose(result);
        }
    }
}
