//using Landis.RasterIO;
using Wisc.Flel.GeospatialModeling.RasterIO;
namespace Landis.Output.Biomass
{
    public class BiomassPixel
        : SingleBandPixel<ushort>
    {
        public BiomassPixel()
            : base()
        {
        }

        //---------------------------------------------------------------------

        public BiomassPixel(ushort band0)
            : base(band0)
        {
        }
    }
}
