namespace PrismOutlook.Core
{
    public interface IRegionDialogService
    {
        void Show(string regionName, string name);
    }

    public static class RegionDialogServiceExtensions
    {
        public static void ShowRibbonDialog(this IRegionDialogService service, string name)
        {
            service.Show(RegionNames.ContentRegion, name);
        }
    }
}
