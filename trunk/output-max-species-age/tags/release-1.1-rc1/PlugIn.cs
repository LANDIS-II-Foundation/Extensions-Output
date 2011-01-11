//  Copyright 2005 University of Wisconsin-Madison
//  Authors:  Jimm Domingo, FLEL
//  License:  Available at  
//  http://landis.forest.wisc.edu/developers/LANDIS-IISourceCodeLicenseAgreement.pdf

using Landis.AgeCohort;
using Landis.Landscape;
using Landis.RasterIO;
using Landis.Species;

using System.Collections.Generic;
using System;

namespace Landis.Output.MaxSpeciesAge
{
    public class PlugIn
        : PlugIns.PlugIn
    {
        private PlugIns.ICore modelCore;
        private string mapNameTemplate;
        private IEnumerable<ISpecies> selectedSpecies;
        private ILandscapeCohorts cohorts;

        //---------------------------------------------------------------------

        public PlugIn()
            : base("Max Species Age", new PlugIns.PlugInType("output"))
        {
        }

        //---------------------------------------------------------------------

        public override void Initialize(string        dataFile,
                                        PlugIns.ICore modelCore)
        {
            this.modelCore = modelCore;

            ParametersParser.SpeciesDataset = modelCore.Species;
            ParametersParser parser = new ParametersParser();
            IParameters parameters = Data.Load<IParameters>(dataFile,
                                                            parser);

            Timestep = parameters.Timestep;
            mapNameTemplate = parameters.MapNames;
            selectedSpecies = parameters.SelectedSpecies;

            cohorts = modelCore.SuccessionCohorts as ILandscapeCohorts;
            if (cohorts == null)
                throw new ApplicationException("Error: Cohorts don't support age-cohort interface");
        }

        //---------------------------------------------------------------------

        public override void Run()
        {
            foreach (ISpecies species in selectedSpecies) {
                IOutputRaster<AgePixel> map = CreateMap(species.Name);
                using (map) {
                    AgePixel pixel = new AgePixel();
                    foreach (Site site in modelCore.Landscape.AllSites) {
                        if (site.IsActive)
                            pixel.Band0 = AgeCohort.Util.GetMaxAge(cohorts[site][species]);
                        else
                            pixel.Band0 = 0;
                        map.WritePixel(pixel);
                    }
                }
            }

            WriteMapWithMaxAgeAmongAll();
        }

        //---------------------------------------------------------------------

        private void WriteMapWithMaxAgeAmongAll()
        {
            //    Maximum age map for all species
            IOutputRaster<AgePixel> map = CreateMap("all");
            using (map) {
                AgePixel pixel = new AgePixel();
                foreach (Site site in modelCore.Landscape.AllSites) {
                    if (site.IsActive)
                        pixel.Band0 = AgeCohort.Util.GetMaxAge(cohorts[site]);
                    else
                        pixel.Band0 = 0;
                    map.WritePixel(pixel);
                }
            }
        }

        //---------------------------------------------------------------------

        private IOutputRaster<AgePixel> CreateMap(string species)
        {
            string path = MapNames.ReplaceTemplateVars(mapNameTemplate, species,
                                                       modelCore.CurrentTime);
            UI.WriteLine("Writing age map to {0} ...", path);
            return modelCore.CreateRaster<AgePixel>(path,
                                                    modelCore.Landscape.Dimensions,
                                                    modelCore.LandscapeMapMetadata);
        }
    }
}
