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
using Prism.Services.Dialogs;

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
            Infragistics.Themes.ThemeManager.ApplicationTheme = new Office2013Theme();
            base.InitializeShell(shell);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IRegionDialogService, RegionDialogService>();

            containerRegistry.RegisterSingleton<IApplicationCommands, ApplicationCommands>();

            //containerRegistry.RegisterDialogWindow<RibbonWindow>();
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
