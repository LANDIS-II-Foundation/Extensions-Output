using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Landis.Library.Metadata;
using Landis.Core;



namespace Landis.Extension.Output.LeafBiomass
{
    public class SppBiomassLogLandscape
    {

        [DataFieldAttribute(Unit = FieldUnits.Year, Desc = "Simulation Year")]
        public int Time {set; get;}

        //[DataFieldAttribute(Desc = "Ecoregion Name")]
        //public string Ecoregion { set; get; }

        //[DataFieldAttribute(Desc = "Ecoregion Index")]
        //public int EcoregionIndex { set; get; }

        //[DataFieldAttribute(Unit = FieldUnits.Count, Desc = "Number of Sites")]
        //public int NumSites { set; get; }

        [DataFieldAttribute(Unit = FieldUnits.g_B_m2, Desc = "Species Biomass", Format = "0.0")]
        public double SppBiomass { set; get; }

    }
}
