//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Arjan de Bruijn 

using Landis.Core;
using System.Collections.Generic;
using Landis.SpatialModeling;
using System.Linq;
using Landis.Library.Parameters.Species; 
 

namespace Landis.Extension.Output.PnET
{
    /// <summary>
    /// The pools of dead biomass for the landscape's sites.
    /// </summary>
    public static class SiteVars
    {
        public static ISiteVar<List<Landis.Library.BiomassCohortsPnET.Cohort>> cohorts;
        public static ISiteVar<float> SubCanopyRadiation;
        public static ISiteVar<Landis.Library.Biomass.Pool> WoodyDebris;
        public static ISiteVar<Landis.Library.Biomass.Pool> Litter;
        public static ISiteVar<byte> CanopyLAImax;
        public static ISiteVar<ushort> Water;

       
        public static void Initialize()
        {
            WoodyDebris = GetSiteVar<Landis.Library.Biomass.Pool>("Succession.WoodyDebris");
            Litter = GetSiteVar<Landis.Library.Biomass.Pool>("Succession.Litter"); 
            cohorts = GetSiteVar<List<Landis.Library.BiomassCohortsPnET.Cohort>>("Succession.CohortsPnET");
            SubCanopyRadiation = GetSiteVar<float>("Succession.SubCanopyRadiation");
            Water = GetSiteVar<ushort>("Succession.SoilWater");
            CanopyLAImax = GetSiteVar<byte>("Succession.CanopyLAImax");

        }
        private static List<T> ToList<T>(ISiteVar<List<T>> values)
        {
            List<T> list_of_values = new List<T>();
            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                list_of_values.AddRange(values[site]);
            }
            return list_of_values;
        }
        private static ISiteVar<T> GetSiteVar<T>(string label)
        {
            ISiteVar<T> sitevar = PlugIn.ModelCore.GetSiteVar<T>(label);

            if (sitevar == null)
            {
                string mesg = string.Format( label + " is empty.  Please double-check that this extension is compatible with your chosen succession extension.");
                throw new System.ApplicationException(mesg);
            }

            return sitevar;
        }
        /*
        public static Landis.Library.Parameters.Ecoregions.AuxParm<float> AverageLAIperEcoRegion
        {
            get
            {
                Landis.Library.Parameters.Ecoregions.AuxParm<double> SumLAI = new Landis.Library.Parameters.Ecoregions.AuxParm<double>(PlugIn.ModelCore.Ecoregions);
                Landis.Library.Parameters.Ecoregions.AuxParm<float> n = new Landis.Library.Parameters.Ecoregions.AuxParm<float>(PlugIn.ModelCore.Ecoregions);

                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    SumLAI[PlugIn.ModelCore.Ecoregion[site]] += (double)CanopyLAImax[site];
                    n[PlugIn.ModelCore.Ecoregion[site]]++;
                }

                Landis.Library.Parameters.Ecoregions.AuxParm<float> AverageLAI = new Landis.Library.Parameters.Ecoregions.AuxParm<float>(PlugIn.ModelCore.Ecoregions);

                foreach (IEcoregion e in PlugIn.ModelCore.Ecoregions)
                {
                    AverageLAI[e] = (float)SumLAI[e] / n[e];
                }
                return AverageLAI;
            }
      
        
        
        }
          
        public static Landis.Library.Parameters.Ecoregions.AuxParm<float> AverageWaterPerEcoregion
        {
            get
            {
                Landis.Library.Parameters.Ecoregions.AuxParm<List<double>> SumWater = new Landis.Library.Parameters.Ecoregions.AuxParm<List<double>>(PlugIn.ModelCore.Ecoregions);
                
                foreach (IEcoregion e in PlugIn.ModelCore.Ecoregions)
                {
                    SumWater[e] = new List<double>();
                }
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    int w = (int)System.Math.Round((double)Water[site], 0);
                    SumWater[PlugIn.ModelCore.Ecoregion[site]].Add(w);
                
                }

                Landis.Library.Parameters.Ecoregions.AuxParm<float> AverageWater = new Landis.Library.Parameters.Ecoregions.AuxParm<float>(PlugIn.ModelCore.Ecoregions);
                
                foreach (IEcoregion e in PlugIn.ModelCore.Ecoregions)
                {
                    if (SumWater[e].Count > 0) AverageWater[e] = (float)SumWater[e].Average();
                    else AverageWater[e] = 0;
                }
                return AverageWater;
            }
        }
        */
        public static ISiteVar<Landis.Library.Parameters.Species.AuxParm<List<int>>> CohortAges
        {
            get
            {
                ISiteVar<Landis.Library.Parameters.Species.AuxParm<List<int>>> cohortages = PlugIn.ModelCore.Landscape.NewSiteVar<Landis.Library.Parameters.Species.AuxParm<List<int>>>();

                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    cohortages[site] = new  AuxParm<List<int>>(PlugIn.ModelCore.Species);
                     
                }
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    List<int> Ages = new List<int>();
                    foreach (Landis.Library.BiomassCohortsPnET.Cohort cohort in cohorts[site])   
                    {
                        Ages.Add(cohort.Age);
                        cohortages[site][cohort.Species] = Ages;//.ToArray();
                    }
                    
                }
                 

