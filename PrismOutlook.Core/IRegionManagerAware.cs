using Prism.Regions;

namespace PrismOutlook.Core
{
    public interface IRegionManagerAware
    {
        IRegionManager RegionManager { get; set; }
    }
}
