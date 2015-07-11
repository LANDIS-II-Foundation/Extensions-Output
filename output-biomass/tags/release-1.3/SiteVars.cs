//  Copyright 2008 Conservation Biology Institute
//  Authors:  Robert M. Scheller
//  License:  Available at  
//  http://landis.forest.wisc.edu/developers/LANDIS-IISourceCodeLicenseAgreement.pdf

using Landis;
using Landis.Biomass.Succession;
using Landis.Ecoregions;
using Landis.Landscape;
using Landis.Species;
using System.Collections.Generic;

namespace Landis.Output.Biomass
{
    /// <summary>
    /// The pools of dead biomass for the landscape's sites.
    /// </summary>
    public static class SiteVars
    {
        private static ISiteVar<Pool> woodyDebris;
        private static ISiteVar<Pool> litter;
        
        //---------------------------------------------------------------------

        /// <summary>
        /// Initializes the module.
        /// </summary>
        public static void Initialize()
        {

            woodyDebris = Model.Core.GetSiteVar<Pool>("Succession.WoodyDebris");
            litter = Model.Core.GetSiteVar<Pool>("Succession.Litter");
            
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// The intact dead woody pools for the landscape's sites.
        /// </summary>
        public static ISiteVar<Pool> WoodyDebris
        {
            get {
                return woodyDebris;
            }
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// The dead non-woody pools for the landscape's sites.
        /// </summary>
        public static ISiteVar<Pool> Litter
        {
            get {
                return litter;
            }
        }

    }
}
