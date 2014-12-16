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
            FileContent.Add("Time" + "\t" + "#Cohorts" + "\t" +  "AverageAge" + "\t" + "AverageB(kg/m2)" + "\t" + "AverageLAI(m2)" + "\t" + "AverageWater(mm)" + "\t" + "SubCanopyPAR(W/m2)" + "\t" + "Litter(kgDW/m2)" + "\t" + "WoodyDebris(kgDW/m2)");
        }
        public static void WriteNrOfCohortsBalance()
        {
            try
            {
                FileContent.Add(PlugIn.ModelCore.CurrentTime.ToString() + "\t" + SiteVars.Cohorts_sum + "\t" + Math.Round(SiteVars.CohortAge_av, 1) + "\t" + Math.Round(SiteVars.Biomass_av, 1) + "\t" + SiteVars.CanopyLAImax.Average<int>() + "\t" + SiteVars.Water.Average<int>() + "\t" + SiteVars.SubCanopyRadiation.Average<float>()  + "\t" + SiteVars.Litter.Average() + "\t" + SiteVars.WoodyDebris.Average());

                System.IO.File.WriteAllLines(FileName, FileContent.ToArray());
                 
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Cannot write to " +FileName +" "+ e.Message);
            }
            

        }
    }
    
}
