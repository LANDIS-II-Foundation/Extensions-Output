//  Copyright 2005-2006 University of Wisconsin-Madison
//  Authors:  Jimm Domingo, Robert M. Scheller
//  License:  Available at  
//  http://landis.forest.wisc.edu/developers/LANDIS-IISourceCodeLicenseAgreement.pdf

using Landis.Biomass;
//using Landis.AgeCohort;
using Landis.Landscape;
using Landis.RasterIO;
using Landis.Species;


using System.Collections.Generic;
using System;

namespace Landis.Output.BiomassReclass
{
    public class PlugIn
        : Landis.PlugIns.PlugIn
    {
        private string mapNameTemplate;
        private IEnumerable<IMapDefinition> mapDefs;
        private PlugIns.ICore modelCore;
        private ILandscapeCohorts cohorts;

        //---------------------------------------------------------------------

        public PlugIn()
            : base("Reclass Biomass Output", new PlugIns.PlugInType("output"))
        {
        }
        //---------------------------------------------------------------------

        /// <summary>
        /// Initializes the component with a data file.
        /// </summary>
        /// <param name="dataFile">
        /// Path to the file with initialization data.
        /// </param>
        /// <param name="startTime">
        /// Initial timestep (year): the timestep that will be passed to the
        /// first call to the component's Run method.
        /// </param>
        public override void Initialize(string dataFile,
                                PlugIns.ICore modelCore)
        {
            
            this.modelCore = modelCore;

            ParametersParser.SpeciesDataset = modelCore.Species;
            ParametersParser parser = new ParametersParser();
            IParameters parameters = Data.Load<IParameters>(dataFile,
                                                            parser);
            Timestep = parameters.Timestep;
            //this.nextTimeToRun = startTime - 1;

            this.mapNameTemplate = parameters.MapFileNames;
            this.mapDefs = parameters.ReclassMaps;
            
            cohorts = modelCore.SuccessionCohorts as ILandscapeCohorts;
            if (cohorts == null)
                throw new ApplicationException("Error: Cohorts don't support biomass interface");

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
                IForestType[] forestTypes = map.ForestTypes;
                
                string path = MapFileNames.ReplaceTemplateVars(mapNameTemplate, map.Name, modelCore.CurrentTime);
                
                IOutputRaster<ClassPixel> newmap = CreateMap(path);
                using (newmap) {
                    ClassPixel pixel = new ClassPixel();
                    foreach (Site site in modelCore.Landscape.AllSites) {
                        if (site.IsActive)
                            pixel.Band0 = CalcForestType(forestTypes, site);
                        else
                            pixel.Band0 = 0;
                        newmap.WritePixel(pixel);
                    }
                }

                //Erdas74TrailerFile.Write(path, map.ForestTypes);
            }

        }

        //---------------------------------------------------------------------

        private IOutputRaster<ClassPixel> CreateMap(string path)
        {
            UI.WriteLine("Writing reclass map to {0} ...", path);
            return modelCore.CreateRaster<ClassPixel>(path,
                                                modelCore.Landscape.Dimensions,
                                                modelCore.LandscapeMapMetadata);
        }

        //---------------------------------------------------------------------

        private byte CalcForestType(IForestType[] forestTypes,
                                    Site site)
        {
            int forTypeCnt = 0;

            double[] forTypValue = new double[forestTypes.Length];
            Species.IDataset SpeciesDataset = modelCore.Species;
            foreach(ISpecies species in SpeciesDataset)
            {
                double sppValue = 0.0;
                
                //Biomass.ICohort cohort = cohorts[site][species];

                //sppValue = cohort.Biomass;//Util.ComputeBiomass(cohorts[site][species]);
                sppValue = Util.ComputeBiomass(cohorts[site][species]);

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
