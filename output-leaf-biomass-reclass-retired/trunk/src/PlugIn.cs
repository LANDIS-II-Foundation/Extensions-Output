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
        //public static MetadataTable<ForestTypeLog> forestTypeLog;
        public static MetadataTable<MapDefLog>[] individualMapDefLog;
        public static List<string>[] forestTypeNames = new List<string>[20];

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
             MetadataHandler.InitializeMetadata(parameters.Timestep, this.mapDefs, this.mapNameTemplate);

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
                            pixel.MapCode.Value = (byte) (CalcForestType(forestTypes, site) +  1);
                        else
                            pixel.MapCode.Value = 0;
                        
                        outputRaster.WriteBufferPixel();
                    }
                }

            }

            int mapDefCnt = 0;
            foreach (IMapDefinition map in mapDefs)
            {
                List<IForestType> forestTypes = map.ForestTypes;
                ExtensionMetadata.ColumnNames = PlugIn.forestTypeNames[mapDefCnt];

                double[] arrayOfForestTypes = new double[forestTypes.Count];

                foreach (ActiveSite site in ModelCore.Landscape)
                {
                    int ftypeFinal = (int) CalcForestType(forestTypes, site);
                    arrayOfForestTypes[ftypeFinal]++;
                }

                individualMapDefLog[mapDefCnt].Clear();
                MapDefLog mdl = new MapDefLog();
                mdl.Time = ModelCore.CurrentTime;
                mdl.ForestTypeCnt_ = arrayOfForestTypes;
                individualMapDefLog[mapDefCnt].AddObject(mdl);
                individualMapDefLog[mapDefCnt].WriteToFile();

                mapDefCnt++;
            }

        }


        //---------------------------------------------------------------------

        private int CalcForestType(List<IForestType> forestTypes, Site site)
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
                    finalForestType = forTypeCnt;
                }
                forTypeCnt++;
            }
            return finalForestType;
        }


    }
}
