//  Copyright 2007 University of Wisconsin-Madison
//  Authors:  James Domingo, UW-Madison, FLEL
//  License:  Available at  
//  http://landis.forest.wisc.edu/developers/LANDIS-IISourceCodeLicenseAgreement.pdf

using Landis.AgeCohort;
using Wisc.Flel.GeospatialModeling.Landscapes.DualScale;

namespace Landis.Output.MaxSpeciesAge
{
    /// <summary>
    /// Computes the maximum age for all the cohorts at each site.
    /// </summary>
    public class SiteMaxAgeCalculator
        : MaxAgeCalculator
    {
        public SiteMaxAgeCalculator(ILandscapeCohorts cohorts)
            : base(cohorts)
        {
        }

        //---------------------------------------------------------------------

        public override ushort ComputeMaxAge(Site site)
        {
            return AgeCohort.Util.GetMaxAge(Cohorts[site]);
        }
    }
}
