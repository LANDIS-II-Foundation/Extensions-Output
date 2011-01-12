//  Copyright 2008-2010  Portland State University, Conservation Biology Institute
//  Authors:  Brendan C. Ward, Robert M. Scheller

using Landis.Core;
using Landis.SpatialModeling;
using Landis.Library.BiomassCohorts;

namespace Landis.Extension.Output.BiomassAgeClass
{
    public static class SiteVars
    {
        private static ISiteVar<SiteCohorts> cohorts;

        //---------------------------------------------------------------------

        public static void Initialize()
        {
            cohorts = PlugIn.ModelCore.GetSiteVar<SiteCohorts>("Succession.BiomassCohorts");
            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                // Test to make sure the cohort type is correct for this extension
                if (site.Location.Row == 1 && site.Location.Column == 1 && !SiteVars.Cohorts[site].HasAge() && !SiteVars.Cohorts[site].HasBiomass())
                {
                    throw new System.ApplicationException("Error in the Scenario file:  Incompatible extensions; Cohort age AND biomass data required for this extension to operate.");
                }
            }

            //Initialize TimeSinceLastFire to the maximum cohort age:
            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                // Test to make sure the cohort type is correct for this extension
                if (site.Location.Row == 1 && site.Location.Column == 1 && !SiteVars.Cohorts[site].HasAge())
                {
                    throw new System.ApplicationException("Error in the Scenario file:  Incompatible extensions; Cohort age data required for this extension to operate.");
                }
            }

        }

        //---------------------------------------------------------------------
        public static ISiteVar<SiteCohorts> Cohorts
        {
            get
            {
                return cohorts;
            }
        }
    }
}
