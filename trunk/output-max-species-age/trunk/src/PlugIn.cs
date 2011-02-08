//  Copyright 2005-2010 Portland State University, University of Wisconsin-Madison
//  Authors:  Robert M. Scheller, James Domingo

using Landis.Core;
using Landis.Library.AgeOnlyCohorts;
using Landis.SpatialModeling;

using System.Collections.Generic;
using System;

namespace Landis.Extension.Output.MaxSpeciesAge
{
    public class PlugIn
        : ExtensionMain
    {
        public static readonly ExtensionType Type = new ExtensionType("output");
        public static readonly string PlugInName = "Output Max Species Age";

        private string mapNameTemplate;
        private IEnumerable<ISpecies> selectedSpecies;

        private IInputParameters parameters;
        private static ICore modelCore;


        //---------------------------------------------------------------------

        public PlugIn()
            : base(PlugInName, Type)
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

            InputParametersParser.SpeciesDataset = modelCore.Species;
            InputParametersParser parser = new InputParametersParser();
            parameters = modelCore.Load<IInputParameters>(dataFile, parser);

        }

        //---------------------------------------------------------------------

        public override void Initialize()
        {
            SiteVars.Initialize();

            Timestep = parameters.Timestep;
            mapNameTemplate = parameters.MapNames;
            selectedSpecies = parameters.SelectedSpecies;

        }

        //---------------------------------------------------------------------

        public override void Run()
        {
            //if keyword == maxage
            foreach (ISpecies species in selectedSpecies) {
                string path = MapNameTemplates.ReplaceTemplateVars(mapNameTemplate, species.Name, modelCore.CurrentTime);
                modelCore.Log.WriteLine("   Writing maximum age map for {0} to {1} ...", species.Name, path);
                using (IOutputRaster<UShortPixel> outputRaster = modelCore.CreateRaster<UShortPixel>(path, modelCore.Landscape.Dimensions))
                {
                    UShortPixel pixel = outputRaster.BufferPixel;
                    foreach (Site site in modelCore.Landscape.AllSites)
                    {
                        if (site.IsActive)
                            pixel.MapCode.Value = SiteVars.GetMaxAge(species, (ActiveSite) site);
                        else
                            pixel.MapCode.Value = 0;

                        outputRaster.WriteBufferPixel();
                    }
                }
            }

            WriteMapWithMaxAgeAmongAll();
        }

        //---------------------------------------------------------------------

        private void WriteMapWithMaxAgeAmongAll()
        {
            //    Maximum age map for all species
            string path = MapNameTemplates.ReplaceTemplateVars(mapNameTemplate, "AllSppMaxAge", modelCore.CurrentTime);
            modelCore.Log.WriteLine("   Writing maximum age map for all species to {0} ...", path);
            using (IOutputRaster<UShortPixel> outputRaster = modelCore.CreateRaster<UShortPixel>(path, modelCore.Landscape.Dimensions))
            {
                UShortPixel pixel = outputRaster.BufferPixel;
                foreach (Site site in modelCore.Landscape.AllSites)
                {
                    if (site.IsActive)
                        pixel.MapCode.Value = SiteVars.GetMaxAge((ActiveSite) site);
                    else
                        pixel.MapCode.Value = 0;

                    outputRaster.WriteBufferPixel();
                }
            }
        }

    }
}

