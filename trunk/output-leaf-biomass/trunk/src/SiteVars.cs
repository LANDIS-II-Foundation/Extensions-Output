//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Robert M. Scheller, James B. Domingo

using Landis.Core;
using Landis.Library.LeafBiomassCohorts;
using System.Collections.Generic;
using Landis.SpatialModeling;

namespace Landis.Extension.Output.LeafBiomass
{
    /// <summary>
    /// The pools of dead biomass for the landscape's sites.
    /// </summary>
    public static class SiteVars
    {
        private static ISiteVar<ISiteCohorts> cohorts;

        
        //---------------------------------------------------------------------

        /// <summary>
        /// Initializes the module.
        /// </summary>
        public static void Initialize()
        {

            cohorts = PlugIn.ModelCore.GetSiteVar<ISiteCohorts>("Succession.LeafBiomassCohorts");
            
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
