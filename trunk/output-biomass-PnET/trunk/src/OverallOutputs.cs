using System.Collections.Generic;
using System;
namespace Landis.Extension.Output.PnET
{
    
    class OverallOutputs
    {
        static List<string> FileContent = null;
        private static string FileName;

        public OverallOutputs(string Template)
        {
            
            FileName = FileNames.ReplaceTemplateVars(Template, "Overall", PlugIn.ModelCore.CurrentTime).Replace(".img", ".txt");
            FileContent = new List<string>();
            FileContent.Add("Time" + "\t" + "#Cohorts" + "\t" +  "AverageAge" + "\t" + "AverageB(g/m2)" + "\t" + "AverageLAI(m2)" + "\t" + "AverageWater(mm)" + "\t" + "SubCanopyPAR(W/m2)" + "\t" + "Litter(kgDW/m2)" + "\t" + "WoodyDebris(kgDW/m2)");
        }
        public static void WriteNrOfCohortsBalance()
        {
            try
            {
                string CohortAge_av = (SiteVars.Cohorts_sum >0) ? Math.Round(SiteVars.CohortAge_av, 1).ToString() : "n/a";

                FileContent.Add(PlugIn.ModelCore.CurrentTime.ToString() + "\t" + SiteVars.Cohorts_sum + "\t" + CohortAge_av + "\t" + SiteVars.CanopyLAImax.Average<byte>() + "\t" + SiteVars.Water.Average<ushort>() + "\t" + SiteVars.SubCanopyRadiation.Average<float>() + "\t" + SiteVars.Litter.Average() + "\t" + SiteVars.WoodyDebris.Average());

                System.IO.File.WriteAllLines(FileName, FileContent.ToArray());
                 
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Cannot write to " +FileName +" "+ e.Message);
            }
            

        }
    }
    
}
