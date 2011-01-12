using Edu.Wisc.Forest.Flel.Util;

using Landis.Biomass.Succession;
using Landis.Biomass;
using Landis.Landscape;
using Landis.RasterIO;
using Landis.Species;

using System;
using System.Collections.Generic;

namespace Landis.Output.Biomass
{
    public class PlugIn
        : Landis.PlugIns.PlugIn
    {
        private IEnumerable<ISpecies> selectedSpecies;
        private string speciesMapNameTemplate;
        private string selectedPools;
        private string poolMapNameTemplate;
        private ILandscapeCohorts cohorts;

        //---------------------------------------------------------------------

        public PlugIn()
            : base("Output Biomass", new PlugIns.PlugInType("output"))
        {
        }

        //---------------------------------------------------------------------

        public override void Initialize(string        dataFile,
                                        PlugIns.ICore modelCore)
        {
            Model.Core = modelCore;

            InputParametersParser parser = new InputParametersParser(Model.Core.Species);
            IInputParameters parameters = Data.Load<IInputParameters>(dataFile,
                                                            parser);

            Timestep = parameters.Timestep;
            this.selectedSpecies = parameters.SelectedSpecies;
            this.speciesMapNameTemplate = parameters.SpeciesMapNames;
            this.selectedPools = parameters.SelectedPools;
            this.poolMapNameTemplate = parameters.PoolMapNames;

            cohorts = Model.Core.SuccessionCohorts as ILandscapeCohorts;
            if (cohorts == null)
                throw new ApplicationException("Error: Cohorts don't support biomass interface");
        }

        //---------------------------------------------------------------------

        public override void Run()
        {
            WriteMapForAllSpecies();

            if (selectedSpecies != null)
                WriteSpeciesMaps();

            //if (selectedPools != SelectedDeadPools.None)
                WritePoolMaps();
        }

        //---------------------------------------------------------------------

        private void WriteSpeciesMaps()
        {
            foreach (ISpecies species in selectedSpecies) {
                IOutputRaster<BiomassPixel> map = CreateMap(MakeSpeciesMapName(species.Name));
                using (map) {
                    BiomassPixel pixel = new BiomassPixel();
                    foreach (Site site in Model.Core.Landscape.AllSites) {
                        if (site.IsActive)
                            pixel.Band0 = (ushort) Math.Round((double) ComputeSpeciesBiomass(cohorts[site][species]) / 100.0);
                        else
                            pixel.Band0 = 0;
                        map.WritePixel(pixel);
                    }
                }
            }

        }

        //---------------------------------------------------------------------

        private void WriteMapForAllSpecies()
        {
            // Biomass map for all species
            IOutputRaster<BiomassPixel> map = CreateMap(MakeSpeciesMapName("TotalBiomass"));
            using (map) {
                BiomassPixel pixel = new BiomassPixel();
                foreach (Site site in Model.Core.Landscape.AllSites) {
                    if (site.IsActive)
                        pixel.Band0 = (ushort) Math.Round((double) ComputeTotalBiomass(cohorts[site]) / 100.0);
                    else
                        pixel.Band0 = 0;
                    map.WritePixel(pixel);
                }
            }
        }

        //---------------------------------------------------------------------

        private string MakeSpeciesMapName(string species)
        {
            return SpeciesMapNames.ReplaceTemplateVars(speciesMapNameTemplate,
                                                       species,
                                                       Model.Core.CurrentTime);
        }

        //---------------------------------------------------------------------

        private IOutputRaster<BiomassPixel> CreateMap(string path)
        {
            UI.WriteLine("Writing biomass map to {0} ...", path);
            return Model.Core.CreateRaster<BiomassPixel>(path,
                                                        Model.Core.Landscape.Dimensions,
                                                        Model.Core.LandscapeMapMetadata);
        }

        //---------------------------------------------------------------------

        private void WritePoolMaps()
        {
            if(selectedPools == "woody" || selectedPools == "both")
                WritePoolMap("woody", SiteVars.WoodyDebris);

            if(selectedPools == "non-woody" || selectedPools == "both")
                WritePoolMap("non-woody", SiteVars.Litter);
        }

        //---------------------------------------------------------------------

        private void WritePoolMap(string         poolName,
                                  ISiteVar<Pool> poolSiteVar)
        {
            string path = PoolMapNames.ReplaceTemplateVars(poolMapNameTemplate,
                                                           poolName,
                                                           Model.Core.CurrentTime);
            if(poolSiteVar != null)
            {
                IOutputRaster<BiomassPixel> map = CreateMap(path);
                using (map) {
                    BiomassPixel pixel = new BiomassPixel();
                    foreach (Site site in Model.Core.Landscape.AllSites) {
                        if (site.IsActive)
                            pixel.Band0 = (ushort) ((float) poolSiteVar[site].Mass / 100.0);
                        else
                            pixel.Band0 = 0;
                        map.WritePixel(pixel);
                    }
                }
            }
        }
        //---------------------------------------------------------------------
        private static int ComputeSpeciesBiomass(ISpeciesCohorts cohorts)
        {
            int total = 0;
            if (cohorts != null)
                foreach (ICohort cohort in cohorts)
                    total += cohort.Biomass;
            return total;
        }

        //---------------------------------------------------------------------

        private static int ComputeTotalBiomass(ISiteCohorts cohorts)
        {
            int total = 0;
            if (cohorts != null)
                foreach (ISpeciesCohorts speciesCohorts in cohorts)
                {
                    total += ComputeSpeciesBiomass(speciesCohorts);
                }
            return total;
        }
    }
}
