using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Landis.Library.Metadata;
using Landis.Core;



namespace Landis.Extension.Output.LeafBiomass
{
    public class SppBiomassLog
    {
        //log.Write("Time, Ecoregion, NumSites,");

        [DataFieldAttribute(Unit = FiledUnits.Year, Desc = "...")]
        public int Time {set; get;}

        [DataFieldAttribute(Unit = FiledUnits.None, Desc = "Ecoregion")]
        public string Ecoregion { set; get; }

        [DataFieldAttribute(Unit = FiledUnits.None, Desc = "Number of Sites")]
        public int NumSites { set; get; }

        [DataFieldAttribute(Unit = FiledUnits.None, Desc = "Species")]
        public string Species { set; get; }

        [DataFieldAttribute(Unit = FiledUnits.g_C_m_2, Desc = "Species Biomass", Format="0.00")]
        public double Biomass { set; get; }

    }
}
