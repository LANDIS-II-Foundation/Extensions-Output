using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Landis.Library.Metadata;
using Landis.Core;



namespace Landis.Extension.Output.LeafBiomassReclass
{
    public class MapDefLog
    {

        [DataFieldAttribute(Unit = FieldUnits.Year, Desc = "Simulation Year")]
        public int Time {set; get;}

        [DataFieldAttribute(Unit = FieldUnits.Count, Desc = "Number of Cells", ColumnList = true)]
        public double[] ForestTypeCnt_ { set; get; }

    }
}
