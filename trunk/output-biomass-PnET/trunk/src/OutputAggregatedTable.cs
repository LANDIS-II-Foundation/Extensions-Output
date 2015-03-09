using System.Collections.Generic;
using System;
using System.Linq;
using Landis.SpatialModeling;

namespace Landis.Extension.Output.PnET
{
    
    class OutputAggregatedTable
    {
        static List<string> FileContent = null;
        private static string FileName;

        public OutputAggregatedTable(string Template)
        {
            
            FileName = FileNames.ReplaceTemplateVars(Template, "Overall", PlugIn.ModelCore.CurrentTime).Replace(".img", ".txt");
            FileContent = new List<string>();
            FileContent.Add("Time" + "\t" + "#Cohorts" + "\t" +  "AverageAge" + "\t" + "AverageB(g/m2)" + "\t" + "AverageLAI(m2)" + "\t" + "AverageWater(mm)" + "\t" + "SubCanopyPAR(W/m2)" + "\t" + "Litter(kgDW/m2)" + "\t" + "WoodyDebris(kgDW/m2)");
        }
        public static void WriteNrOfCohortsBalance()
        {
            try
            {
                 
                string CohortBiom_av = PlugIn.cohorts.GetIsiteVar(x => x.BiomassSum).Average().ToString();
                string CohortAge_av = PlugIn.cohorts.GetIsiteVar(x => x.AverageAge).Average().ToString();
                string CohortLAI_av = PlugIn.cohorts.GetIsiteVar(x => x.CanopyLAImax).Average().ToString();
                string Water_av = PlugIn.cohorts.GetIsiteVar(x => x.Water).Average().ToString();
                string SubCanopyRad_av = PlugIn.cohorts.GetIsiteVar(x => x.SubCanopyParMAX).Average().ToString();
                string Litter_av = PlugIn.cohorts.GetIsiteVar(x => x.Litter).Average().ToString();
                string Woody_debris_ave = PlugIn.cohorts.GetIsiteVar(x => x.WoodyDebris).Average().ToString();
                string c = PlugIn.cohorts.GetIsiteVar(x => x.CohortCount).Average().ToString();

                FileContent.Add(PlugIn.ModelCore.CurrentTime.ToString() + "\t" + c + "\t" + CohortAge_av + "\t" + CohortBiom_av + "\t" + CohortLAI_av + "\t" + Water_av + "\t" + SubCanopyRad_av + "\t" + Litter_av + "\t" + Woody_debris_ave);

                System.IO.File.WriteAllLines(FileName, FileContent.ToArray());
                 
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Cannot write to " +FileName +" "+ e.Message);
            }
            

        }
    }
    
}
