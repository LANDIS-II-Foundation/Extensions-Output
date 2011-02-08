//  Copyright 2008-2010  Portland State University, Conservation Biology Institute
//  Authors:  Brendan C. Ward, Robert M. Scheller

using Edu.Wisc.Forest.Flel.Util;
using Landis.Core;
using Landis.Library.AgeOnlyCohorts;
using Landis.SpatialModeling;
using System.Collections.Generic;

namespace Landis.Extension.Output.CohortStats

{
 
    public class CohortUtils
    {
        public delegate ushort SiteCohortStatDelegate(ActiveSite site); 
        public delegate ushort SpeciesCohortStatDelegate(ISpecies species, ActiveSite site);
        
        public static ushort GetMaxAge(ActiveSite site) 
        {
            if (SiteVars.Cohorts[site] == null)
                return 0;
            ushort max = 0;
            foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
            {
                ushort maxSpeciesAge = GetMaxAge(speciesCohorts.Species, site);
                if (maxSpeciesAge > max)
                    max = maxSpeciesAge;
            }
            return max;
        }

        //---------------------------------------------------------------------
        public static ushort GetMaxAge(ISpecies species, ActiveSite site)
        {
            if (SiteVars.Cohorts[site] == null)
            {
                PlugIn.ModelCore.Log.WriteLine("Cohort are null.");
                return 0;
            }
            ushort max = 0;

            foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
            {
                if(speciesCohorts.Species == species)
                    foreach (ICohort cohort in speciesCohorts)
                    {
                        if (cohort.Age > max)
                            max = cohort.Age;
                    }
            }
            return max;
        }
        /*public static ushort GetMaxAge(ISpecies species, ActiveSite site) //Cohorts speciesCohorts)
        {
            //if (speciesCohorts == null)
            //    return 0;
            ushort max = 0;
            foreach (ICohort cohort in speciesCohorts)
            {
                //  First cohort is the oldest
                max = cohort.Age;
                break;
            }
            return max;
        }*/

        //---------------------------------------------------------------------

        public static ushort GetMinAge(ActiveSite site) 
        {
            if (SiteVars.Cohorts[site] == null)
                return 0;
            ushort min = 65535;//maxof ushort
            foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
            {
                ushort minSpeciesAge = GetMinAge(speciesCohorts.Species, site); //Cohorts);
                if (minSpeciesAge < min)
                    min = minSpeciesAge;
            }
            return min;
        }
        //---------------------------------------------------------------------
        public static ushort GetMinAge(ISpecies species, ActiveSite site)
        {
            if (SiteVars.Cohorts[site] == null)
            {
                PlugIn.ModelCore.Log.WriteLine("Cohort are null.");
                return 0;
            }
            ushort min = 65535;//maxof ushort

            foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
            {
                if (speciesCohorts.Species == species)
                    foreach (ICohort cohort in speciesCohorts)
                    {
                        if (cohort.Age < min)
                            min = cohort.Age;
                    }
            }
            return min;
        }

        /*public static ushort GetMinAge(ISpeciesCohorts speciesCohorts)
        {
            if (speciesCohorts == null)
                return 0;
            ushort min = 65535;
            foreach (ICohort cohort in speciesCohorts)
            {
                if(cohort.Age<min)
                    min = cohort.Age;
            }
            return min;
        }*/

        //---------------------------------------------------------------------

        public static ushort GetMedianAge(ActiveSite site) 
        {
            if (SiteVars.Cohorts[site] == null)
                return 0;
            ushort median = 0;
            double dbl_median = 0.0;

            List<ushort> cohort_ages = new List<ushort>();
            foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
            {
                foreach(ICohort cohort in speciesCohorts)
                {
                    cohort_ages.Add(cohort.Age);
                }
            }
            int count = cohort_ages.Count;
            if (count == 0)
            {
                return 0;
            }

            else if (count == 1)
            {
                return cohort_ages[0];
            }

            cohort_ages.Sort();//sorts in ascending order
            
            if (count % 2 == 0)
            {
                dbl_median = (cohort_ages[count / 2] + cohort_ages[(count / 2) - 1]) / 2.0;
                median = (ushort)dbl_median;
            }
            else
            {
                median = cohort_ages[count / 2];
            }
            return median;
        }

        public static ushort GetMedianAge(ISpecies species, ActiveSite site) 
        {
            if (SiteVars.Cohorts[site] == null)
                return 0;
            ushort median = 0;
            double dbl_median = 0.0;

            List<ushort> cohort_ages = new List<ushort>();
            foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
            {
                if(speciesCohorts.Species == species)
                    foreach (ICohort cohort in speciesCohorts)
                    {
                        cohort_ages.Add(cohort.Age);
                    }
            }
            int count = cohort_ages.Count;
            if (count == 0)
            {
                return 0;
            }

            else if (count == 1)
            {
                return cohort_ages[0];
            }            
            
            cohort_ages.Sort();//sorts in ascending order
            

            if (count % 2 == 0)
            {
                dbl_median = (cohort_ages[count / 2] + cohort_ages[(count / 2) - 1]) / 2.0;
                median = (ushort)dbl_median;
            }
            else
            {
                median = cohort_ages[count / 2];
            }
            return median;
        }

