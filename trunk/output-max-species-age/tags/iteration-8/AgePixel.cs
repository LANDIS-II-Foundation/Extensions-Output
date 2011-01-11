using Landis.Raster;

namespace Landis.Output.MaxSpeciesAge
{
	public class AgePixel
		: Raster.Pixel
	{
		private PixelBandUShort band0;

		//---------------------------------------------------------------------

		public ushort Band0
		{
			get {
				return band0;
			}
			set {
				band0.Value = value;
			}
		}

		//---------------------------------------------------------------------

		private void InitializeBands()
		{
			band0 = new PixelBandUShort();
			SetBands(band0);
		}

		//---------------------------------------------------------------------

		public AgePixel()
		{
			InitializeBands();
		}

		//---------------------------------------------------------------------

		public AgePixel(ushort band0)
		{
			InitializeBands();
			Band0 = band0;
		}
	}
}
