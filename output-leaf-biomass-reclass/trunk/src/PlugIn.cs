//  Copyright 2005-2013 Portland State University
//  Authors:  Robert M. Scheller

using Landis.Core;
using Landis.Library.LeafBiomassCohorts;
using Landis.SpatialModeling;
using Landis.Library.Metadata;
using System.Collections.Generic;
using System;

namespace Landis.Extension.Output.LeafBiomassReclass
{
    public class PlugIn
        : ExtensionMain
    {

        public static readonly ExtensionType type = new ExtensionType("output");
        public static readonly string ExtensionName = "Output Leaf Biomass Reclass";
        public static MetadataTable<ForestTypeLog>[] individualForestTypes;

        private string mapNameTemplate;
        private IEnumerable<IMapDefinition> mapDefs;

        private static IInputParameters parameters;
        private static ICore modelCore;


        //---------------------------------------------------------------------

        public PlugIn()
            : base(ExtensionName, type)
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
            parameters = Landis.Data.Load<IInputParameters>(dataFile, parser);

        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Initializes the component with a data file.
        /// </summary>
        public override void Initialize()
        {

            Timestep = parameters.Timestep;
            SiteVars.Initialize();
            this.mapNameTemplate = parameters.MapFileNames;
            this.mapDefs = parameters.ReclassMaps;
            MetadataHandler.InitializeMetadata(parameters.Timestep, this.mapDefs, this.mapNameTemplate, ModelCore);

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

                string path = MapFileNames.ReplaceTemplateVars(mapNameTemplate, map.Name, modelCore.CurrentTime);
                modelCore.UI.WriteLine("   Writing Biomass Reclass map to {0} ...", path);
                using (IOutputRaster<BytePixel> outputRaster = modelCore.CreateRaster<BytePixel>(path, modelCore.Landscape.Dimensions))
                {
                    BytePixel pixel = outputRaster.BufferPixel;
                    foreach (Site site in modelCore.Landscape.AllSites)
                    {
                        if (site.IsActive)
                            pixel.MapCode.Value = CalcForestType(forestTypes, site);
                        else
                            pixel.MapCode.Value = 0;
                        
                        outputRaster.WriteBufferPixel();
                    }
                }

            }

            foreach (IMapDefinition map in mapDefs)
            {
                List<IForestType> forestTypes = map.ForestTypes;
                int[] arrayOfForestTypes = new int[50];

                foreach (ActiveSite site in ModelCore.Landscape)
                {
                    int ftypeFinal = (int) CalcForestType(forestTypes, site);

                    int forTypeCnt2 = 0;
                    foreach (IForestType ftype in forestTypes)
                    {
                        if (ftypeFinal == forTypeCnt2 -1)
                        {
                            arrayOfForestTypes[forTypeCnt2]++;
                            break;
                        }
                        forTypeCnt2++;
                    }
                }

                int forTypeCnt = 0;
                foreach (IForestType ftype in forestTypes)
                {
                    // log stuff here
                    forTypeCnt++;
                }

                break; // Only do the first one.
            }

        }


        //---------------------------------------------------------------------

        private byte CalcForestType(List<IForestType> forestTypes,
                                    Site site)
        {
            int forTypeCnt = 0;

            double[] forTypValue = new double[forestTypes.Count];

            foreach(ISpecies species in modelCore.Species)
            {
                double sppValue = 0.0;

                if (SiteVars.Cohorts[site] == null)
                    break;

                sppValue = Util.ComputeBiomass(SiteVars.Cohorts[site][species]);

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