        //---------------------------------------------------------------------

        public static ushort GetAvgAge(ActiveSite site) 
        {
            if (SiteVars.Cohorts[site] == null)
                return 0;
            ushort avg = 0;
            uint sum = 0;
            ushort count = 0;

            foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
            {
                foreach (ICohort cohort in speciesCohorts)
                {
                    sum += cohort.Age;
                    count++;
                }
            }

            if (count == 0)
            {
                return 0;
            }

            avg = (ushort)(sum / count);
            return avg;
        }

        public static ushort GetAvgAge(ISpecies species, ActiveSite site) 
        {
            if (SiteVars.Cohorts[site] == null)
                return 0;
            ushort avg = 0;
            uint sum = 0;
            ushort count = 0;

            foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
            {
                if(speciesCohorts.Species == species)
                    foreach (ICohort cohort in speciesCohorts)
                    {
                        sum += cohort.Age;
                        count++;
                    }
            }

            if (count == 0)
            {
                return 0;
            }

            avg = (ushort)(sum / count);
            return avg;
        }

        //---------------------------------------------------------------------
        //Note: don't call Var directly, it will be too big!
        public static uint GetVarAge(ActiveSite site)
        {
            if (SiteVars.Cohorts[site] == null)
                return 0;
            ushort avg = GetAvgAge(site);
            double sum = 0;
            ushort count = 0;
            foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
            {
                foreach (ICohort cohort in speciesCohorts)
                {
                    sum += System.Math.Pow(cohort.Age - avg, 2);
                    count++;
                }
            }
            if (count <= 1)
                return 0;
            return (uint)System.Math.Round((sum / (count - 1)));
        }

        public static uint GetVarAge(ISpecies species, ActiveSite site) 
        {
            if (SiteVars.Cohorts[site] == null)
                return 0;
            ushort avg = GetAvgAge(species, site); //speciesCohorts);
            double sum = 0;
            ushort count = 0;
            foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
            {
                if(speciesCohorts.Species == species)
                    foreach (ICohort cohort in speciesCohorts)
                    {
                        sum += System.Math.Pow(cohort.Age - avg, 2);
                        count++;
                    }
            }
            if (count <= 1)
                return 0;
            return (uint)System.Math.Round((sum / (count - 1)));
        }

        //---------------------------------------------------------------------

        public static ushort GetStdDevAge(ActiveSite site) 
        {
            if (SiteVars.Cohorts[site] == null)
                return 0;
            ushort std_dev = (ushort)System.Math.Sqrt(GetVarAge(site));
            return std_dev;
        }

        public static ushort GetStdDevAge(ISpecies species, ActiveSite site)
        {
            if (SiteVars.Cohorts[site] == null)
                return 0;
            ushort std_dev = (ushort)System.Math.Round(System.Math.Sqrt(GetVarAge(species, site)),0);         
            return std_dev;
        }

        //---------------------------------------------------------------------

        public static ushort GetAgeRichness(ActiveSite site) 
        {//return total count of cohorts
            ushort count = 0;
            if (SiteVars.Cohorts[site] == null)
                return 0;
            foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
            {
                foreach (ICohort cohort in speciesCohorts)
                {
                    count++;
                }
            }
            return count;
        }

        //---------------------------------------------------------------------
        //Use E = Hprime / ln S   where S apparently is # species)    
        //where Hprime = -sum (pI * ln(pI))   where pI is proportion of individuals found in Ith species
        //from Magurran, A.  1988.  Ecological diversity and its measurements.  Princeton, NJ: Princeton University Press.  Pp 35-37)
        //Return E * 100 to fit within ushort range
        public static ushort GetAgeEvenness(ActiveSite site) 
        {
            double E = 0;
            double Hprime = 0;
            double proportion=0;
            ushort evenness = 0;
            ushort total_count = 0;
            Dictionary<ushort, ushort> cohort_counts = new Dictionary<ushort, ushort>();
            if (SiteVars.Cohorts[site] == null)
                return 0;
            foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
            {
                foreach (ICohort cohort in speciesCohorts)
                {
                    total_count++;
                    if (!cohort_counts.ContainsKey(cohort.Age))
                    {
                        cohort_counts.Add(cohort.Age, 1);
                    }
                    else
                    {
                        cohort_counts[cohort.Age]++;
                    }
                }
            }

            foreach (KeyValuePair<ushort,ushort> cohortIter in cohort_counts)
            {
                proportion = (double)cohortIter.Value / (double)total_count;
                Hprime += proportion * System.Math.Log(proportion);
            }
            Hprime = - Hprime;
            E = Hprime / System.Math.Log(cohort_counts.Count);
            evenness = (ushort)(E * 100.0);

            return evenness;
        }

        //---------------------------------------------------------------------

        public static ushort GetSppRichness(ActiveSite site) 
        {//return total count of species
            ushort count = 0;
            if (SiteVars.Cohorts[site] == null)
                return 0;
            foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
            {
                count++;
            }
            return count;
        }







    }
}