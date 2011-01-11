//  Copyright 2007 University of Wisconsin-Madison
//  Authors:  James Domingo, UW-Madison, FLEL
//  License:  Available at  
//  http://landis.forest.wisc.edu/developers/LANDIS-IISourceCodeLicenseAgreement.pdf

using Landis.AgeCohort;
using Landis.Species;
using Wisc.Flel.GeospatialModeling.Landscapes.DualScale;

namespace Landis.Output.MaxSpeciesAge
{
    /// <summary>
    /// Computes the maximum age for a particular species at each site.
    /// </summary>
    public class SpeciesMaxAgeCalculator
        : MaxAgeCalculator
    {
        private ISpecies species;

        //---------------------------------------------------------------------

        public string SpeciesName
        {
            get {
                return species.Name;
            }
        }

        //---------------------------------------------------------------------

        public SpeciesMaxAgeCalculator(ISpecies          species,
                                       ILandscapeCohorts cohorts)
            : base(cohorts)
        {
            this.species = species;
        }

        //---------------------------------------------------------------------

        public override ushort ComputeMaxAge(Site site)
        {
            return AgeCohort.Util.GetMaxAge(Cohorts[site][species]);
        }
    }
}
