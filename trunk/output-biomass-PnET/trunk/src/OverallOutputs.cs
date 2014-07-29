using Landis.Core;
using Landis.SpatialModeling;
using Landis.Library.BiomassCohortsPnET;
using System.Collections.Generic;
using Edu.Wisc.Forest.Flel.Util;
using System.IO;
using System;
namespace Landis.Extension.Output.PnET
{
    
    class OverallOutputs
    {
        static List<string> FileContent = null;
        private static string FileName;

        public OverallOutputs(string Template)
        {
            FileName = FileNames.MakeValueTableName(Template,"Overall"); 
             
            FileContent = new List<string>();
            FileContent.Add("Time" + "\t" + "#Cohorts" + "\t" + "#DeadCohorts" + "\t" + "#NewCohorts" + "\t" + "AverageAge" + "\t" + "AverageB(kg/m2)" + "\t" + "AverageLAI(m2)" + "\t" + "AverageWater(mm)" + "\t" + "SubCanopyPAR(W/m2)" + "\t" + "Litter(kgDW/m2)" + "\t" + "WoodyDebris(kgDW/m2)");
        }
        public static void WriteNrOfCohortsBalance()
        {
            try
            {
                FileContent.Add(PlugIn.ModelCore.CurrentTime.ToString() + "\t" + SiteVars.Cohorts_sum + "\t" + SiteVars.Deadcohorts_sum + "\t" + SiteVars.NewCohorts_sum + "\t" + Math.Round(SiteVars.CohortAge_av, 1) + "\t" + Math.Round(SiteVars.Biomass_av, 1) + "\t" + SiteVars.Lai_av + "\t" + SiteVars.Water_av + "\t" + SiteVars.Subcanopypar_av + "\t" + SiteVars.LitterAv + "\t" + "SiteVars.WoodyDebrisAv");

                System.IO.File.WriteAllLines(FileName, FileContent.ToArray());
                 
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Cannot write to " +FileName +" "+ e.Message);
            }
            

        }
    }
    
}
