using Landis.Core;
using Edu.Wisc.Forest.Flel.Util;
using Landis.Library.BiomassCohortsPnET;
using Landis.SpatialModeling;
using Landis.Extension.Succession.BiomassPnET;
using System;
using System.Collections.Generic;
using System.IO;
 
using Landis.Library.Parameters;

namespace Landis.Extension.Output.PnET
{
    class OutputTableEcoregions
    {
        List<string> FileContent = new List<string>();
        string FileName;


        public OutputTableEcoregions(string MapNameTemplate)
        {
            FileName = FileNames.ReplaceTemplateVars(MapNameTemplate).Replace(".img", "eco.txt").Replace(".gis", "eco.txt");
            FileNames.MakeFolders(FileName);

            string hdr = "time\t";
            foreach ( IEcoregion e in PlugIn.ModelCore.Ecoregions) hdr += e.Name + "\t";

            FileContent.Add(hdr);
        }
        public void WriteUpdate(int year, Landis.Library.Parameters.Ecoregions.AuxParm<float> values)
        {
            string line = year + "\t";

            foreach (IEcoregion e in PlugIn.ModelCore.Ecoregions)  
            {
                line += values[e] + "\t";
            }

            FileContent.Add(line);


            System.IO.File.WriteAllLines(FileName, FileContent.ToArray());

        }

         
    }
}
