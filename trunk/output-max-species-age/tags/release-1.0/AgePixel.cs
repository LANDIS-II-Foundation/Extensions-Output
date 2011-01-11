using Landis.RasterIO;

namespace Landis.Output.MaxSpeciesAge
{
	public class AgePixel
		: SingleBandPixel<ushort>
	{
		public AgePixel()
			: base()
		{
		}

		//---------------------------------------------------------------------

		public AgePixel(ushort band0)
			: base(band0)
		{
		}
	}
}
