using Landis.Core;
using Landis.Library.Parameters.Species;
using System.Collections.Generic;

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
        public static void Write<T>(string MapNameTemplate, string units, int TStep, AuxParm<T> Values_spc)
        {
            string FileName = FileNames.ReplaceTemplateVars(MapNameTemplate).Replace(".img", ".txt");

            if (PlugIn.ModelCore.CurrentTime  == 0)
            {
                FileNames.MakeFolders(FileName);
                System.IO.File.WriteAllLines(FileName, new string[] { Header(units) });
            }
           
            string line = TStep + "\t";
            foreach (ISpecies spc in PlugIn.ModelCore.Species)
            {
                line += Values_spc[spc] + "\t";
            }
            System.IO.StreamWriter sw = new System.IO.StreamWriter(FileName, true);
            sw.WriteLine(line);
            sw.Close();
             
   
        }
    }
}
