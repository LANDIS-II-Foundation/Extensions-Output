﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Landis.Library.Metadata;
using Edu.Wisc.Forest.Flel.Util;
using Landis.Core;

namespace Landis.Extension.Output.LeafBiomassReclass
{
    public static class MetadataHandler
    {
        
        public static ExtensionMetadata Extension {get; set;}

        public static void InitializeMetadata(int Timestep, IEnumerable<IMapDefinition> mapDefs, string mapNameTemplate, ICore mCore)
        {
            ScenarioReplicationMetadata scenRep = new ScenarioReplicationMetadata() {
                RasterOutCellArea = PlugIn.ModelCore.CellArea,
                TimeMin = PlugIn.ModelCore.StartTime,
                TimeMax = PlugIn.ModelCore.EndTime,
            };

            Extension = new ExtensionMetadata(mCore){
                Name = PlugIn.ExtensionName,
                TimeInterval = Timestep, 
                ScenarioReplicationMetadata = scenRep
            };

            //---------------------------------------            
            //          map outputs:         
            //---------------------------------------
            //PlugIn.ModelCore.UI.WriteLine("   Writing biomass maps ...");
            foreach (IMapDefinition map in mapDefs)
            {
                string mapTypePath = MapFileNames.ReplaceTemplateVarsMetadata(mapNameTemplate, map.Name);

                OutputMetadata mapOut_ForestType = new OutputMetadata()
                {
                    Type = OutputType.Map,
                    Name = (map.Name + " Forest Type Map"),
                    FilePath = @mapTypePath,
                    Map_DataType = MapDataType.Nominal,
                    Visualize = true//,
                };
                Extension.OutputMetadatas.Add(mapOut_ForestType);
            }
            //---------------------------------------
            MetadataProvider mp = new MetadataProvider(Extension);
            mp.WriteMetadataToXMLFile("Metadata", Extension.Name, Extension.Name);




        }
    }
}
