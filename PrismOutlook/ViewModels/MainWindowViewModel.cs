using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using PrismOutlook.Core;
using System;

namespace PrismOutlook.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private DelegateCommand<string> _navigateCommand;
        private readonly IRegionManager _regionManager;

        public DelegateCommand<string> NavigateCommand =>
            _navigateCommand ?? (_navigateCommand = new DelegateCommand<string>(ExecuteNavigateCommand));

        public MainWindowViewModel(IRegionManager regionManager, IApplicationCommands applicationCommands)
        {
            _regionManager = regionManager;
            applicationCommands.NavigateCommand.RegisterCommand(NavigateCommand);
        }

        void ExecuteNavigateCommand(string navigationPath)
        {
            if (string.IsNullOrEmpty(navigationPath))
                throw new ArgumentNullException();

            _regionManager.RequestNavigate(RegionNames.ContentRegion, navigationPath);
        }
    }
}
