//  Copyright 2013 Portland State University
//  Authors:  Robert M. Scheller

using Landis.Core;
using Landis.SpatialModeling;
using Landis.Library.LeafBiomassCohorts;

namespace Landis.Extension.Output.LeafBiomassReclass
{
    public static class SiteVars
    {
        private static ISiteVar<ISiteCohorts> cohorts;

        //---------------------------------------------------------------------

        public static void Initialize()
        {
            cohorts = PlugIn.ModelCore.GetSiteVar<ISiteCohorts>("Succession.BiomassCohorts");

            if (cohorts == null)
            {
                string mesg = string.Format("Cohorts are empty.  Please double-check that this extension is compatible with your chosen succession extension.");
                throw new System.ApplicationException(mesg);
            }
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
