//  Copyright 2008-2010  Portland State University, Conservation Biology Institute
//  Authors:  Brendan C. Ward, Robert M. Scheller

using Landis.Core;
using Landis.SpatialModeling;
using Landis.Library.AgeOnlyCohorts;

namespace Landis.Extension.Output.CohortStats
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
    }
}
