//  Copyright 2005-2007 University of Wisconsin-Madison
//  Authors:  James Domingo, UW-Madison, FLEL
//  License:  Available at  
//  http://landis.forest.wisc.edu/developers/LANDIS-IISourceCodeLicenseAgreement.pdf

using Landis.AgeCohort;
using Landis.Species;
using Landis.DualScale;
using Wisc.Flel.GeospatialModeling.Landscapes.DualScale;
using Wisc.Flel.GeospatialModeling.RasterIO;

using System.Collections.Generic;
using System;

namespace Landis.Output.MaxSpeciesAge
{
    public class PlugIn
        : PlugIns.PlugIn
    {
        private PlugIns.ICore modelCore;
        private string mapNameTemplate;
        private IList<SpeciesMaxAgeCalculator> selectedSpecies;
        private SiteMaxAgeCalculator siteMaxAgeCalculator;
        private BlockRowBuffer<ushort> ageBuffer;

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

            ILandscapeCohorts cohorts = modelCore.SuccessionCohorts as ILandscapeCohorts;
            if (cohorts == null)
                throw new ApplicationException("Error: Cohorts don't support age-cohort interface");

            selectedSpecies = new List<SpeciesMaxAgeCalculator>();
            foreach (ISpecies species in parameters.SelectedSpecies)
                selectedSpecies.Add(new SpeciesMaxAgeCalculator(species, cohorts));

            siteMaxAgeCalculator = new SiteMaxAgeCalculator(cohorts);

            ageBuffer = new BlockRowBuffer<ushort>(modelCore.Landscape);
        }

        //---------------------------------------------------------------------

        public override void Run()
        {
            foreach (SpeciesMaxAgeCalculator speciesAgeCalculator in selectedSpecies) {
                WriteMap(speciesAgeCalculator.SpeciesName,
                         speciesAgeCalculator);
            }

            WriteMap("AllSppMaxAge", siteMaxAgeCalculator);
        }

        //---------------------------------------------------------------------

        private void WriteMap(string           speciesName,
                              MaxAgeCalculator maxAgeCalculator)
        {
            IOutputRaster<AgePixel> map = CreateMap(speciesName);
            using (map) {
                AgePixel pixel = new AgePixel();
                Location location_1_1 = new Location(1,1);
                foreach (Site site in modelCore.Landscape.AllSites) {
                    ushort age;
                    if (site.IsActive) {
                        ActiveSite activeSite = (ActiveSite) site;
                        if (activeSite.SharesData) {
                            Location blockLocation = activeSite.BroadScaleLocation;
                            if (activeSite.LocationInBlock == location_1_1) {
                                age = maxAgeCalculator.ComputeMaxAge(activeSite);
                                ageBuffer[blockLocation.Column] = age;
                            }
                            else {
                                // already computed age for the block
                                age = ageBuffer[blockLocation.Column];
                            }
                        }
                        else {
                            age = maxAgeCalculator.ComputeMaxAge(activeSite);
                        }
                    }
                    else {
                        // inactive site
                        age = 0;
                    }
                    pixel.Band0 = age;
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
