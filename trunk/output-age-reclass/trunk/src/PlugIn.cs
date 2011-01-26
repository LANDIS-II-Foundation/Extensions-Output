//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:   Robert M. Scheller, James B. Domingo

using Landis.SpatialModeling;
using Landis.Library.AgeOnlyCohorts;
using Landis.Core;

using System.Collections.Generic;
using System;

namespace Landis.Extension.Output.AgeReclass
{
    public class PlugIn
        : ExtensionMain
    {
        public static readonly ExtensionType Type = new ExtensionType("output");
        public static readonly string ExtensionName = "Output Age Reclass";

        private string mapNameTemplate;
        private IEnumerable<IMapDefinition> mapDefs;
        private double[] reclassCoefs;
        private IInputParameters parameters;
        private static ICore modelCore;


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
            SiteVars.Initialize();
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

                string path = MapFiles.ReplaceTemplateVars(mapNameTemplate, map.Name, modelCore.CurrentTime);
                modelCore.Log.WriteLine("   Writing age reclass map {0} to {1} ...", map.Name, path);
                using (IOutputRaster<BytePixel> outputRaster = modelCore.CreateRaster<BytePixel>(path, modelCore.Landscape.Dimensions))
                {
                    BytePixel pixel = outputRaster.BufferPixel;
                    foreach (Site site in modelCore.Landscape.AllSites)
                    {
                        if (site.IsActive)
                            pixel.MapCode.Value = CalcForestType(site,forestTypes);
                        else
                            pixel.MapCode.Value = 0;

                        outputRaster.WriteBufferPixel();
                    }
                }

            }

        }

        //---------------------------------------------------------------------

        private byte CalcForestType(Site site, List<IForestType> forestTypes)
        {
            int forTypeCnt = 0;

            double[] forTypValue = new double[forestTypes.Count];
            foreach (ISpecies species in PlugIn.ModelCore.Species)
            {
                if (SiteVars.Cohorts[site] != null)
                {
                    ushort maxSpeciesAge = 0;
                double sppValue = 0.0;
                maxSpeciesAge = Util.GetMaxAge(SiteVars.Cohorts[site][species]);

                if (maxSpeciesAge > 0)
                {
                    sppValue = (double)maxSpeciesAge /
                        (double)species.Longevity *
                        (double)reclassCoefs[species.Index];

                    forTypeCnt = 0;
                    foreach (IForestType ftype in forestTypes)
                    {
                        if (ftype[species.Index] != 0)
                        {
                            if (ftype[species.Index] == -1)
                                forTypValue[forTypeCnt] -= sppValue;
                            if (ftype[species.Index] == 1)
                                forTypValue[forTypeCnt] += sppValue;
                        }
                        forTypeCnt++;
                    }
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
