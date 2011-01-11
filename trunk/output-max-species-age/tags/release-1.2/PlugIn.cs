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

            InputParametersParser.SpeciesDataset = modelCore.Species;
            InputParametersParser parser = new InputParametersParser();
            IInputParameters parameters = Data.Load<IInputParameters>(dataFile, parser);

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
            //if keyword == maxage
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
            IOutputRaster<AgePixel> map = CreateMap("AllSppMaxAge");
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
            string path = MapNameTemplates.ReplaceTemplateVars(mapNameTemplate, species,
                                                       modelCore.CurrentTime);
            UI.WriteLine("Writing age map to {0} ...", path);
            return modelCore.CreateRaster<AgePixel>(path,
                                                    modelCore.Landscape.Dimensions,
                                                    modelCore.LandscapeMapMetadata);
        }
    }
}

        //---------------------------------------------------------------------

        /// <summary>
        /// Gets the maximum age among all cohorts at a site.
        /// </summary>
        /// <returns>
        /// The age of the oldest cohort or 0 if there are no cohorts.
        /// </returns>
/*        public static ushort GetMaxAge(ISiteCohorts cohorts)
        {
            if (cohorts == null)
                return 0;
            ushort max = 0;
            foreach (ISpeciesCohorts speciesCohorts in cohorts) {
                ushort maxSpeciesAge = GetMaxAge(speciesCohorts);
                if (maxSpeciesAge > max)
                    max = maxSpeciesAge;
            }
            return max;
        }
*/