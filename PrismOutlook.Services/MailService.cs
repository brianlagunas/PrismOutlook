using PrismOutlook.Business;
using PrismOutlook.Services.Data;
using PrismOutlook.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PrismOutlook.Services
{
    public class MailService : IMailService
    {
        static List<MailMessage> InboxItems = new List<MailMessage>()
        {
            new MailMessage()
            {
                Id = 1,
                From = "jerrynixon@microsoft.com",
                To = new ObservableCollection<string>(){ "jane@doe.com", "john@doe.com" },
                Subject = "This is a test email",
                Body = Resources.DavidSmit_SampleCoverLetterEmail,
                DateSent = DateTime.Now
            },
            new MailMessage()
            {
                Id = 2,
                From = "jerrynixon@microsoft.com",
                To = new ObservableCollection<string>(){ "jane@doe.com", "john@doe.com" },
                Subject = "This is a test email 2",
                Body = Resources.Barbara_Bailey_RE_GraphicDesignerCoverLetter,
                DateSent = DateTime.Now.AddDays(-1)
            },
            new MailMessage()
            {
                Id = 3,
                From = "jerrynixon@microsoft.com",
                To = new ObservableCollection<string>(){ "jane@doe.com", "john@doe.com" },
                Subject = "This is a test email 3",
                Body = Resources.MargaretJones_RE_GraphicDesignerCoverLetter,
                DateSent = DateTime.Now.AddDays(-5)
            },
        };

        static List<MailMessage> SentItems = new List<MailMessage>();

        static List<MailMessage> DeletedItems = new List<MailMessage>();

        public void DeleteMessage(int id)
        {
            var messages = new List<MailMessage>();

            var message = DeletedItems.FirstOrDefault(m => m.Id == id);
            if (message != null)
            {
                DeletedItems.Remove(message);
                return;
            }

            message = InboxItems.FirstOrDefault(m => m.Id == id);
            if (message != null)
            {
                InboxItems.Remove(message);
            }
            else
            {
                message = SentItems.FirstOrDefault(m => m.Id == id);
                if (message != null)
                    SentItems.Remove(message);
            }

            if (message != null)
            {
                DeletedItems.Add(message);
            }
        }

        public IList<MailMessage> GetDeletedItems()
        {
            return DeletedItems;
        }

        public IList<MailMessage> GetInboxItems()
        {
            return InboxItems;
        }

        public MailMessage GetMessage(int id)
        {
            var messages = new List<MailMessage>();
            messages.AddRange(InboxItems);
            messages.AddRange(SentItems);
            messages.AddRange(DeletedItems);
            return messages.FirstOrDefault(m => m.Id == id);
        }

        public IList<MailMessage> GetSentItems()
        {
            return SentItems;
        }

        public void SendMessage(MailMessage message)
        {
            message.DateSent = DateTime.Now;
            SentItems.Add(message);
        }
    }
}
