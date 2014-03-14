//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Robert M. Scheller, James B. Domingo

using Landis.Core;
using Landis.Library.BiomassCohortsPnET;
using Landis.Extension.Succession.BiomassPnET;
using System.Collections.Generic;
using Landis.SpatialModeling;

namespace Landis.Extension.Output.BiomassPnET
{
    /// <summary>
    /// The pools of dead biomass for the landscape's sites.
    /// </summary>
    public static class SiteVars
    {

        private static ISiteVar<Landis.Library.Biomass.Pool> woodyDebris;
        private static ISiteVar<Landis.Library.Biomass.Pool> litter;
        private static ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> deadcohorts;
        private static ISiteVar<Landis.Library.Biomass.Species.AuxParm<List<int>>> deadcohortages;
        private static ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> newcohorts;
        private static ISiteVar<ISiteCohorts> cohorts;
        private static ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> establishments;
        
        private static ISiteVar<float> soilwater;
        private static ISiteVar<float> annualtranspiration;
        private static ISiteVar<float> canopylaimax;
        private static ISiteVar<float> subcanopyparmax;
        
        
        //---------------------------------------------------------------------

        /// <summary>
        /// Initializes the module.
        /// </summary>
        public static void Initialize()
        {
            deadcohorts = PlugIn.ModelCore.GetSiteVar <Landis.Library.Biomass.Species.AuxParm<int>>("Succession.DeadCohorts");
            deadcohortages = PlugIn.ModelCore.GetSiteVar<Landis.Library.Biomass.Species.AuxParm<List<int>>>("Succession.DeadCohortAges");
            newcohorts = PlugIn.ModelCore.GetSiteVar<Landis.Library.Biomass.Species.AuxParm<int>>("Succession.NewCohorts");

            woodyDebris = PlugIn.ModelCore.GetSiteVar<Landis.Library.Biomass.Pool>("Succession.WoodyDebris");
            litter = PlugIn.ModelCore.GetSiteVar<Landis.Library.Biomass.Pool>("Succession.Litter");
            establishments = PlugIn.ModelCore.GetSiteVar<Landis.Library.Biomass.Species.AuxParm<int>>("Succession.Establishments");
            cohorts = PlugIn.ModelCore.GetSiteVar<ISiteCohorts>("Succession.BiomassCohortsPnET");
            
            if (cohorts == null)
            {
                string mesg = string.Format("Cohorts are empty.  Please double-check that this extension is compatible with your chosen succession extension.");
                throw new System.ApplicationException(mesg);
            }
            annualtranspiration = PlugIn.ModelCore.GetSiteVar<float>("Succession.AnnualTranspiration");
            subcanopyparmax = PlugIn.ModelCore.GetSiteVar<float>("Succession.SubCanopyPARmax");
            canopylaimax = PlugIn.ModelCore.GetSiteVar<float>("Succession.CanopyLAImax");
            soilwater = PlugIn.ModelCore.GetSiteVar<float>("Succession.SoilWater");
        }
        public static Landis.Library.Biomass.Species.AuxParm<int> GetMaxCohortsPerSpc(ISiteVar<ISiteCohorts> cohorts)
        {
            // Max nr of cohorts per species across the sites
            Landis.Library.Biomass.Species.AuxParm<int> max = new Library.Biomass.Species.AuxParm<int>(PlugIn.ModelCore.Species);
            foreach (ISpecies spc in PlugIn.ModelCore.Species)   max[spc] = int.MinValue;

            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                foreach (ISpecies spc in PlugIn.ModelCore.Species)
                {
                    foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
                    {
                        if(speciesCohorts.Count > max[spc])max[spc]=speciesCohorts.Count;
                    }
                }
                
            }
            return max;
        }
        public static int GetMax(ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> deadcohorts)
        {
            int max = int.MinValue;
            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                foreach (ISpecies spc in PlugIn.ModelCore.Species)
                {
                    if (deadcohorts[site][spc] > max) max = deadcohorts[site][spc];
                }

            }
            return max;
        }

        public static int GetMax(Landis.Library.Biomass.Species.AuxParm<int> ValuesPerSpecies)
        {
            int max = int.MinValue;
            foreach (ISpecies spc in PlugIn.ModelCore.Species)
            {
                if (ValuesPerSpecies[spc] > max) max = ValuesPerSpecies[spc];
            }
            return max;
        }
        public static int GetMaxCohorts()
        { 
            // Max nr of cohorts of a species on a site
            return GetMax(GetMaxCohortsPerSpc(cohorts));
        
        }
        public static double SumSiteVar(ISiteVar<Landis.Library.Biomass.Pool> sitevar)
        {
            double total = 0;

            foreach (ActiveSite site in PlugIn.ModelCore.Landscape) total += sitevar[site].Mass;

            return total;
        }
        public static ISiteVar<Landis.Library.Biomass.Species.AuxParm<List<int>>> GetCohortAges()
        {
            ISiteVar<Landis.Library.Biomass.Species.AuxParm<List<int>>> ages = PlugIn.ModelCore.Landscape.NewSiteVar<Landis.Library.Biomass.Species.AuxParm<List<int>>>();

            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                ages[site] = new Library.Biomass.Species.AuxParm<List<int>>(PlugIn.ModelCore.Species);
                foreach (ISpecies species in PlugIn.ModelCore.Species)
                {
                    ages[site][species] = new List<int>();
                }
            }

            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                if (SiteVars.Cohorts[site] == null) continue;
                foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
                {
                    foreach (ICohort cohort in speciesCohorts)
                    {
                        ages[site][cohort.Species].Add(cohort.Age);
                    }
                }
            }
            return ages;
        }
        public static Landis.Library.Biomass.Species.AuxParm<int> SpeciesPerSiteToSpecies(ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> Values)
         {
             Library.Biomass.Species.AuxParm<int> Values_spc = new Library.Biomass.Species.AuxParm<int>(PlugIn.ModelCore.Species);
             foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
             {
                 foreach (ISpeciesCohorts spc in SiteVars.Cohorts[site])
                 {
                     foreach (ICohort cohort in spc)
                     {
                         Values_spc[cohort.Species] += Values[site][cohort.Species];
                     }
                 }
             }
             return Values_spc;
         }
        public static float GetAverage(ISiteVar<Landis.Library.Biomass.Pool> PerSite)
        {
            return GetTotal(PerSite) / PlugIn.ModelCore.Landscape.ActiveSiteCount;
        }
        public static float GetAverage(ISiteVar<float> PerSite)
        {
            return GetTotal(PerSite) / PlugIn.ModelCore.Landscape.ActiveSiteCount;
        }
        public static float GetAverage(ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> PerSitePerSpecies)
        {
            float n = 0;
            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                foreach (ISpecies spc in PlugIn.ModelCore.Species)
                {
                    n++;
                }
            }
            return GetTotal(PerSitePerSpecies)/n;
        }

        public static float GetTotal(ISiteVar<Landis.Library.Biomass.Pool> PerSite)
        {
            float total = 0;
            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                total += (float)PerSite[site].Mass;
            }
            return total;
        }
        public static float GetTotal(ISiteVar<float> PerSite)
        {
            float total = 0;
            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                total += PerSite[site];
            }
            return total;
        }
        public static int GetTotal(ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> PerSitePerSpecies)
        {
            int total = 0;
            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                foreach (ISpecies spc in PlugIn.ModelCore.Species)
                {
                    total += PerSitePerSpecies[site][spc];
                }
            }
            return total;
        }
        public static ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> GetNrOfCohorts()
        {
            ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> Values = PlugIn.ModelCore.Landscape.NewSiteVar<Landis.Library.Biomass.Species.AuxParm<int>>();

            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                Values[site] = new Library.Biomass.Species.AuxParm<int>(PlugIn.ModelCore.Species);
                foreach (ISpeciesCohorts spc in SiteVars.Cohorts[site])
                {
                    foreach (ICohort c in spc)
                    {
                        Values[site][c.Species]++;
                    }
                }
            }
            return Values;
        }

        public static ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> GetBiomass()
        {
            ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> Values = PlugIn.ModelCore.Landscape.NewSiteVar<Landis.Library.Biomass.Species.AuxParm<int>>();
            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                Values[site] = new Library.Biomass.Species.AuxParm<int>(PlugIn.ModelCore.Species);
                foreach (ISpeciesCohorts spc in SiteVars.Cohorts[site])
                {
                    foreach (ICohort cohort in spc)
                    {
                        Values[site][cohort.Species] += cohort.Biomass;
                    }
                }
            }
            return Values;
        }
        public static double AverageSiteVar(ISiteVar<Landis.Library.Biomass.Pool> sitevar)
        {
            double n = 0;
            foreach (ActiveSite s in PlugIn.ModelCore.Landscape) n++;

            return SumSiteVar(sitevar) / n;
        }


        public static ISiteVar<int> GetLAI()
        {
            ISiteVar<int> LAI = PlugIn.ModelCore.Landscape.NewSiteVar<int>();
            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                LAI[site] = (int)System.Math.Round(10 * SiteVars.CanopyLAImax[site], 0);
            }
            return LAI;
        }
        public static ISiteVar<int> GetBelowGroundBiomass()
        {
            ISiteVar<int> Biomass = PlugIn.ModelCore.Landscape.NewSiteVar<int>();
            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                foreach (ISpeciesCohorts spc in SiteVars.Cohorts[site])
                {
                    foreach (ICohort c in spc)
                    {
                        Biomass[site] += (int)System.Math.Round(c.Root);
                    }
                }
            }
            return Biomass;
        }
        public static ISiteVar<int> PoolToInt(ISiteVar<Landis.Library.Biomass.Pool> f)
        {
            ISiteVar<int> r = PlugIn.ModelCore.Landscape.NewSiteVar<int>();

            foreach (ActiveSite s in PlugIn.ModelCore.Landscape)
            {
                r[s] = (int)System.Math.Round(f[s].Mass, 0);
            }
            return r;
        }
        public static ISiteVar<int> FloatToInt(ISiteVar<float> f)
        {
            ISiteVar<int> r = PlugIn.ModelCore.Landscape.NewSiteVar<int>();

            foreach (ActiveSite s in PlugIn.ModelCore.Landscape)
            {
                r[s] = (int)System.Math.Round(f[s], 0);
            }
            return r;
        }
        //---------------------------------------------------------------------
        public static ISiteVar<ISiteCohorts> Cohorts
        {
            get
            {
                if (cohorts == null)
                {
                    string mesg = string.Format("Cohorts are empty.  Please double-check that this extension is compatible with your chosen succession extension.");
                    throw new System.ApplicationException(mesg);
                }
                return cohorts;
            }
        }

        public static ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> DeadCohorts
        {
            get
            {
                return deadcohorts;
            }
        }

        
        
        public static ISiteVar<Landis.Library.Biomass.Species.AuxParm<List<int>>> DeadCohortAges
        {
            get
            {
                return deadcohortages ;
            }
        }
        public static ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> NewCohorts
        {
            get
            {
                return newcohorts;
            }
        }
        public static ISiteVar<float> SubCanopyPARmax
        {
            get
            {
                return subcanopyparmax;
            }
        }
        public static ISiteVar<float> CanopyLAImax
        {
            get
            {
                return canopylaimax;
            }
        }
        public static ISiteVar<float> AnnualTranspiration
        {
            get
            {
                return annualtranspiration;
            }
        }
        public static ISiteVar<float> SoilWater
        {
            get
            {
                return soilwater;
            }
        }
        public static ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> Establishments
        {
            get
            {
                return establishments;
            }
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// The intact dead woody pools for the landscape's sites.
        /// </summary>
        public static ISiteVar<Landis.Library.Biomass.Pool> WoodyDebris
        {
            get {
                return woodyDebris;
            }
        }
        
        //---------------------------------------------------------------------
        /// <summary>
        /// The dead non-woody pools for the landscape's sites.
        /// </summary>
        public static ISiteVar<Landis.Library.Biomass.Pool> Litter
        {
            get {
                return litter;
            }
        }

    }
}
