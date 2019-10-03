using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace PrismOutlook.Core.Dialogs
{
    public class RegionDialogService : IRegionDialogService
    {
        private readonly IContainerExtension _containerExtension;
        private readonly IRegionManager _regionManager;

        public RegionDialogService(IContainerExtension containerExtension, IRegionManager regionManager)
        {
            _containerExtension = containerExtension;
            _regionManager = regionManager;
        }

        public void Show(string name, IDialogParameters dialogParameters, Action<IDialogResult> callback)
        {
            var window = _containerExtension.Resolve<RibbonDialogWindow>();

            var scopedRegionManager = _regionManager.CreateRegionManager();
            RegionManager.SetRegionManager(window, scopedRegionManager);

            IRegion region = scopedRegionManager.Regions[RegionNames.ContentRegion];

            region.RequestNavigate(name);

            var activeView = region.ActiveViews.FirstOrDefault() as FrameworkElement;
            IDialogAware dialogAware = activeView.DataContext as IDialogAware;
            if (dialogAware == null)
                throw new ArgumentNullException("dialogAware");

            dialogAware.OnDialogOpened(dialogParameters);

            Action<IDialogResult> requestCloseHandler = null;
            requestCloseHandler = (result) =>
            {
                window.Result = result;
                window.Close();
            };

            CancelEventHandler closingHandler = null;
            closingHandler = (o, e) =>
            {
                if (!dialogAware.CanCloseDialog())
                    e.Cancel = true;
            };
            window.Closing += closingHandler;

            RoutedEventHandler loadedHandler = null;
            loadedHandler = (o, e) =>
            {
                window.Loaded -= loadedHandler;
                dialogAware.RequestClose += requestCloseHandler;
            };
            window.Loaded += loadedHandler;

            EventHandler closedHandler = null;
            closedHandler = (o, e) =>
            {
                window.Closed -= closedHandler;
                window.Closing -= closingHandler;
                dialogAware.RequestClose -= requestCloseHandler;

                dialogAware.OnDialogClosed();

                var result = window.Result;
                if (result == null)
                    result = new DialogResult();

                callback.Invoke(result);                

                window.DataContext = null;
                window.Content = null;
            };
            window.Closed += closedHandler;

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Show();
        }
    }
}
