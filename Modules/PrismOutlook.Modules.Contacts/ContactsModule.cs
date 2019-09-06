using PrismOutlook.Modules.Contacts.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using PrismOutlook.Core;
using PrismOutlook.Modules.Contacts.Menus;
using PrismOutlook.Modules.Contacts.ViewModels;

namespace PrismOutlook.Modules.Contacts
{
    public class ContactsModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public ContactsModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.OutlookGroupRegion, typeof(ContactsGroup));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ViewA, ViewAViewModel>();
        }
    }
}