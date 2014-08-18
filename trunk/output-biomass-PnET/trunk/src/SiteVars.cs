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
        public static ISiteVar<List<Landis.Extension.Succession.BiomassPnET.Cohort>> cohorts;
        public static ISiteVar<AuxParm<int>> deadcohortcount;
        public static ISiteVar<AuxParm<int>> newcohortcount;
        public static ISiteVar<AuxParm<int>> lumpedcohortcount;
        public static ISiteVar<AuxParm<List<int>>> DeadCohortAges;
        public static ISiteVar<float> SubCanopyRadiation;
        public static ISiteVar<Landis.Library.Biomass.Pool> WoodyDebris;
        public static ISiteVar<Landis.Library.Biomass.Pool> Litter;
        public static ISiteVar<int> AnnualTranspiration;
        public static ISiteVar<int> CanopyLAImax;
        public static ISiteVar<int> Water;

        
        public static void Initialize()
        {
            WoodyDebris = GetSiteVar<Landis.Library.Biomass.Pool>("Succession.WoodyDebris");
            Litter = GetSiteVar<Landis.Library.Biomass.Pool>("Succession.Litter"); 
            deadcohortcount = GetSiteVar<Landis.Library.Parameters.Species.AuxParm<int>>("Succession.DeadCohortCount");
            newcohortcount = GetSiteVar<Landis.Library.Parameters.Species.AuxParm<int>>("Succession.NewCohortCount");
            lumpedcohortcount = GetSiteVar<Landis.Library.Parameters.Species.AuxParm<int>>("Succession.LumpedCohortCount");
            DeadCohortAges = GetSiteVar<Landis.Library.Parameters.Species.AuxParm<List<int>>>("Succession.DeadCohortAges");
            cohorts = GetSiteVar<List<Landis.Extension.Succession.BiomassPnET.Cohort>>("Succession.CohortsPnET");
            SubCanopyRadiation = GetSiteVar<float>("Succession.SubCanopyRadiation");
            AnnualTranspiration = GetSiteVar<int>("Succession.AnnualTranspiration");
            Water = GetSiteVar<int>("Succession.SoilWater");
            CanopyLAImax = GetSiteVar<int>("Succession.CanopyLAImax");

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

        public static ISiteVar<int> ToInt<T>(ISiteVar<T> values)
        {
            ISiteVar<int> intvalues = PlugIn.ModelCore.Landscape.NewSiteVar<int>();

            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                intvalues[site] = (int)double.Parse(values[site].ToString());
            }
            return intvalues;
        }
        private static List<T> ToList<T>(ISiteVar<T> values)
        {
            List<T> list_of_values = new List<T>();
            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                T d = values[site];
                list_of_values.Add(values[site]);
            }
            return list_of_values;
        }
         
        public static int Deadcohorts_sum
        {
            get
            {
                int deadcohorts_sum;
                deadcohorts_sum = 0;
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    foreach (ISpecies spc in PlugIn.ModelCore.Species)
                    {
                        deadcohorts_sum += deadcohortcount[site][spc];
                    }
                }
                return deadcohorts_sum;
            }
        }

        public static double WoodyDebrisAv
        {
            get
            {
                return ToList<Landis.Library.Biomass.Pool>(WoodyDebris).Average(o => o.Mass );
            }       
        }
        
        public static double Subcanopypar_av
        {
            get
            {
                return ToList<float>(SubCanopyRadiation).Average();
            }
        }


        public static Landis.Library.Parameters.Species.AuxParm<int> Establishments_spc
        {
            get
            {
                Landis.Library.Parameters.Species.AuxParm<int> establishments_spc = new  AuxParm<int>(PlugIn.ModelCore.Species);

                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    foreach (ISpecies spc in PlugIn.ModelCore.Species)
                    {
                        establishments_spc[spc] += newcohortcount[site][spc];
                    }
                }

                return establishments_spc;
            }
        }

        public static float Establishments_avg
        {
            get
            {

                return Establishments_sum / (float)PlugIn.ModelCore.Species.Count;
            }
        }
        
        public static int Establishments_sum
        {
            get
            {
                int sum = 0; 
                 

                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    foreach (ISpecies spc in PlugIn.ModelCore.Species)
                    {
                        sum += newcohortcount[site][spc];
                    }
                }

                return sum;
            }
        }
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
        public static double LitterAv
        {
            get
            {
                 
                return ToList<Landis.Library.Biomass.Pool>(Litter).Average(o => o.Mass);
            }
        }
        public static double Lai_av
        {
            get
            {
                return ToList<int>(CanopyLAImax).Average();

            }
        }
        public static double Water_av
        {
            get
            {
                return ToList<int>(Water).Average();
            }
        }
        public static Landis.Library.Parameters.Species.AuxParm<int> Newcohorts_Spc
        {
            get
            {
                AuxParm<int> newcohorts_spc = new  AuxParm<int>(PlugIn.ModelCore.Species);

                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    foreach (ISpecies spc in PlugIn.ModelCore.Species)
                    {
                        newcohorts_spc[spc] += newcohortcount[site][spc];
                    }
                }
                return newcohorts_spc;
            }
        }


        public static int LumpedCohorts_sum
        {
            get
            {
                int LumpedCohorts_sum = 0;

                List<Landis.Library.Parameters.Species.AuxParm<int>> LumpedCohorts = ToList<Landis.Library.Parameters.Species.AuxParm<int>>(lumpedcohortcount);

                foreach (Landis.Library.Parameters.Species.AuxParm<int> s in LumpedCohorts)
                {
                    foreach (ISpecies spc in PlugIn.ModelCore.Species)
                    {
                        LumpedCohorts_sum += s[spc];
                    }
                }

                return LumpedCohorts_sum;
            }
        }
        public static int NewCohorts_sum
        {
            get
            {
                int newcohorts_sum = 0;

                List<Landis.Library.Parameters.Species.AuxParm<int>> newcohorts = ToList<Landis.Library.Parameters.Species.AuxParm<int>>(newcohortcount);

                foreach (Landis.Library.Parameters.Species.AuxParm<int> s in newcohorts)
                {
                    foreach (ISpecies spc in PlugIn.ModelCore.Species)
                    {
                        newcohorts_sum += s[spc];
                    }
                }
                
                return newcohorts_sum;
            }
        }

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
        
        public static Landis.Library.Parameters.Species.AuxParm<int> Deadcohorts_spc
        {
            get
            {
                 AuxParm<int> deadcohorts_spc = new  AuxParm<int>(PlugIn.ModelCore.Species);
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    foreach (ISpecies spc in PlugIn.ModelCore.Species)
                    {
                        deadcohorts_spc[spc] += deadcohortcount[site][spc];
                    }
                }
                return deadcohorts_spc;
            }
        }
        public static ISiteVar<Landis.Library.Parameters.Species.AuxParm<int>> Deadcohorts
        {
            get
            {
                ISiteVar<Landis.Library.Parameters.Species.AuxParm<int>> deadcohorts = PlugIn.ModelCore.Landscape.NewSiteVar<Landis.Library.Parameters.Species.AuxParm<int>>();
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    deadcohorts[site] = new  AuxParm<int>(PlugIn.ModelCore.Species);
                }
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    foreach (ISpecies spc in PlugIn.ModelCore.Species)
                    {
                        deadcohorts[site][spc] += deadcohortcount[site][spc];
                    }
                }
                return deadcohorts;
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
        
        public static ISiteVar<Landis.Library.Parameters.Species.AuxParm<int>> Cohorts
        {
            get
            {
                ISiteVar<Landis.Library.Parameters.Species.AuxParm<int>> _cohorts = PlugIn.ModelCore.Landscape.NewSiteVar<Landis.Library.Parameters.Species.AuxParm<int>>();

                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    _cohorts[site] = new  AuxParm<int>(PlugIn.ModelCore.Species);
                    foreach (Landis.Library.BiomassCohortsPnET.Cohort cohort in cohorts[site])   
                    {
                        _cohorts[site][cohort.Species]++;
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

                foreach(Landis.Extension.Succession.BiomassPnET.Cohort cohort in  ToList<Landis.Extension.Succession.BiomassPnET.Cohort>(cohorts))
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
                return ToList<Landis.Extension.Succession.BiomassPnET.Cohort>(cohorts).Average(o => o.Age);

            }
        }
        public static double Cohorts_sum
        {
            get
            {
                return ToList<Landis.Extension.Succession.BiomassPnET.Cohort>(cohorts).Count;
                
            }
        }
        public static double Cohorts_avg
        {
            get
            {
                return ToList<Landis.Extension.Succession.BiomassPnET.Cohort>(cohorts).Count()/ (float)PlugIn.ModelCore.Landscape.ActiveSiteCount;
                 
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
                List<Landis.Extension.Succession.BiomassPnET.Cohort> lcohorts = ToList<Landis.Extension.Succession.BiomassPnET.Cohort>(cohorts);
                List<double> Biomass= new List<double>();
                foreach (Landis.Extension.Succession.BiomassPnET.Cohort cohort in lcohorts)
                {
                    Biomass.Add(cohort.Biomass);
                }
                return Biomass.Sum();
 
            }
        }
        public static ISiteVar<Landis.Library.Parameters.Species.AuxParm<int>> Biomass
        {
            get
            {
                // Average g/m2 per species
                ISiteVar<Landis.Library.Parameters.Species.AuxParm<int>> biomass = PlugIn.ModelCore.Landscape.NewSiteVar<Landis.Library.Parameters.Species.AuxParm<int>>();
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    biomass[site] = new AuxParm<int>(PlugIn.ModelCore.Species);
                    foreach (Landis.Library.BiomassCohortsPnET.Cohort cohort in cohorts[site])     
                    {
                        biomass[site][cohort.Species] += cohort.Biomass;
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
