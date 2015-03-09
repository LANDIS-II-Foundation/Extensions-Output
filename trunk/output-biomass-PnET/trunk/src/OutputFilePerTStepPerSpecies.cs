using Landis.Core;
using Landis.Library.Parameters.Species;
using System.Collections.Generic;
using Landis.SpatialModeling;

namespace Landis.Extension.Output.PnET
{
    public static class OutputFilePerTStepPerSpecies
    {
        static string Header(string units)
        {

            string hdr = "Time" + "\t";
            foreach (ISpecies spc in PlugIn.ModelCore.Species)
            {
                hdr += spc.Name + "\t";
            }
            return hdr;
            
        }
        public static void Write<T>(string MapNameTemplate, string units, int TStep, ISiteVar<Landis.Library.Parameters.Species.AuxParm<int>> Values)
        {
            string FileName = FileNames.ReplaceTemplateVars(MapNameTemplate).Replace(".img", ".txt");

            if (PlugIn.ModelCore.CurrentTime  == 0)
            {
                FileNames.MakeFolders(FileName);
                System.IO.File.WriteAllLines(FileName, new string[] { Header(units) });
            }

            AuxParm<int> Values_spc = new AuxParm<int>(PlugIn.ModelCore.Species);

            string line = TStep + "\t";

            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                foreach (ISpecies spc in PlugIn.ModelCore.Species)
                {
                    Values_spc[spc] += Values[site][spc];
                }
            }
            System.IO.StreamWriter sw = new System.IO.StreamWriter(FileName, true);
            sw.WriteLine(line);
            sw.Close();
             
   
        }
    }
}
