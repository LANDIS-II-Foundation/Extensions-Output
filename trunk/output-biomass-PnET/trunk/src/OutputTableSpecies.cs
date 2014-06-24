using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Landis.Core;
using Landis.SpatialModeling;

namespace Landis.Extension.Output.PnET
{

    public class OutputTableSpecies
    {
        List<string> FileContent = new List<string>();
        string FileName;

        public OutputTableSpecies(string MapNameTemplate)
        {
            FileName = FileNames.ReplaceTemplateVars(MapNameTemplate).Replace(".img", ".txt").Replace(".gis", ".txt");
            FileNames.MakeFolders(FileName);

            string hdr = "time\t";
            foreach (ISpecies species in PlugIn.ModelCore.Species) hdr += species.Name + "\t";

            FileContent.Add(hdr);
        }
        public void WriteUpdate(int year, Landis.Library.Biomass.Species.AuxParm<int> values)
        {
            string line = year + "\t";
            foreach (ISpecies species in PlugIn.SelectedSpecies)
            {
                line += values[species] + "\t";
            }

            FileContent.Add(line);

             
            System.IO.File.WriteAllLines(FileName, FileContent.ToArray());

        }
        
    }
}
