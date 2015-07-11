
using Landis.Core;
using Landis.SpatialModeling;
using Landis.Library.AgeOnlyCohorts;

namespace Landis.Extension.Output.MaxSpeciesAge
{
    public static class SiteVars
    {
        private static ISiteVar<ISiteCohorts> cohorts;

        //---------------------------------------------------------------------

        public static void Initialize()
        {
            cohorts = PlugIn.ModelCore.GetSiteVar<ISiteCohorts>("Succession.AgeCohorts");
            
        }

        //---------------------------------------------------------------------

        public static ISiteVar<ISiteCohorts> Cohorts
        {
            get
            {
                return cohorts;
            }
        }

        //---------------------------------------------------------------------
        public static short GetMaxAge(ISpecies species, ActiveSite site)
        {
            if (SiteVars.Cohorts[site] == null)
            {
                PlugIn.ModelCore.UI.WriteLine("Cohort are null.");
                return 0;
            }
            short max = 0;

            foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
            {
                if(speciesCohorts.Species == species)
                    foreach (ICohort cohort in speciesCohorts)
                    {
                        if (cohort.Age > max)
                            max = (short) cohort.Age;
                    }
            }
            return max;
        }
        //---------------------------------------------------------------------
        public static short GetMaxAge(ActiveSite site)
        {
            if (SiteVars.Cohorts[site] == null)
            {
                PlugIn.ModelCore.UI.WriteLine("Cohort are null.");
                return 0;
            }
            short max = 0;

            foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
            {
                foreach (ICohort cohort in speciesCohorts)
                {
                    if (cohort.Age > max)
                        max = (short) cohort.Age;
                }
            }
            return max;
        }
    }
}

