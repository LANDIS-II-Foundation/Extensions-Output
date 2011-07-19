//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Robert M. Scheller, James B. Domingo

using Edu.Wisc.Forest.Flel.Util;
using Landis.SpatialModeling;
using Landis.Core;

using System;
using System.Collections.Generic;

namespace Landis.Extension.Output.BiomassByAge
{
    public class PlugIn
        : ExtensionMain
    {
        public static readonly ExtensionType Type = new ExtensionType("output");
        public static readonly string ExtensionName = "Output Biomass-by-Age";
        
        private static ICore modelCore;
        private IEnumerable<ISpecies> selectedSpecies;
        private string speciesMapNameTemplate;
        private Dictionary<string, List<AgeClass>> ageClasses;
        private IInputParameters parameters;

        private AgeClass ageclass = new AgeClass();

        //---------------------------------------------------------------------

        public PlugIn()
            : base(ExtensionName, Type)
        {
        }

        //---------------------------------------------------------------------

        public static ICore ModelCore
        {
            get
            {
                return modelCore;
            }
        }
        //---------------------------------------------------------------------

        public override void LoadParameters(string dataFile, ICore mCore)
        {
            modelCore = mCore;
            SiteVars.Initialize();
            InputParametersParser parser = new InputParametersParser();
            parameters = mCore.Load<IInputParameters>(dataFile, parser);
        }
        //---------------------------------------------------------------------

        public override void Initialize()
        {
            Timestep = parameters.Timestep;
            this.selectedSpecies = parameters.SelectedSpecies;
            this.speciesMapNameTemplate = parameters.SpeciesMapNames;
            this.ageClasses = parameters.AgeClasses;
        }

        //---------------------------------------------------------------------

        public override void Run()
        {

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
                    string path = MakeSpeciesMapName(species.Name, ageclass.Name);
                    ModelCore.Log.WriteLine("   Writing {0} and {1} map to {2} ...", species.Name, ageclass.Name, path);
                    using (IOutputRaster<ShortPixel> outputRaster = modelCore.CreateRaster<ShortPixel>(path, modelCore.Landscape.Dimensions))
                    {
                        ShortPixel pixel = outputRaster.BufferPixel;
                        foreach (Site site in modelCore.Landscape.AllSites)
                        {
                            if (site.IsActive)
                                pixel.MapCode.Value = (short)((float)Util.ComputeAgeClassBiomass(SiteVars.Cohorts[site][species], ageclass));
                            else
                                pixel.MapCode.Value = 0;

                            outputRaster.WriteBufferPixel();
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

    }
}
