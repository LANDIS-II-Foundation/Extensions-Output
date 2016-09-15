using Edu.Wisc.Forest.Flel.Util;

using Landis.Biomass;
using Landis.Landscape;
using Landis.RasterIO;
using Landis.Species;

using System;
using System.Collections.Generic;

namespace Landis.Output.BiomassAgeClass
{
    public class PlugIn
        : Landis.PlugIns.PlugIn
    {
        private PlugIns.ICore modelCore;
        private IEnumerable<ISpecies> selectedSpecies;
        private string speciesMapNameTemplate;
        private ILandscapeCohorts cohorts;
        private Dictionary<string, List<AgeClass>> ageClasses;

        private AgeClass ageclass = new AgeClass();

        //---------------------------------------------------------------------

        public PlugIn()
            : base("Output Biomass AgeClass", new PlugIns.PlugInType("output"))
        {
        }

        //---------------------------------------------------------------------

        public override void Initialize(string        dataFile,
                                        PlugIns.ICore modelCore)
        {
            this.modelCore = modelCore;

            InputParametersParser parser = new InputParametersParser(modelCore.Species);
            IInputParameters parameters = Data.Load<IInputParameters>(dataFile, parser);

            Timestep = parameters.Timestep;
            this.selectedSpecies = parameters.SelectedSpecies;
            this.speciesMapNameTemplate = parameters.SpeciesMapNames;
            this.ageClasses = parameters.AgeClasses;

            cohorts = modelCore.SuccessionCohorts as ILandscapeCohorts;
            if (cohorts == null)
                throw new ApplicationException("Error: Cohorts don't support biomass interface");
        }

        //---------------------------------------------------------------------

        public override void Run()
        {
            //FIXME!
            //WriteMapForAllSpecies();

            if (selectedSpecies != null)
                WriteSpeciesMaps();

        }

        //---------------------------------------------------------------------


        private void WriteSpeciesMaps()
        {
            foreach (ISpecies species in selectedSpecies)
            {
                foreach(AgeClass ageclass in ageClasses[species.Name])
                {
                    IOutputRaster<BiomassPixel> map = CreateMap(MakeSpeciesMapName(species.Name, ageclass.Name));
                    using (map)
                    {
                        BiomassPixel pixel = new BiomassPixel();
                        foreach (Site site in modelCore.Landscape.AllSites)
                        {
                            if (site.IsActive)
                                pixel.Band0 = (ushort)((float)Util.ComputeAgeClassBiomass(cohorts[site][species], ageclass) / 100.0);
                            else
                                pixel.Band0 = 0;
                            map.WritePixel(pixel);
                        }
                    }
                }
            }

        }

        //---------------------------------------------------------------------

        private string MakeSpeciesMapName(string species,string ageclass)
        {
            return MapNames.ReplaceTemplateVars(speciesMapNameTemplate,
                                                       species,
                                                       ageclass,
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

    }
}
