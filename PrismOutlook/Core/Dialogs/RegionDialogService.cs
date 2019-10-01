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
    //TODO: think about this some more
    public class RegionDialogService : IRegionDialogService
    {
        private readonly IContainerExtension _containerExtension;
        private readonly IRegionManager _regionManager;

        public RegionDialogService(IContainerExtension containerExtension, IRegionManager regionManager)
        {
            _containerExtension = containerExtension;
            _regionManager = regionManager;
        

        public void Show(string regionName, string name)
        {
            var window = _containerExtension.Resolve<RibbonDialogWindow>();

            var scopedRegionManager = _regionManager.CreateRegionManager();
            RegionManager.SetRegionManager(window, scopedRegionManager);

            IRegion region = scopedRegionManager.Regions[regionName];

            region.ActiveViews.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (var view in args.NewItems)
                    {
                        IDialogAware dialogAware = ((FrameworkElement)view).DataContext as IDialogAware;

                        Action<IDialogResult> requestCloseHandler = null;
                        requestCloseHandler = (o) =>
                        {
                            window.Close();
                        };

                        //TODO: This is a problem
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

                            window.DataContext = null;
                            window.Content = null;
                        };
                        window.Closed += closedHandler;
                    }
                }
                else if (args.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (var view in args.OldItems)
                    {
                        IDialogAware dialogAware = ((FrameworkElement)view).DataContext as IDialogAware;

                        //TODO: we need this to happen
                        window.Closing -= closingHandler;
                    }
                }
            };


            scopedRegionManager.RequestNavigate(regionName, name);

            //var activeView = region.ActiveViews.FirstOrDefault() as FrameworkElement;
            //IDialogAware dialogAware = activeView.DataContext as IDialogAware;           


            //Action<IDialogResult> requestCloseHandler = null;
            //requestCloseHandler = (o) =>
            //{
            //    window.Close();
            //};

            //CancelEventHandler closingHandler = null;
            //closingHandler = (o, e) =>
            //{
            //    if (!dialogAware.CanCloseDialog())
            //        e.Cancel = true;
            //};
            //window.Closing += closingHandler;

            //RoutedEventHandler loadedHandler = null;
            //loadedHandler = (o, e) =>
            //{
            //    window.Loaded -= loadedHandler;
            //    dialogAware.RequestClose += requestCloseHandler;
            //};
            //window.Loaded += loadedHandler;

            //EventHandler closedHandler = null;
            //closedHandler = (o, e) =>
            //{
            //    window.Closed -= closedHandler;
            //    window.Closing -= closingHandler;

            //    window.DataContext = null;
            //    window.Content = null;
            //};
            //window.Closed += closedHandler;

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Show();
        }

        private void ActiveViews_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {

            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {

            }
        }
    }
}
