//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Arjan de Bruijn 

using Landis.SpatialModeling;

namespace Landis.Extension.Output.PnET
{
    public class IntPixel : Pixel
    {
        public Band<int> MapCode  = "The numeric code for each raster cell";

        public IntPixel()
        {
            SetBands(MapCode);
        }
    }
}
