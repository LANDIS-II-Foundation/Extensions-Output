//  Copyright 2005 University of Wisconsin-Madison
//  Authors:  Jimm Domingo
//  License:  Available at  
//  http://landis.forest.wisc.edu/developers/LANDIS-IISourceCodeLicenseAgreement.pdf

using Landis.AgeCohort;
using Landis.Landscape;
using Landis.RasterIO;
using Landis.Species;

using System.Collections.Generic;
using System;

namespace Landis.Output.Reclass
{
    public class PlugIn
        : PlugIns.PlugIn
    {
        private PlugIns.ICore modelCore;
        private string mapNameTemplate;
        //private IEnumerable<ISpecies> selectedSpecies;
        private IEnumerable<IMapDefinition> mapDefs;
        private ILandscapeCohorts cohorts;
        private double[] reclassCoefs;

        //---------------------------------------------------------------------

        public PlugIn()
            : base("Reclass Output", new PlugIns.PlugInType("output"))
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
            mapNameTemplate = parameters.MapFileNames;
            mapDefs = parameters.ReclassMaps;
            reclassCoefs = parameters.ReclassCoefficients;

            cohorts = modelCore.SuccessionCohorts as ILandscapeCohorts;
            if (cohorts == null)
                throw new ApplicationException("Error: Cohorts don't support age-cohort interface");
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

                //Erdas74TrailerFile.Write(path, map.ForestTypes);
            }

        }

        //---------------------------------------------------------------------

        private IOutputRaster<ClassPixel> CreateMap(string mapname)
        {
            string path = MapFileNames.ReplaceTemplateVars(mapNameTemplate, mapname, modelCore.CurrentTime);
            UI.WriteLine("Writing reclass map to {0} ...", path);
            return modelCore.CreateRaster<ClassPixel>(path,
                                                    modelCore.Landscape.Dimensions,
                                                    modelCore.LandscapeMapMetadata);
        }

        //---------------------------------------------------------------------

        private byte CalcForestType(Site site, IForestType[] forestTypes)
        {
            int forTypeCnt = 0;

            double[] forTypValue = new double[forestTypes.Length];
            Species.IDataset SpeciesDataset = modelCore.Species;
            foreach(ISpecies species in SpeciesDataset)
            {
                ushort maxSpeciesAge = 0;
                double sppValue = 0.0;
                //ISpeciesCohorts speciesCohorts = cohorts[species];
                maxSpeciesAge = AgeCohort.Util.GetMaxAge(cohorts[site][species]);
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

        //---------------------------------------------------------------------
/*
        private ushort MaxAge(ISpeciesCohorts cohorts)
        {
            if (cohorts == null)
                return 0;
            ushort max = 0;
            foreach (ushort age in cohorts.Ages)
                if (age > max)
                    max = age;
            return max;
        }
        //---------------------------------------------------------------------
*/
    }
}
