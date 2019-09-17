using Prism.Regions;
using Prism.Services.Dialogs;
using System.Windows;

namespace PrismOutlook.Core.Dialogs
{
    //TODO: think about this some more
    public static class IDialogServiceExtensions
    {
        public static void ShowRibbonWindow(this IDialogService dialogService, string name)
        {
            var window = new RibbonWindow();

            var regionManager = new RegionManager();
            RegionManager.SetRegionManager(window, regionManager);

            regionManager.RequestNavigate(RegionNames.ContentRegion, name);

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Show();
        }
    }
}
