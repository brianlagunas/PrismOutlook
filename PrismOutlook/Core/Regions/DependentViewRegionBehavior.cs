using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace PrismOutlook.Core.Regions
{
    public class DependentViewRegionBehavior : RegionBehavior
    {
        public const string BehaviorKey = "DependentViewRegionBehavior";
        private readonly IContainerExtension _container;

        Dictionary<object, List<DependentViewInfo>> _dependentViewCache = new Dictionary<object, List<DependentViewInfo>>();

        public DependentViewRegionBehavior(IContainerExtension container)
        {
            _container = container;
        }

        protected override void OnAttach()
        {
            Region.ActiveViews.CollectionChanged += ActiveViews_CollectionChanged;
        }

        private void ActiveViews_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var newView in e.NewItems)
                {
                    var dependentViews = new List<DependentViewInfo>();

                    if (_dependentViewCache.ContainsKey(newView))
                    {
                        dependentViews = _dependentViewCache[newView];
                    }
                    else
                    {
                        var atts = GetCustomAttributes<DependentViewAttribute>(newView.GetType());
                        foreach (var att in atts)
                        {
                            var info = CreateDependentViewInfo(att);

                            if (info.View is ISupportDataContext infoDC && newView is ISupportDataContext viewDC)
                            {
                                infoDC.DataContext = viewDC.DataContext;
                            }

                            if (info.View is ISupportRichText infoRT && newView is ISupportRichText viewRT)
                            {
                                infoRT.RichTextEditor = viewRT.RichTextEditor;
                            }

                            dependentViews.Add(info);
                        }

                        _dependentViewCache.Add(newView, dependentViews);
                    }


                    dependentViews.ForEach(item => Region.RegionManager.Regions[item.Region].Add(item.View));
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach(var oldView in e.OldItems)
                {
                    if (_dependentViewCache.ContainsKey(oldView))
                    {
                        var dependentViews = _dependentViewCache[oldView];
                        dependentViews.ForEach(item => Region.RegionManager.Regions[item.Region].Remove(item.View));

                        if (!ShouldKeepAlive(oldView))
                            _dependentViewCache.Remove(oldView);
                    }
                }
            }
        }

        private bool ShouldKeepAlive(object oldView)
        {
            var regionLifetime = GetViewOrDataContextLifeTime(oldView);
            if (regionLifetime != null)
                return regionLifetime.KeepAlive;

            return true;
        }

        IRegionMemberLifetime GetViewOrDataContextLifeTime(object view)
        {
            if (view is IRegionMemberLifetime regionLifetime)
                return regionLifetime;

            if (view is FrameworkElement fe)
                return fe.DataContext as IRegionMemberLifetime;

            return null;
        }

        DependentViewInfo CreateDependentViewInfo(DependentViewAttribute attribute)
        {
            var info = new DependentViewInfo();
            info.Region = attribute.Region;
            info.View = _container.Resolve(attribute.Type);
            return info;
        }

        private static IEnumerable<T> GetCustomAttributes<T>(Type type)
        {
            return type.GetCustomAttributes(typeof(T), true).OfType<T>();
        }
    }
}
