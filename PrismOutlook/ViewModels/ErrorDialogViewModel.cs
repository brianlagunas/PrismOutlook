using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace PrismOutlook.ViewModels
{
    internal class ErrorDialogViewModel : BindableBase, IDialogAware
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private DelegateCommand _closeDialogCommand;
        public DelegateCommand CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand(CloseDialog));

        private void CloseDialog()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Message = parameters.GetValue<string>("message");
        }

        public string Title { get; } = "Error";

        public event Action<IDialogResult> RequestClose;
    }
}
