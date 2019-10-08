using PrismOutlook.Business;
using System.Collections.Generic;

namespace PrismOutlook.Services.Interfaces
{
    public interface IMailService
    {
        IList<MailMessage> GetInboxItems();
        IList<MailMessage> GetSentItems();
        IList<MailMessage> GetDeletedItems();

        MailMessage GetMessage(int id);
        void DeleteMessage(int id);
        void SendMessage(MailMessage message);
    }
}
