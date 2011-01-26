//  Copyright 2005 University of Wisconsin-Madison
//  Authors:  Jimm Domingo
//  License:  Available at  
//  http://landis.forest.wisc.edu/developers/LANDIS-IISourceCodeLicenseAgreement.pdf

//using Landis.RasterIO;
using Wisc.Flel.GeospatialModeling.RasterIO;

namespace Landis.Extension.Output.Reclass
{
	public class ClassPixel
        : Wisc.Flel.GeospatialModeling.RasterIO.SingleBandPixel<byte>
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
