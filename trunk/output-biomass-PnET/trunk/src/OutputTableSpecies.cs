using System.Collections.Generic;
using Landis.Core;
using Landis.Library.Parameters.Species;

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
        public void WriteUpdate(int year, AuxParm<int> values)
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
