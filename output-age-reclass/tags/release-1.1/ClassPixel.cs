//  Copyright 2005 University of Wisconsin-Madison
//  Authors:  Jimm Domingo
//  License:  Available at  
//  http://landis.forest.wisc.edu/developers/LANDIS-IISourceCodeLicenseAgreement.pdf

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