                return cohortages;
            }
        }
        public static ISiteVar<int> MaxAges
        {
            get
            {
                ISiteVar<int> maxages  = PlugIn.ModelCore.Landscape.NewSiteVar<int>();
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    maxages[site] = int.MinValue;
                    foreach (Landis.Library.BiomassCohortsPnET.Cohort cohort in cohorts[site])   
                    {
                        if (cohort.Age > maxages[site]) maxages[site] = cohort.Age;
                    }
                }
                return maxages;
            }
        }
         
        public static ISiteVar<int> BelowGroundBiomass
        {
            get
            {
                

                ISiteVar<int> belowgroundbiomass = PlugIn.ModelCore.Landscape.NewSiteVar<int>();
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    belowgroundbiomass[site] = (int)cohorts[site].Sum(o => o.Root);
                }
                return belowgroundbiomass;
            }
        }
        public static bool SpeciesIsPresent(ActiveSite site, ISpecies spc)
        {
            foreach (Landis.Library.BiomassCohortsPnET.Cohort cohort in cohorts[site])
            {
                if (cohort.species == spc) return true;
            }
            return false;
        }
        public static Landis.Library.Parameters.Species.AuxParm<ISiteVar<int>> Cohorts
        {
            get
            {
                Landis.Library.Parameters.Species.AuxParm<ISiteVar<int>> _cohorts = new AuxParm<ISiteVar<int>>(PlugIn.ModelCore.Species);

                foreach (ISpecies Species in PlugIn.ModelCore.Species)
                {
                    _cohorts[Species] = PlugIn.ModelCore.Landscape.NewSiteVar<int>();
                }
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    foreach (Landis.Library.BiomassCohortsPnET.Cohort cohort in cohorts[site])   
                    {
                        _cohorts[cohort.Species][site]++;
                    }
                }

                return _cohorts;
            }
        }
        public static ISiteVar<int> CohortsPerSite
        {
            get
            {
                ISiteVar<int> CohortsPerSite = PlugIn.ModelCore.Landscape.NewSiteVar<int>();

                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    foreach (Landis.Library.BiomassCohortsPnET.Cohort cohort in cohorts[site])   
                    {
                        CohortsPerSite[site]++;
                    }
                }


                return CohortsPerSite;
            }
        }
        public static Landis.Library.Parameters.Species.AuxParm<int> Cohorts_spc
        {
            get
            {
                AuxParm<int> cohorts_spc = new  AuxParm<int>(PlugIn.ModelCore.Species);

                foreach (Landis.Library.BiomassCohortsPnET.Cohort cohort in ToList<Landis.Library.BiomassCohortsPnET.Cohort>(cohorts))
                {
                    cohorts_spc[cohort.Species]++;
                }

                
                return cohorts_spc;
            }
        }

        public static double CohortAge_av
        {
            get
            {
                return ToList<Landis.Library.BiomassCohortsPnET.Cohort>(cohorts).Average(o => o.Age);

            }
        }
        public static double Cohorts_sum
        {
            get
            {
                return ToList<Landis.Library.BiomassCohortsPnET.Cohort>(cohorts).Count;
                
            }
        }
        public static double Cohorts_avg
        {
            get
            {
                return ToList<Landis.Library.BiomassCohortsPnET.Cohort>(cohorts).Count()/ (float)PlugIn.ModelCore.Landscape.ActiveSiteCount;
                 
            }
        }
        public static double Biomass_av
        {
            get
            {
                List<float> Biomass = new List<float>();

                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    if (cohorts[site].Count > 0) Biomass.Add(cohorts[site].Sum(o => o.Biomass));
                    else Biomass.Add(0);
                }

                return Biomass.Average();
            }
        }
        public static double Biomass_sum
        {
            get
            {
                List<Landis.Library.BiomassCohortsPnET.Cohort> lcohorts = ToList<Landis.Library.BiomassCohortsPnET.Cohort>(cohorts);
                List<double> Biomass= new List<double>();
                foreach (Landis.Library.BiomassCohortsPnET.Cohort cohort in lcohorts)
                {
                    Biomass.Add(cohort.Biomass);
                }
                return Biomass.Sum();
 
            }
        }
        public static Landis.Library.Parameters.Species.AuxParm<ISiteVar<int>> Biomass
        {
            get
            {
                // Average g/m2 per species
                Landis.Library.Parameters.Species.AuxParm<ISiteVar<int>> biomass = new AuxParm<ISiteVar<int>>(PlugIn.ModelCore.Species);

                foreach(ISpecies spc in PlugIn.ModelCore.Species)
                {
                    biomass[spc] = PlugIn.ModelCore.Landscape.NewSiteVar<int>();
                }

                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    foreach (Landis.Library.BiomassCohortsPnET.Cohort cohort in cohorts[site])     
                    {
                        biomass[cohort.Species][site] += cohort.Biomass;
                    }
                }
                return biomass;
            }
        }
        public static Landis.Library.Parameters.Species.AuxParm<float> Biomass_spc
        {
            get
            {
                // Average (g/m2) per species
                AuxParm<float> biomass_spc = new AuxParm<float>(PlugIn.ModelCore.Species);
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    foreach (Landis.Library.BiomassCohortsPnET.Cohort cohort in cohorts[site])
                    {
                        biomass_spc[cohort.Species] += cohort.Biomass;
                    }
                }
                
                foreach (ISpecies spc in PlugIn.ModelCore.Species)
                {
                    biomass_spc[spc] *= (float)(1F / (float)PlugIn.ModelCore.Landscape.ActiveSiteCount);
                }
                
                return biomass_spc;
            }
        }


       
        
    }
}
