//  Copyright 2007 University of Wisconsin-Madison
//  Authors:  James Domingo, UW-Madison, FLEL
//  License:  Available at  
//  http://landis.forest.wisc.edu/developers/LANDIS-IISourceCodeLicenseAgreement.pdf

using Landis.AgeCohort;
using Wisc.Flel.GeospatialModeling.Landscapes.DualScale;

namespace Landis.Output.MaxSpeciesAge
{
    /// <summary>
    /// Computes the maximum age for a particular set of cohorts at each site.
    /// </summary>
    public abstract class MaxAgeCalculator
    {
        private ILandscapeCohorts cohorts;

        //---------------------------------------------------------------------

        protected ILandscapeCohorts Cohorts
        {
            get {
                return cohorts;
            }
        }

        //---------------------------------------------------------------------

        public MaxAgeCalculator(ILandscapeCohorts cohorts)
        {
            this.cohorts = cohorts;
        }

        //---------------------------------------------------------------------

        public abstract ushort ComputeMaxAge(Site site);
    }
}
