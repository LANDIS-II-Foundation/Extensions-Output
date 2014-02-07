//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Robert M. Scheller, James B. Domingo

using Landis.Core;
using Edu.Wisc.Forest.Flel.Util;
using Landis.Library.BiomassCohortsPnET;
using Landis.SpatialModeling;
using Landis.Extension.Succession.BiomassPnET;

using System;
using System.Collections.Generic;
using System.IO;

namespace Landis.Extension.Output.BiomassPnET
{
    public class PlugIn
        : ExtensionMain
    {
        public static readonly ExtensionType Type = new ExtensionType("output");
        public static readonly string ExtensionName = "Output PnET";

        private string selectedPools;
        private string poolMapNameTemplate;
        private IInputParameters parameters;
        private static ICore modelCore;
        private bool makeTable;

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
            parameters = Landis.Data.Load<IInputParameters>(dataFile, parser);
        }

        //---------------------------------------------------------------------

        public override void Initialize()
        {
            Timestep = parameters.Timestep;

            OutputVariables.Initialize(parameters.SelectedSpecies, parameters);

            this.selectedPools = parameters.SelectedPools;
            this.poolMapNameTemplate = parameters.PoolMapNames;
            this.makeTable = parameters.MakeTable;

            SiteVars.Initialize();
        }

        //---------------------------------------------------------------------

        public override void Run()
        {
            OutputVariables.Update();
            
            WritePoolMaps();

        }

        
        //---------------------------------------------------------------------

        private void WritePoolMaps()
        {
            if(selectedPools == "woody" || selectedPools == "both")
                WritePoolMap("woody", SiteVars.WoodyDebris);

            if(selectedPools == "non-woody" || selectedPools == "both")
                WritePoolMap("non-woody", SiteVars.Litter);
        }

        //---------------------------------------------------------------------

        private void WritePoolMap(string         poolName,
                                  ISiteVar<Landis.Extension.Succession.Biomass.Pool> poolSiteVar)
        {
            string path = PoolMapNames.ReplaceTemplateVars(poolMapNameTemplate,
                                                           poolName,
                                                           PlugIn.ModelCore.CurrentTime);
            if(poolSiteVar != null)
            {
                Console.WriteLine("   Writing {0} biomass map to {1} ...", poolName, path);
                using (IOutputRaster<IntPixel> outputRaster = modelCore.CreateRaster<IntPixel>(path, modelCore.Landscape.Dimensions))
                {
                    IntPixel pixel = outputRaster.BufferPixel;
                    foreach (Site site in PlugIn.ModelCore.Landscape.AllSites)
                    {
                        if (site.IsActive)
                            pixel.MapCode.Value = (int)((float)poolSiteVar[site].Mass);
                        else
                            pixel.MapCode.Value = 0;

                        outputRaster.WriteBufferPixel();
                    }
                }
            }
        }
        
        
    }
}
