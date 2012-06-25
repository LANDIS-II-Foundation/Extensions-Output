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
        public delegate int SiteCohortStatDelegate(Site site); 
        public delegate int SpeciesCohortStatDelegate(ISpecies species, Site site);
        
        public static int GetMaxAge(Site site) 
        {
            if (SiteVars.Cohorts[site] == null)
                return 0;
            int max = 0;
            foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
            {
                int maxSpeciesAge = GetMaxAge(speciesCohorts.Species, site);
                if (maxSpeciesAge > max)
                    max = maxSpeciesAge;
            }
            return max;
        }

        //---------------------------------------------------------------------
        public static int GetMaxAge(ISpecies species, Site site)
        {
            if (SiteVars.Cohorts[site] == null)
            {
                PlugIn.ModelCore.Log.WriteLine("Cohort are null.");
                return 0;
            }
            int max = 0;

            foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
            {
                if(speciesCohorts.Species == species)
                    foreach (ICohort cohort in speciesCohorts)
                    {
                        if (cohort.Age > max)
                            max = (int) cohort.Age;
                    }
            }
            return max;
        }
        /*public static uint GetMaxAge(ISpecies species, ActiveSite site) //Cohorts speciesCohorts)
        {
            //if (speciesCohorts == null)
            //    return 0;
            uint max = 0;
            foreach (ICohort cohort in speciesCohorts)
            {
                //  First cohort is the oldest
                max = cohort.Age;
                break;
            }
            return max;
        }*/

        //---------------------------------------------------------------------

        public static int GetMinAge(Site site) 
        {
            if (SiteVars.Cohorts[site] == null)
                return 0;
            int min = 32767;//maxof uint
            foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
            {
                int minSpeciesAge = GetMinAge(speciesCohorts.Species, site); //Cohorts);
                if (minSpeciesAge < min)
                    min = minSpeciesAge;
            }
            return min;
        }
        //---------------------------------------------------------------------
        public static int GetMinAge(ISpecies species, Site site)
        {
            if (SiteVars.Cohorts[site] == null)
            {
                PlugIn.ModelCore.Log.WriteLine("Cohort are null.");
                return 0;
            }
            int min = 32767;//maxof uint

            foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
            {
                if (speciesCohorts.Species == species)
                    foreach (ICohort cohort in speciesCohorts)
                    {
                        if (cohort.Age < min)
                            min = (int) cohort.Age;
                    }
            }
            return min;
        }

        /*public static uint GetMinAge(ISpeciesCohorts speciesCohorts)
        {
            if (speciesCohorts == null)
                return 0;
            uint min = 65535;
            foreach (ICohort cohort in speciesCohorts)
            {
                if(cohort.Age<min)
                    min = cohort.Age;
            }
            return min;
        }*/

        //---------------------------------------------------------------------

        public static int GetMedianAge(Site site) 
        {
            if (SiteVars.Cohorts[site] == null)
                return 0;
            int median = 0;
            double dbl_median = 0.0;

            List<int> cohort_ages = new List<int>();
            foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
            {
                foreach(ICohort cohort in speciesCohorts)
                {
                    cohort_ages.Add((int) cohort.Age);
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
                median = (int)dbl_median;
            }
            else
            {
                median = cohort_ages[count / 2];
            }
            return median;
        }

        public static int GetMedianAge(ISpecies species, Site site) 
        {
            if (SiteVars.Cohorts[site] == null)
                return 0;
            int median = 0;
            double dbl_median = 0.0;

            List<int> cohort_ages = new List<int>();
            foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
            {
                if(speciesCohorts.Species == species)
                    foreach (ICohort cohort in speciesCohorts)
                    {
                        cohort_ages.Add((int) cohort.Age);
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
                median = (int)dbl_median;
            }
            else
            {
                median = cohort_ages[count / 2];
            }
            return median;
        }

        //---------------------------------------------------------------------

        public static int GetAvgAge(Site site) 
        {
            if (SiteVars.Cohorts[site] == null)
                return 0;
            int avg = 0;
            int sum = 0;
            int count = 0;

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

            avg = (int)(sum / count);
            return avg;
        }

        public static int GetAvgAge(ISpecies species, Site site) 
        {
            if (SiteVars.Cohorts[site] == null)
                return 0;
            int avg = 0;
            int sum = 0;
            int count = 0;

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

            avg = (int)(sum / count);
            return avg;
        }

        //---------------------------------------------------------------------
        //Note: don't call Var directly, it will be too big!
        public static uint GetVarAge(Site site)
        {
            if (SiteVars.Cohorts[site] == null)
                return 0;
            int avg = GetAvgAge(site);
            double sum = 0;
            int count = 0;
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

        public static uint GetVarAge(ISpecies species, Site site) 
        {
            if (SiteVars.Cohorts[site] == null)
                return 0;
            int avg = GetAvgAge(species, site); //speciesCohorts);
            double sum = 0;
            int count = 0;
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

        public static int GetStdDevAge(Site site) 
        {
            if (SiteVars.Cohorts[site] == null)
                return 0;
            int std_dev = (int)System.Math.Sqrt(GetVarAge(site));
            return std_dev;
        }

        public static int GetStdDevAge(ISpecies species, Site site)
        {
            if (SiteVars.Cohorts[site] == null)
                return 0;
            int std_dev = (int)System.Math.Round(System.Math.Sqrt(GetVarAge(species, site)),0);         
            return std_dev;
        }

        //---------------------------------------------------------------------

        public static int GetCohortCount(Site site) 
        {//return total count of cohorts
            int count = 0;
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
        //Return E * 100 to fit within uint range
        public static int GetAgeEvenness(Site site) 
        {
            double E = 0;
            double Hprime = 0;
            double proportion=0;
            int evenness = 0;
            int total_count = 0;
            Dictionary<int, int> cohort_counts = new Dictionary<int, int>();
            if (SiteVars.Cohorts[site] == null)
                return 0;
            foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
            {
                foreach (ICohort cohort in speciesCohorts)
                {
                    total_count++;
                    if (!cohort_counts.ContainsKey((int) cohort.Age))
                    {
                        cohort_counts.Add((int) cohort.Age, 1);
                    }
                    else
                    {
                        cohort_counts[(int) cohort.Age]++;
                    }
                }
            }

            foreach (KeyValuePair<int,int> cohortIter in cohort_counts)
            {
                proportion = (double)cohortIter.Value / (double)total_count;
                Hprime += proportion * System.Math.Log(proportion);
            }
            Hprime = - Hprime;
            E = Hprime / System.Math.Log(cohort_counts.Count);
            evenness = (int)(E * 100.0);

            return evenness;
        }

        //---------------------------------------------------------------------
        // Number of age classes, an indicator of structural complexity.
        public static int GetAgeRichness(Site site)
        {
            int age_richness = 0;
            List<int> ages = new List<int>();
            if (SiteVars.Cohorts[site] == null)
                return 0;
            foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
            {
                foreach (ICohort cohort in speciesCohorts)
                {
                    if (!ages.Contains(cohort.Age))
                    {
                        ages.Add(cohort.Age);
                        age_richness++;
                    }
                }
            }

            return age_richness;
        }
        //---------------------------------------------------------------------

        public static int GetSppRichness(Site site) 
        {//return total count of species
            int count = 0;
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