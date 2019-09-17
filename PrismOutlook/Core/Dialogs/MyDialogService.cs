using Prism.Common;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PrismOutlook.Core.Dialogs
{
    //TODO: think about this some more
    public class MyDialogService : IMyDialogService
    {
        private readonly IContainerExtension _containerExtension;
        private readonly IRegionManager _regionManager;

        public MyDialogService(IContainerExtension containerExtension, IRegionManager regionManager) 
        {
            _containerExtension = containerExtension;
            _regionManager = regionManager;
        }

        public void Show(string name)
        {
            var window = _containerExtension.Resolve<RibbonWindow>();

            var regionManager = _regionManager.CreateRegionManager();
            RegionManager.SetRegionManager(window, regionManager);

            regionManager.RequestNavigate(RegionNames.ContentRegion, name);

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Show();
        }
    }
}
