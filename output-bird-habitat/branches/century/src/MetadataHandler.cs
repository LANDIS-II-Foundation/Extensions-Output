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

        public static void InitializeMetadata(int Timestep, string MapFileName, IEnumerable<IModelDefinition> modelDefs, ICore mCore)
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

            //---------------------------------------
            //          table outputs:   
            //---------------------------------------

            // NO LOG FILE PlugIn.eventLog = new MetadataTable<EventsLog>("wind-events-log.csv");

            //OutputMetadata tblOut_events = new OutputMetadata()
            //{
            //    Type = OutputType.Table,
            //    Name = "WindLog",
            //    FilePath = PlugIn.eventLog.FilePath,
            //    Visualize = false,
            //};
            //tblOut_events.RetriveFields(typeof(EventsLog));
            //Extension.OutputMetadatas.Add(tblOut_events);


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
