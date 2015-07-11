using Landis.Biomass;

namespace Landis.Output.BiomassReclass
{
    /// <summary>
    /// Methods for computing biomass for groups of cohorts.
    /// </summary>
    public static class Util
    {
        public static ushort ComputeBiomass(ISpeciesCohorts cohorts)
        {
            ushort total = 0;
            if (cohorts != null)
                foreach (ICohort cohort in cohorts)
                    total += cohort.Biomass;
            return total;
        }

        //---------------------------------------------------------------------

        public static ushort ComputeBiomass(ISiteCohorts cohorts)
        {
            ushort total = 0;
            if (cohorts != null)
                foreach (ISpeciesCohorts speciesCohorts in cohorts)
                    total += ComputeBiomass(speciesCohorts);
            return total;
        }
    }
}
