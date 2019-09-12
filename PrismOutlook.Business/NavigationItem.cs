using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PrismOutlook.Business
{
    public class NavigationItem
    {
        public string Caption { get; set; }
        public string NavigationPath { get; set; }

        public bool IsExpanded { get; set; }

        public ObservableCollection<NavigationItem> Items { get; set; }

        public NavigationItem()
        {
            Items = new ObservableCollection<NavigationItem>();
        }
    }
}
