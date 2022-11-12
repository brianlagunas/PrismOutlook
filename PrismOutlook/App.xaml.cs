using PrismOutlook.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using PrismOutlook.Modules.Mail;
using Prism.Regions;
using Infragistics.Windows.OutlookBar;
using PrismOutlook.Core.Regions;
using Infragistics.Windows.Ribbon;
using PrismOutlook.Modules.Contacts;
using PrismOutlook.Core;
using Infragistics.Themes;
using PrismOutlook.Core.Dialogs;
using PrismOutlook.ViewModels;

namespace PrismOutlook
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell(Window shell)
        {
            ThemeManager.ApplicationTheme = new Office2013Theme();
            base.InitializeShell(shell);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //custom dialog service that handles regions
            containerRegistry.RegisterSingleton<IRegionDialogService, RegionDialogService>();
            containerRegistry.RegisterSingleton<IApplicationCommands, ApplicationCommands>();

            //using the default dialog service for simple dialogs
            containerRegistry.RegisterDialog<ErrorDialog, ErrorDialogViewModel>("Error");
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<MailModule>();
            moduleCatalog.AddModule<ContactsModule>();
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
            regionAdapterMappings.RegisterMapping(typeof(XamOutlookBar), Container.Resolve<XamOutlookBarRegionAdapter>());
            regionAdapterMappings.RegisterMapping(typeof(XamRibbon), Container.Resolve<XamRibbonRegionAdapter>());
        }

        protected override void ConfigureDefaultRegionBehaviors(IRegionBehaviorFactory regionBehaviors)
        {
            base.ConfigureDefaultRegionBehaviors(regionBehaviors);

            regionBehaviors.AddIfMissing(DependentViewRegionBehavior.BehaviorKey, typeof(DependentViewRegionBehavior));
        }
    }
}
