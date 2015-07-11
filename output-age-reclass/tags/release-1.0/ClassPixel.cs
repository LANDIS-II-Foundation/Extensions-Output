using Landis.RasterIO;

namespace Landis.Output.Reclass
{
	public class ClassPixel
		: RasterIO.SingleBandPixel<byte>
	{
		//---------------------------------------------------------------------

		public ClassPixel()
			: base()
		{
		}

		//---------------------------------------------------------------------

		public ClassPixel(byte band0)
			: base(band0)
		{
		}
	}
}
