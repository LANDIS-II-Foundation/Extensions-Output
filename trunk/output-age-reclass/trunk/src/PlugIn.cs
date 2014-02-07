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
        public static readonly ExtensionType ExtType = new ExtensionType("output");
        public static readonly string ExtensionName = "Output Age Reclass";

        private string mapNameTemplate;
        private IEnumerable<IMapDefinition> mapDefs;
        private double[] reclassCoefs;
        private IInputParameters parameters;
        private static ICore modelCore;


        //---------------------------------------------------------------------

        public PlugIn()
            : base(ExtensionName, ExtType)
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
            parameters = Landis.Data.Load<IInputParameters>(dataFile, parser);
        }

        //---------------------------------------------------------------------

        public override void Initialize()
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
                modelCore.UI.WriteLine("   Writing age reclass map {0} to {1} ...", map.Name, path);
                using (IOutputRaster<BytePixel> outputRaster = modelCore.CreateRaster<BytePixel>(path, modelCore.Landscape.Dimensions))
                {
                    BytePixel pixel = outputRaster.BufferPixel;
                    foreach (Site site in modelCore.Landscape.AllSites)
                    {
                        if (site.IsActive)
                            pixel.MapCode.Value = CalcForestType(site, forestTypes);
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
                    maxSpeciesAge = GetSppMaxAge(site, species);


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
                if(forTypValue[forTypeCnt] > maxValue)
                {
                    maxValue = forTypValue[forTypeCnt];
                    finalForestType = forTypeCnt+1;
                }
                ModelCore.UI.WriteLine("ftype={0}, value={1}.", ftype.Name, forTypValue[forTypeCnt]);
                forTypeCnt++;
            }
            return (byte) finalForestType;
        }

        //---------------------------------------------------------------------
        public static ushort GetSppMaxAge(Site site, ISpecies spp)
        {
            if (!site.IsActive)
                return 0;

            if (SiteVars.Cohorts[site] == null)
            {
                PlugIn.ModelCore.UI.WriteLine("Cohort are null.");
                return 0;
            }
            ushort max = 0;

            foreach (ISpeciesCohorts sppCohorts in SiteVars.Cohorts[site])
            {
                if (sppCohorts.Species == spp)
                {
                    //ModelCore.UI.WriteLine("cohort spp = {0}, compare species = {1}.", sppCohorts.Species.Name, spp.Name);
                    foreach (ICohort cohort in sppCohorts)
                        if (cohort.Age > max)
                            max = cohort.Age;
                }
            }
            return max;
        }
    }
}
