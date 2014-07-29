//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Robert M. Scheller, James B. Domingo

using Landis.Core;
using Landis.Library.BiomassCohortsPnET;
using Landis.Extension.Succession.BiomassPnET;
using System.Collections.Generic;
using Landis.SpatialModeling;

namespace Landis.Extension.Output.PnET
{
    /// <summary>
    /// The pools of dead biomass for the landscape's sites.
    /// </summary>
    public static class SiteVars
    {
        private static ISiteVar<Landis.Extension.Succession.BiomassPnET.ISiteCohorts> sitecohorts;
        private static ISiteVar<Landis.Library.Biomass.Pool> woodyDebris;
        private static ISiteVar<Landis.Library.Biomass.Pool> litter;

        public static ISiteVar<Landis.Library.Biomass.Pool> Litter
        {
            get
            {
                return litter;
            }
        }
        public static ISiteVar<Landis.Library.Biomass.Pool> WoodyDebris
        {
            get
            {
                return woodyDebris;
            }
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
                        deadcohorts_sum += sitecohorts[site].DeadCohorts[spc];
                    }
                }
                return deadcohorts_sum;
            }
        }

        public static double WoodyDebrisAv
        {
            get
            {
                return 0;
            }
            /*
            get
            {
                double litter_sum = 0;
                float n = 0;
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    litter_sum += siteconditions[site].forestfloor.WoodyDebris.Mass;
                    n++;
                }
                return System.Math.Round(litter_sum / n, 2);;
            }
             */
        }
        public static double LitterAv
        {
            get
            {
                double litter_sum = 0;
                float n = 0;
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    litter_sum += Litter[site].Mass;
                    n++;
                }
                return System.Math.Round(litter_sum / n, 2);;
            }
        }
        public static double Subcanopypar_av
        {
            get
            {
                double subcanopypar_sum = 0;
                float n = 0;
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    subcanopypar_sum += sitecohorts[site].SubCanopyPAR;
                    n++;
                }
                return System.Math.Round(subcanopypar_sum / n, 2);
            }
        }
        public static ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> Establishments
        {
            get
            {
                ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> establishments = PlugIn.ModelCore.Landscape.NewSiteVar<Landis.Library.Biomass.Species.AuxParm<int>>();
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    establishments[site] = new Library.Biomass.Species.AuxParm<int>(PlugIn.ModelCore.Species);
                    foreach (ISpecies spc in PlugIn.ModelCore.Species)
                    {
                        establishments[site][spc] = sitecohorts[site].Establishment.Establishments[spc];
                    }

                }
                return establishments;
            }
        }
        public static Landis.Library.Biomass.Species.AuxParm<int> Establishments_spc
        {
            get
            {
                Landis.Library.Biomass.Species.AuxParm<int> establishments_spc = new Library.Biomass.Species.AuxParm<int>(PlugIn.ModelCore.Species);

                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    foreach (ISpecies spc in PlugIn.ModelCore.Species)
                    {
                        establishments_spc[spc] += sitecohorts[site].Establishment.Establishments[spc];

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
                        sum += sitecohorts[site].Establishment.Establishments[spc];
                    }
                }

                return sum;
            }
        }
        

        public static ISiteVar<int> SubCanopyPARmax
        {
            get
            {
                ISiteVar<int> subcanopyparmax = PlugIn.ModelCore.Landscape.NewSiteVar<int>();

                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    subcanopyparmax[site] = (int)System.Math.Round(sitecohorts[site].SubCanopyPARmax, 0);
                }

                return subcanopyparmax;
            }
        }

        
        public static ISiteVar<int> AnnualTranspiration
        {
            get
            {
                ISiteVar<int> annualtranspiration = PlugIn.ModelCore.Landscape.NewSiteVar<int>();
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    annualtranspiration[site] = (int)System.Math.Round(sitecohorts[site].Hydrology.AnnualTranspiration, 0);
                }
                return annualtranspiration;
            }
        }


        public static Library.Biomass.Ecoregions.AuxParm<float> AverageLAIperEcoRegion
        {
            get
            {
                Library.Biomass.Ecoregions.AuxParm<double> SumLAI = new Library.Biomass.Ecoregions.AuxParm<double>(PlugIn.ModelCore.Ecoregions);
                Library.Biomass.Ecoregions.AuxParm<float> n = new Library.Biomass.Ecoregions.AuxParm<float>(PlugIn.ModelCore.Ecoregions);

                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    SumLAI[PlugIn.ModelCore.Ecoregion[site]] += (double)System.Math.Round(sitecohorts[site].CanopyLAImax, 0);
                    n[PlugIn.ModelCore.Ecoregion[site]]++;
                }

                Library.Biomass.Ecoregions.AuxParm<float> AverageLAI = new Library.Biomass.Ecoregions.AuxParm<float>(PlugIn.ModelCore.Ecoregions);

                foreach (IEcoregion e in PlugIn.ModelCore.Ecoregions)
                {
                    AverageLAI[e] = (float)SumLAI[e] / n[e];
                }
                return AverageLAI;
            }
      
        
        
        }
        public static Library.Biomass.Ecoregions.AuxParm<float> AverageWaterPerEcoregion
        {
            get
            {
                Library.Biomass.Ecoregions.AuxParm<double> SumWater = new Library.Biomass.Ecoregions.AuxParm<double>(PlugIn.ModelCore.Ecoregions);
                Library.Biomass.Ecoregions.AuxParm<float> n = new Library.Biomass.Ecoregions.AuxParm<float>(PlugIn.ModelCore.Ecoregions);
                
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    SumWater[PlugIn.ModelCore.Ecoregion[site]] += (double)System.Math.Round(sitecohorts[site].Hydrology.Water, 0);
                    n[PlugIn.ModelCore.Ecoregion[site]]++;
                }

                Library.Biomass.Ecoregions.AuxParm<float> AverageWater = new Library.Biomass.Ecoregions.AuxParm<float>(PlugIn.ModelCore.Ecoregions);
                
                foreach (IEcoregion e in PlugIn.ModelCore.Ecoregions)
                {
                    AverageWater[e] = (float)SumWater[e] / n[e];
                }
                return AverageWater;
            }
        }
        public static ISiteVar<int> Water
        {
            get
            {
                ISiteVar<int> water = PlugIn.ModelCore.Landscape.NewSiteVar<int>();
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    water[site] = (int)System.Math.Round(sitecohorts[site].Hydrology.Water, 0);
                }
                return water;
            }
        }
        public static ISiteVar<int> Lai
        {
            get
            {
                ISiteVar<int> lai = PlugIn.ModelCore.Landscape.NewSiteVar<int>();
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    lai[site] = (int)System.Math.Round(sitecohorts[site].CanopyLAImax, 0);
                }
                return lai;
            }
        }
        public static double Lai_av
        {
            get
            {
                double lai_sum = 0;
                float n = 0;
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    lai_sum += sitecohorts[site].CanopyLAImax;
                    n++;
                }
                return System.Math.Round(lai_sum / n, 2);
            }
        }
        public static double Water_av
        {
            get
            {
                float n = 0;
                double water_sum = 0;
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    water_sum += sitecohorts[site].Hydrology.Water;
                    n++;
                }
                return System.Math.Round(water_sum / n, 2);
            }
        }
        public static Landis.Library.Biomass.Species.AuxParm<int> Newcohorts_Spc
        {
            get
            {
                Landis.Library.Biomass.Species.AuxParm<int> newcohorts_spc = new Library.Biomass.Species.AuxParm<int>(PlugIn.ModelCore.Species);

                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    foreach (ISpecies spc in PlugIn.ModelCore.Species)
                    {
                        newcohorts_spc[spc] += sitecohorts[site].NewCohorts[spc];
                    }
                }
                return newcohorts_spc;
            }
        }

        
        public static int NewCohorts_sum
        {
            get
            {
                int newcohorts_sum = 0;
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    foreach (ISpecies spc in PlugIn.ModelCore.Species)
                    {
                        newcohorts_sum += sitecohorts[site].NewCohorts[spc];
                    }
                }
                return newcohorts_sum;
            }
        }


        public static ISiteVar<Landis.Library.Biomass.Species.AuxParm<int[]>>  DeadCohortAges
        {
            get
            {
                ISiteVar<Landis.Library.Biomass.Species.AuxParm<int[]>> deadcohortages = PlugIn.ModelCore.Landscape.NewSiteVar<Landis.Library.Biomass.Species.AuxParm<int[]>>();

                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    deadcohortages[site] = new Library.Biomass.Species.AuxParm<int[]>(PlugIn.ModelCore.Species);
                    foreach (ISpecies spc in PlugIn.ModelCore.Species)
                    {
                        deadcohortages[site][spc] = sitecohorts[site].DeadCohortAges[spc].ToArray();
                       
                    }
                }

                return deadcohortages;
            }
        }
        public static ISiteVar<Landis.Library.Biomass.Species.AuxParm<int[]>> CohortAges
        {
            get
            {
                ISiteVar<Landis.Library.Biomass.Species.AuxParm<int[]>> cohortages = PlugIn.ModelCore.Landscape.NewSiteVar<Landis.Library.Biomass.Species.AuxParm<int[]>>();

                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    cohortages[site] = new Library.Biomass.Species.AuxParm<int[]>(PlugIn.ModelCore.Species);
                     
                }
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    List<int> Ages = new List<int>();
                    foreach (ISpeciesCohorts spc in sitecohorts[site].Cohorts)
                    {
                        foreach (Landis.Library.BiomassCohortsPnET.ICohort cohort in spc)
                        {
                            Ages.Add(cohort.Age);
                           
                        }
                        cohortages[site][spc.Species] = Ages.ToArray();
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
                    foreach (ISpeciesCohorts spc in sitecohorts[site].Cohorts)
                    {
                        foreach (Landis.Library.BiomassCohortsPnET.ICohort cohort in spc)
                        {
                            if (cohort.Age > maxages[site]) maxages[site] = cohort.Age;
                        }
                    }
                }
                return maxages;
            }
        }
        
        public static Landis.Library.Biomass.Species.AuxParm<int> Deadcohorts_spc
        {
            get
            {
                Landis.Library.Biomass.Species.AuxParm<int> deadcohorts_spc = new Library.Biomass.Species.AuxParm<int>(PlugIn.ModelCore.Species);
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    foreach (ISpecies spc in PlugIn.ModelCore.Species)
                    {
                        deadcohorts_spc[spc] += sitecohorts[site].DeadCohorts[spc];
                    }
                }
                return deadcohorts_spc;
            }
        }
        public static ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> Deadcohorts
        {
            get
            {
                ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> deadcohorts = PlugIn.ModelCore.Landscape.NewSiteVar<Landis.Library.Biomass.Species.AuxParm<int>>();
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    deadcohorts[site] = new Library.Biomass.Species.AuxParm<int>(PlugIn.ModelCore.Species);
                }
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    foreach (ISpecies spc in PlugIn.ModelCore.Species)
                    {
                        deadcohorts[site][spc] += sitecohorts[site].DeadCohorts[spc];
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
                    foreach (ISpeciesCohorts spc in sitecohorts[site].Cohorts)
                    {
                        foreach (Landis.Library.BiomassCohortsPnET.ICohort cohort in spc)
                        {
                            belowgroundbiomass[site] += (int)System.Math.Round(cohort.Root, 0);
                        }
                    }
                }
                return belowgroundbiomass;
            }
        }
        
        public static ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> Cohorts
        {
            get
            {
                ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> cohorts = PlugIn.ModelCore.Landscape.NewSiteVar<Landis.Library.Biomass.Species.AuxParm<int>>();

                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    cohorts[site] = new Library.Biomass.Species.AuxParm<int>(PlugIn.ModelCore.Species);
                    foreach (ISpeciesCohorts spc in sitecohorts[site].Cohorts)
                    {
                        foreach (Landis.Library.BiomassCohortsPnET.ICohort cohort in spc)
                        {
                            cohorts[site][spc.Species]++;
                        }
                    }
                }

                return cohorts;
            }
        }
        public static Landis.Library.Biomass.Species.AuxParm<int> Cohorts_spc
        {
            get
            {
                Landis.Library.Biomass.Species.AuxParm<int> cohorts_spc = new Library.Biomass.Species.AuxParm<int>(PlugIn.ModelCore.Species);

                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    foreach (ISpeciesCohorts spc in sitecohorts[site].Cohorts)
                    {
                        foreach (Landis.Library.BiomassCohortsPnET.ICohort cohort in spc)
                        {
                            cohorts_spc[spc.Species]++;
                        }
                    }
                }
                return cohorts_spc;
            }
        }
        public static double CohortAge_av
        {
            get
            {
                double cohortage_sum = 0;
                float n = 0;
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    foreach (ISpeciesCohorts spc in sitecohorts[site].Cohorts)
                    {
                        foreach (Landis.Library.BiomassCohortsPnET.ICohort cohort in spc)
                        {
                            cohortage_sum += cohort.Age;
                            n++;
                        }
                    }
                }
                return cohortage_sum / n;
            }
        }
        public static double Cohorts_sum
        {
            get
            {
                float n = 0;
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    foreach (ISpeciesCohorts spc in sitecohorts[site].Cohorts)
                    {
                        foreach (Landis.Library.BiomassCohortsPnET.ICohort cohort in spc)
                        {
                            n++;
                        }
                    }
                }
                return n;
            }
        }
        public static double Cohorts_avg
        {
            get
            {
                float Cohorts_sum = 0;
                float n = 0;
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    n++;
                    foreach (ISpeciesCohorts spc in sitecohorts[site].Cohorts)
                    {
                        foreach (Landis.Library.BiomassCohortsPnET.ICohort cohort in spc)
                        {
                            Cohorts_sum++;
                        }
                    }
                }
                //System.Console.WriteLine(Cohorts_sum);
                return Cohorts_sum / PlugIn.ModelCore.Landscape.ActiveSiteCount;
            }
        }
        public static double Biomass_av
        {
            get
            {
                // Average total biomass (kg/m2) in the landscape
                double biomass_sum =0;
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    foreach (ISpeciesCohorts spc in sitecohorts[site].Cohorts)
                    {
                        foreach (Landis.Library.BiomassCohortsPnET.ICohort cohort in spc)
                        {
                            biomass_sum += cohort.Biomass;
                             
                        }
                    }
                }
                return biomass_sum / (float) PlugIn.ModelCore.Landscape.ActiveSiteCount;
            }
        }
        public static double Biomass_sum
        {
            get
            {
                // total biomass in the landscape
                double biomass_sum = 0;
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    foreach (ISpeciesCohorts spc in sitecohorts[site].Cohorts)
                    {
                        foreach (Landis.Library.BiomassCohortsPnET.ICohort cohort in spc)
                        {
                            biomass_sum += cohort.Biomass;
                        }
                    }
                }
                return biomass_sum;
            }
        }
        public static ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> Biomass
        {
            get
            {
                // Average g/m2 per species
                ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> biomass = PlugIn.ModelCore.Landscape.NewSiteVar<Landis.Library.Biomass.Species.AuxParm<int>>();
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    biomass[site] = new Library.Biomass.Species.AuxParm<int>(PlugIn.ModelCore.Species);
                    foreach (ISpeciesCohorts spc in sitecohorts[site].Cohorts)
                    {
                        foreach (Landis.Library.BiomassCohortsPnET.ICohort cohort in spc)
                        {
                            biomass[site][spc.Species] += cohort.Biomass;
                        }
                    }
                }
                return biomass;
            }
        }
        public static Landis.Library.Biomass.Species.AuxParm<float> Biomass_spc
        {
            get
            {
                // Average (g/m2) per species
                Landis.Library.Biomass.Species.AuxParm<float> biomass_spc = new Library.Biomass.Species.AuxParm<float>(PlugIn.ModelCore.Species);
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    foreach (ISpeciesCohorts spc in sitecohorts[site].Cohorts)
                    {
                        foreach (Landis.Library.BiomassCohortsPnET.ICohort cohort in spc)
                        {
                            biomass_spc[spc.Species] += cohort.Biomass;
                        }
                    }
                }
                
                foreach (ISpecies spc in PlugIn.ModelCore.Species)
                {
                    biomass_spc[spc] *= (float)(1F / (float)PlugIn.ModelCore.Landscape.ActiveSiteCount);
                }
                
                return biomass_spc;
            }
        }


        /// <summary>
        /// Initializes the module.
        /// </summary>
        public static void Initialize()
        {
            woodyDebris = PlugIn.ModelCore.GetSiteVar<Landis.Library.Biomass.Pool>("Succession.WoodyDebris");
            litter = PlugIn.ModelCore.GetSiteVar<Landis.Library.Biomass.Pool>("Succession.Litter");

            sitecohorts = PlugIn.ModelCore.GetSiteVar<Landis.Extension.Succession.BiomassPnET.ISiteCohorts>("Succession.SiteCohortsPnET");

            if (woodyDebris == null)
            {
                string mesg = string.Format("Cannot retrieve woodyDebris pool in " + PlugIn.ExtensionName);
                throw new System.ApplicationException(mesg);
            }
            if (litter == null)
            {
                string mesg = string.Format("Cannot retrieve litter pool in "+ PlugIn.ExtensionName);
                throw new System.ApplicationException(mesg);
            }
            if (sitecohorts == null)
            {
                string mesg = string.Format("Siteconditions are empty.  Please double-check that this extension is compatible with your chosen succession extension.");
                throw new System.ApplicationException(mesg);
            }

        }
        
    }
}
