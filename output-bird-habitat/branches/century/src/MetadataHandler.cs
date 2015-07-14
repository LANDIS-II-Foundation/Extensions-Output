using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Landis.Library.Metadata;
using Edu.Wisc.Forest.Flel.Util;
using Landis.Core;

namespace Landis.Extension.Output.BirdHabitat
{
    public static class MetadataHandler
    {

        
        public static ExtensionMetadata Extension {get; set;}

        public static void InitializeMetadata(int Timestep, string MapFileName, IEnumerable<IModelDefinition> modelDefs, ICore mCore, string LogFileName)
        {
            ScenarioReplicationMetadata scenRep = new ScenarioReplicationMetadata() {
                RasterOutCellArea = PlugIn.ModelCore.CellArea,
                TimeMin = PlugIn.ModelCore.StartTime,
                TimeMax = PlugIn.ModelCore.EndTime//,
                //ProjectionFilePath = "Projection.?" 
            };

            Extension = new ExtensionMetadata(mCore){
                Name = PlugIn.PlugInName,
                TimeInterval = Timestep, 
                ScenarioReplicationMetadata = scenRep
            };

            /*
            //---------------------------------------
            //          table outputs:   
            //---------------------------------------
            PlugIn.habitatLog = new MetadataTable<SpeciesHabitatLog>(LogFileName);
            OutputMetadata tblOut_events = new OutputMetadata()
            {
                Type = OutputType.Table,
                Name = "SppHabitatLog",
                FilePath = PlugIn.habitatLog.FilePath,
                Visualize = true,
            };
            tblOut_events.RetriveFields(typeof(SpeciesHabitatLog));
            Extension.OutputMetadatas.Add(tblOut_events);
            */

            //---------------------------------------            
            //          map outputs:         
            //---------------------------------------

            foreach (ModelDefinition sppModel in modelDefs)
            {
                
                OutputMetadata mapOut_Birds = new OutputMetadata()
                {
                    Type = OutputType.Map,
                    Name = sppModel.Name,
                    FilePath = @MapFileName,
                    Map_DataType = MapDataType.Continuous,
                    //Map_Unit = FieldUnits.Severity_Rank,
                    Visualize = true,
                };
                Extension.OutputMetadatas.Add(mapOut_Birds);
            }
            //---------------------------------------
            MetadataProvider mp = new MetadataProvider(Extension);
            mp.WriteMetadataToXMLFile("Metadata", Extension.Name, Extension.Name);




        }
    }
}
