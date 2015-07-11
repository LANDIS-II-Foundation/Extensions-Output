//  Copyright 2005 University of Wisconsin-Madison
//  Authors:  Jimm Domingo
//  License:  Available at
//  http://landis.forest.wisc.edu/developers/LANDIS-IISourceCodeLicenseAgreement.pdf

using Wisc.Flel.GeospatialModeling.Landscapes;
using Landis.Library.BaseCohorts;
//using Landis.Landscape;
//using Landis.RasterIO;
using Landis.Core;
using Wisc.Flel.GeospatialModeling.RasterIO;
//using Landis.Species;

using System.Collections.Generic;
using System;

namespace Landis.Extension.Output.Reclass
{
    public class PlugIn
        : ExtensionMain
    {
        public static readonly string PlugInName = "Age Reclass Output";
        private string mapNameTemplate;
        private IEnumerable<IMapDefinition> mapDefs;
        private double[] reclassCoefs;
        private IInputParameters parameters;
        private static ICore modelCore;


        //---------------------------------------------------------------------

        public PlugIn()
            //: base("Age Reclass Output", new PlugInType("output"))
            : base("Age Reclass Output", new ExtensionType("output"))
        {
        }

        //---------------------------------------------------------------------
        public override void LoadParameters(string dataFile,
                                            ICore mCore)
        {
            modelCore = mCore;
            InputParametersParser.SpeciesDataset = modelCore.Species;
            InputParametersParser parser = new InputParametersParser();
            parameters = modelCore.Load<IInputParameters>(dataFile, parser);
        }

        //---------------------------------------------------------------------

        public override void Initialize(string dataFile)
        {
            Timestep = parameters.Timestep;
            mapNameTemplate = parameters.MapFileNames;
            mapDefs = parameters.ReclassMaps;
            reclassCoefs = parameters.ReclassCoefficients;
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Runs the component for a particular timestep.
        /// </summary>
        /// <param name="currentTime">
        /// The current model timestep.
        /// </param>
        public override void Run()
        {
            foreach (IMapDefinition map in mapDefs)
            {
                List<IForestType> forestTypes = map.ForestTypes;
                IOutputRaster<ClassPixel> newmap = CreateMap(map.Name);
                string path = newmap.Path;
                using (newmap) {
                    ClassPixel pixel = new ClassPixel();
                    foreach (Site site in modelCore.Landscape.AllSites) {
                        if (site.IsActive)
                            pixel.Band0 = CalcForestType(site,forestTypes);
                        else
                            pixel.Band0 = 0;
                        newmap.WritePixel(pixel);
                    }
                }

            }

        }

        //---------------------------------------------------------------------

        private IOutputRaster<ClassPixel> CreateMap(string mapname)
        {
            string path = MapFileNames.ReplaceTemplateVars(mapNameTemplate, mapname, modelCore.CurrentTime);
            modelCore.Log.WriteLine("Writing reclass map to {0} ...", path);
            return modelCore.CreateRaster<ClassPixel>(path,
                                                    modelCore.Landscape.Dimensions,
                                                    modelCore.LandscapeMapMetadata);
        }

        //---------------------------------------------------------------------

        private byte CalcForestType(Site site, List<IForestType> forestTypes)
        {
            int forTypeCnt = 0;

            double[] forTypValue = new double[forestTypes.Count];
            ISpeciesDataset SpeciesDataset = modelCore.Species;
            foreach(ISpecies species in SpeciesDataset)
            {
                ushort maxSpeciesAge = 0;
                double sppValue = 0.0;
                //ISpeciesCohorts speciesCohorts = cohorts[species];
//                maxSpeciesAge = Library.Cohort.AgeOnly.Util.GetMaxAge(cohorts[site][species]);
                //MaxAge(speciesCohorts);

                if(maxSpeciesAge > 0)
                {
                    sppValue = (double) maxSpeciesAge /
                        (double) species.Longevity *
                        (double) reclassCoefs[species.Index];

                    forTypeCnt = 0;
                    foreach(IForestType ftype in forestTypes)
                    {
                        if(ftype[species.Index] != 0)
                        {
                            if(ftype[species.Index] == -1)
                                forTypValue[forTypeCnt] -= sppValue;
                            if(ftype[species.Index] == 1)
                                forTypValue[forTypeCnt] += sppValue;
                        }
                        forTypeCnt++;
                    }
                }
            }

            int finalForestType = 0;
            double maxValue = 0.0;
            forTypeCnt = 0;
            foreach(IForestType ftype in forestTypes)
            {
                //System.Console.WriteLine("ForestTypeNum={0}, Value={1}.",forTypeCnt,forTypValue[forTypeCnt]);
                if(forTypValue[forTypeCnt]>maxValue)
                {
                    maxValue = forTypValue[forTypeCnt];
                    finalForestType = forTypeCnt+1;
                }
                forTypeCnt++;
            }
            return (byte) finalForestType;
        }

    }
}
