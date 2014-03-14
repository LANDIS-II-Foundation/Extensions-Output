using Landis.Core;
using Landis.SpatialModeling;
using Landis.Library.BiomassCohortsPnET;
using System.Collections.Generic;
using Edu.Wisc.Forest.Flel.Util;
using System.IO;
using System;
namespace Landis.Extension.Output.BiomassPnET
{
    
    class OverallOutputs
    {
        static List<string> FileContent = null;
        private static string FileName;

        public OverallOutputs(string Template)
        {
            FileName = FileNames.MakeValueTableName(Template,"Overall"); 
             
            FileContent = new List<string>();
            FileContent.Add("Time" + "\t" + "#Cohorts" + "\t" + "#DeadCohorts" + "\t"+ "#NewCohorts" + "\t"+ "combinedcohorts" +"\t"+ "AverageB(kg/m2)" + "\t"+ "AverageLAI(m2)" + "\t"+ "AverageWater(mm)" + "\t"+ "SubCanopyPAR(W/m2)" + "\t"+ "litter");
        }
        public static void WriteNrOfCohortsBalance()
        {
            try
            {
                int newcohorts = SiteVars.GetTotal(SiteVars.NewCohorts);
                int combinedcohorts = SiteVars.GetTotal(SiteVars.CombinedCohorts);
                int deadcohorts = SiteVars.GetTotal(SiteVars.DeadCohorts);
                int noc = SiteVars.GetTotal(SiteVars.GetNrOfCohorts()) + deadcohorts;
                float B = SiteVars.GetAverage(SiteVars.GetBiomass());
                float W = SiteVars.GetAverage(SiteVars.SoilWater);
                float lai = SiteVars.GetAverage(SiteVars.CanopyLAImax);
                float scp = SiteVars.GetAverage(SiteVars.SubCanopyPARmax);
                double litter = SiteVars.GetAverage(SiteVars.Litter);

                FileContent.Add(PlugIn.ModelCore.CurrentTime.ToString() + "\t" + noc + "\t" + deadcohorts + "\t" + newcohorts + "\t" + combinedcohorts + "\t" + B + "\t" + lai + "\t" + W + "\t" + scp + "\t" + litter);

                System.IO.File.WriteAllLines(FileName, FileContent.ToArray());
                 
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Cannot write to " +FileName +" "+ e.Message);
            }
            

        }
    }
    
}
