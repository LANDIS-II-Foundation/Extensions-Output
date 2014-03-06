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
        private static FileProps f;
        private static string FileName;

        public static double SumSiteVar(ISiteVar<Landis.Library.Biomass.Pool> sitevar)
        {
            double total = 0;
             
            foreach (ActiveSite site in PlugIn.ModelCore.Landscape) total += sitevar[site].Mass;
             
            return total;
        }
        public static int SumSiteVar(ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> sitevar)
        {
            int total = 0;
            foreach (ISpecies species in PlugIn.ModelCore.Species)
            {
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape) total += sitevar[site][species];
            }
            return total;
        }
        public static float SumSiteVar(ISiteVar<float> sitevar)
        {
            float total = 0;
             
            foreach (ActiveSite s in PlugIn.ModelCore.Landscape)
            {
                float v = sitevar[s];
                total += v;
            
            }
            return total;
        }
        public static int SumSiteVar(ISiteVar<int> sitevar)
        {
            int total = 0;
            foreach (ActiveSite s in PlugIn.ModelCore.Landscape)
            {
                total += sitevar[s];
            }
            return total;
        }
        public static float AverageSiteVar(ISiteVar<float> sitevar)
        {
            float n = 0;
            foreach (ActiveSite s in PlugIn.ModelCore.Landscape)n++;
             
            return SumSiteVar(sitevar) / n;
        }
        public static double AverageSiteVar(ISiteVar<Landis.Library.Biomass.Pool> sitevar)
        {
            double n = 0;
            foreach (ActiveSite s in PlugIn.ModelCore.Landscape) n++;

            return SumSiteVar(sitevar) / n;
        }

        public static float AverageB()
        {
            float totalB = 0;
            int n = 0;
            foreach (ActiveSite s in PlugIn.ModelCore.Landscape)
            {
                if (SiteVars.Cohorts[s] == null) continue;
                foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[s])
                {
                    foreach (ICohort cohort in speciesCohorts)
                    {
                        totalB += cohort.Biomass;
                       
                    }
                }
                n++;
            }
            return totalB / (float)n;


        }
        
        public static int NumberOfCohorts()
        {
            int totalCohorts = 0;
            foreach (ActiveSite s in PlugIn.ModelCore.Landscape)
            {
                if (SiteVars.Cohorts[s] == null) continue;
                foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[s])
                {
                    foreach (ICohort cohort in speciesCohorts)
                    {
                        //System.Console.WriteLine("cohort" + cohort.Species.Name +"\t"+s);
                        totalCohorts++;
                    }
                }
            }
            return totalCohorts;
        }
        
        public OverallOutputs(string filename)
        {
            FileName = filename;
            f = new FileProps(FileProps.FileDelimiters.comma);

            FileContent = new List<string>();
            FileContent.Add("Time" + f.Delim + "#Cohorts" + f.Delim + "#DeadCohorts" + f.Delim + "#NewCohorts" + f.Delim + "AverageB(kg/m2)" + f.Delim + "AverageLAI(m2)" + f.Delim + "AverageWater(mm)" + f.Delim + "SubCanopyPAR(W/m2)" + f.Delim + "litter");
        }
        public static void WriteNrOfCohortsBalance()
        {
            try
            {
                int newcohorts = SumSiteVar(SiteVars.NewCohorts);
                int deadcohorts = SumSiteVar(SiteVars.DeadCohorts);
                int noc = NumberOfCohorts() + deadcohorts;
                float B = AverageB();
                float W = AverageSiteVar(SiteVars.SoilWater);
                float lai = AverageSiteVar(SiteVars.CanopyLAImax);
                float scp = AverageSiteVar(SiteVars.SubCanopyPARmax);
                double litter = AverageSiteVar(SiteVars.Litter);

                FileContent.Add(PlugIn.ModelCore.CurrentTime.ToString() + f.Delim + noc + f.Delim + deadcohorts + f.Delim + newcohorts + f.Delim + B + f.Delim + lai + f.Delim + W + f.Delim + scp + f.Delim + litter);

                System.IO.File.WriteAllLines(FileName, FileContent.ToArray());
                 
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Cannot write to " +FileName +" "+ e.Message);
            }
            

        }
    }
}
