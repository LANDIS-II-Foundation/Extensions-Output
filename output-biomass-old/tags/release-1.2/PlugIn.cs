using Edu.Wisc.Forest.Flel.Util;

using Landis.Biomass;
using Landis.Biomass.Dead;
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
        private PlugIns.ICore modelCore;
        private IEnumerable<ISpecies> selectedSpecies;
        private string speciesMapNameTemplate;
        private SelectedDeadPools selectedPools;
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
            this.modelCore = modelCore;

            ParametersParser parser = new ParametersParser(modelCore.Species);
            IParameters parameters = Data.Load<IParameters>(dataFile,
                                                            parser);

            Timestep = parameters.Timestep;
            this.selectedSpecies = parameters.SelectedSpecies;
            this.speciesMapNameTemplate = parameters.SpeciesMapNames;
            this.selectedPools = parameters.SelectedPools;
            this.poolMapNameTemplate = parameters.PoolMapNames;

            cohorts = modelCore.SuccessionCohorts as ILandscapeCohorts;
            if (cohorts == null)
                throw new ApplicationException("Error: Cohorts don't support biomass interface");
        }

        //---------------------------------------------------------------------

        public override void Run()
        {
            WriteMapForAllSpecies();
            
            if (selectedSpecies != null)
                WriteSpeciesMaps();

            if (selectedPools != SelectedDeadPools.None)
                WritePoolMaps();
        }

        //---------------------------------------------------------------------

        private void WriteSpeciesMaps()
        {
            foreach (ISpecies species in selectedSpecies) {
                IOutputRaster<BiomassPixel> map = CreateMap(MakeSpeciesMapName(species.Name));
                using (map) {
                    BiomassPixel pixel = new BiomassPixel();
                    foreach (Site site in modelCore.Landscape.AllSites) {
                        if (site.IsActive)
                            pixel.Band0 = (ushort) ((float) Util.ComputeBiomass(cohorts[site][species]) / 100.0);
                        else
                            pixel.Band0 = 0;
                        map.WritePixel(pixel);
                    }
                }
            }

            //WriteMapForAllSpecies();
        }

        //---------------------------------------------------------------------

        private void WriteMapForAllSpecies()
        {
            // Biomass map for all species
            IOutputRaster<BiomassPixel> map = CreateMap(MakeSpeciesMapName("TotalBiomass"));
            using (map) {
                BiomassPixel pixel = new BiomassPixel();
                foreach (Site site in modelCore.Landscape.AllSites) {
                    if (site.IsActive)
                        pixel.Band0 = (ushort) ((float) Util.ComputeBiomass(cohorts[site]) / 100.0);
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
                                                       modelCore.CurrentTime);
        }

        //---------------------------------------------------------------------

        private IOutputRaster<BiomassPixel> CreateMap(string path)
        {
            UI.WriteLine("Writing biomass map to {0} ...", path);
            return modelCore.CreateRaster<BiomassPixel>(path,
                                                        modelCore.Landscape.Dimensions,
                                                        modelCore.LandscapeMapMetadata);
        }

        //---------------------------------------------------------------------

        private void WritePoolMaps()
        {
            if ((selectedPools & SelectedDeadPools.Woody) != 0)
                WritePoolMap("woody", Pools.Woody);

            if ((selectedPools & SelectedDeadPools.NonWoody) != 0)
                WritePoolMap("non-woody", Pools.NonWoody);
        }

        //---------------------------------------------------------------------

        private void WritePoolMap(string         poolName,
                                  ISiteVar<Pool> poolSiteVar)
        {
            string path = PoolMapNames.ReplaceTemplateVars(poolMapNameTemplate,
                                                           poolName,
                                                           modelCore.CurrentTime);
            IOutputRaster<BiomassPixel> map = CreateMap(path);
            using (map) {
                BiomassPixel pixel = new BiomassPixel();
                foreach (Site site in modelCore.Landscape.AllSites) {
                    if (site.IsActive)
                        pixel.Band0 = (ushort) (poolSiteVar[site].Biomass);
                    else
                        pixel.Band0 = 0;
                    map.WritePixel(pixel);
                }
            }
        }
    }
}
