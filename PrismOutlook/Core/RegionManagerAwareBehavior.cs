using Prism.Regions;
using System;
using System.Collections.Specialized;
using System.Windows;

namespace PrismOutlook.Core
{
    public class RegionManagerAwareBehavior : RegionBehavior
    {
        public const String BehaviorKey = "RegionManagerAwareBehavior";

        protected override void OnAttach()
        {
            Region.ActiveViews.CollectionChanged += ActiveViews_CollectionChanged;
        }

        void ActiveViews_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    InvokeOnRegionManagerAwareElement(item, x => x.RegionManager = Region.RegionManager);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    InvokeOnRegionManagerAwareElement(item, x => x.RegionManager = null);
                }
            }
        }

        private static void InvokeOnRegionManagerAwareElement(object item, Action<IRegionManagerAware> invocation)
        {
            var regionManagerAwareItem = item as IRegionManagerAware;
            if (regionManagerAwareItem != null)
            {
                invocation(regionManagerAwareItem);
            }

            FrameworkElement frameworkElement = item as FrameworkElement;
            if (frameworkElement != null)
            {
                IRegionManagerAware regionManagerAwareDataContext = frameworkElement.DataContext as IRegionManagerAware;
                if (regionManagerAwareDataContext != null)
                {
                    invocation(regionManagerAwareDataContext);
                }
            }
        }
    }
}
