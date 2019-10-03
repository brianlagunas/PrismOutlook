using Prism.Services.Dialogs;
using System;

namespace PrismOutlook.Core
{
    public interface IRegionDialogService
    {
        void Show(string name, IDialogParameters dialogParameters, Action<IDialogResult> callback);
    }
}
