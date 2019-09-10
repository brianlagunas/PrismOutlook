using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismOutlook.Modules.Contacts.ViewModels
{
    public class ViewAViewModel : BindableBase, IRegionMemberLifetime
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public bool KeepAlive => false;

        public ViewAViewModel()
        {
            Message = "View A from your Prism Module";
        }
    }
}
