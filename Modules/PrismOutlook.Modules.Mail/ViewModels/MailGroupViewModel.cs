using Prism.Commands;
using PrismOutlook.Business;
using PrismOutlook.Core;
using System.Collections.ObjectModel;

namespace PrismOutlook.Modules.Mail.ViewModels
{
    public class MailGroupViewModel : ViewModelBase
    {
        private ObservableCollection<NavigationItem> _items;
        public ObservableCollection<NavigationItem> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        private DelegateCommand<NavigationItem> _selectedCommand;
        private readonly IApplicationCommands _applicationCommands;

        public DelegateCommand<NavigationItem> SelectedCommand =>
            _selectedCommand ?? (_selectedCommand = new DelegateCommand<NavigationItem>(ExecuteSelectedCommand));

        public MailGroupViewModel(IApplicationCommands applicationCommands)
        {
            GenerateMenu();
            _applicationCommands = applicationCommands;
        }

        void ExecuteSelectedCommand(NavigationItem navigationItem)
        {
            if (navigationItem != null)
                _applicationCommands.NavigateCommand.Execute(navigationItem.NavigationPath);
        }

        void GenerateMenu()
        {
            Items = new ObservableCollection<NavigationItem>();

            var root = new NavigationItem() { Caption = "Personal Folder", NavigationPath = "MailList?id=Default" };
            root.Items.Add(new NavigationItem() { Caption = "Inbox", NavigationPath = "MailList?id=Inbox" });
            root.Items.Add(new NavigationItem() { Caption = "Deleted", NavigationPath = "MailList?id=Deleted" });
            root.Items.Add(new NavigationItem() { Caption = "Sent", NavigationPath = "MailList?id=Sent" });

            Items.Add(root);
        }
    }
}
