//  Copyright 2005-2006 University of Wisconsin-Madison
//  Authors:  Jimm Domingo, Robert M. Scheller
//  License:  Available at  
//  http://landis.forest.wisc.edu/developers/LANDIS-IISourceCodeLicenseAgreement.pdf

using Landis.RasterIO;

namespace Landis.Output.BiomassReclass
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
