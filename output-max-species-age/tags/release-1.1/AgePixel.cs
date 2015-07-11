//  Copyright 2005 University of Wisconsin-Madison
//  Authors:  Jimm Domingo, FLEL
//  License:  Available at  
//  http://landis.forest.wisc.edu/developers/LANDIS-IISourceCodeLicenseAgreement.pdf

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
