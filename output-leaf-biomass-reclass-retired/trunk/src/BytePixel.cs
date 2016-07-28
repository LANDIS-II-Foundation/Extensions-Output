//  Copyright 2013 Portland State University
//  Authors:  Robert M. Scheller

using Landis.SpatialModeling;

namespace Landis.Extension.Output.LeafBiomassReclass
{
    public class BytePixel : Pixel
    {
        public Band<byte> MapCode  = "The numeric code for each ecoregion";

        public BytePixel() 
        {
            SetBands(MapCode);
        }
    }
}
