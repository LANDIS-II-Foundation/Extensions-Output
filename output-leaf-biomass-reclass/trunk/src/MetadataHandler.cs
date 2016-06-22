using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.IO;
using Landis.Library.Metadata;
using Edu.Wisc.Forest.Flel.Util;
using Landis.Core;

namespace Landis.Extension.Output.LeafBiomassReclass
{
    public static class MetadataHandler
    {
        
        public static ExtensionMetadata Extension {get; set;}

        public static void InitializeMetadata(int Timestep, IEnumerable<IMapDefinition> mapDefs, string mapNameTemplate)
        {
            ScenarioReplicationMetadata scenRep = new ScenarioReplicationMetadata() {
                RasterOutCellArea = PlugIn.ModelCore.CellArea,
                TimeMin = PlugIn.ModelCore.StartTime,
                TimeMax = PlugIn.ModelCore.EndTime,
            };

            Extension = new ExtensionMetadata(PlugIn.ModelCore){
                Name = PlugIn.ExtensionName,
                TimeInterval = Timestep, 
                ScenarioReplicationMetadata = scenRep
            };

            //---------------------------------------
            //          table outputs:   
            //---------------------------------------

            //PlugIn.individualForestTypeLog = new MetadataTable<ForestTypeLog>[50];
            //foreach (IMapDefinition map in mapDefs)
            //{
            //    int forestTypesCnt = 0;
            //    List<IForestType> forestTypes = map.ForestTypes;
            //    foreach (IForestType ftype in forestTypes)
            //    {

            //        string forestTypeLogName = ("output-leaf-biomass-reclass/" + ftype.Name + "-forest-type-log.csv");
            //        CreateDirectory(forestTypeLogName);
            //        PlugIn.individualForestTypeLog[forestTypesCnt] = new MetadataTable<ForestTypeLog>(forestTypeLogName);

            //        OutputMetadata tblOut_events = new OutputMetadata()
            //        {
            //            Type = OutputType.Table,
            //            Name = "ForestTypeCountLog",
            //            FilePath = PlugIn.individualForestTypeLog[forestTypesCnt].FilePath,
            //            Visualize = true
            //        };
            //        tblOut_events.RetriveFields(typeof(ForestTypeLog));
            //        Extension.OutputMetadatas.Add(tblOut_events);

            //        forestTypesCnt++;
            //    }
            //    break;  // only the first one.
            //}

            // RMS 03/2016: Added dynamic column names.
            PlugIn.individualMapDefLog = new MetadataTable<MapDefLog>[50];
            int mapDefCnt = 0;
            foreach (IMapDefinition map in mapDefs)
            {
                int forestTypeCnt = 0;
                List<string> forestTypeNames = new List<string>();
                foreach (IForestType ftype in map.ForestTypes)
                {
                    forestTypeNames.Add(ftype.Name);
                    forestTypeCnt++;
                }

                PlugIn.forestTypeNames[mapDefCnt] = forestTypeNames;
                ExtensionMetadata.ColumnNames = PlugIn.forestTypeNames[mapDefCnt];

                string forestTypeLogName = ("output-leaf-biomass-reclass/" + map.Name + "-forest-type-log.csv");
                CreateDirectory(forestTypeLogName);
                PlugIn.individualMapDefLog[mapDefCnt] = new MetadataTable<MapDefLog>(forestTypeLogName);

                OutputMetadata tblOut_events = new OutputMetadata()
                {
                    Type = OutputType.Table,
                    Name = "ForestTypeCountLog",
                    FilePath = PlugIn.individualMapDefLog[mapDefCnt].FilePath,
                    Visualize = true
                };
                tblOut_events.RetriveFields(typeof(MapDefLog));
                Extension.OutputMetadatas.Add(tblOut_events);

                mapDefCnt++;
            }
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
        public static void CreateDirectory(string path)
        {
            //Require.ArgumentNotNull(path);
            path = path.Trim(null);
            if (path.Length == 0)
                throw new ArgumentException("path is empty or just whitespace");

            string dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir))
            {
                Edu.Wisc.Forest.Flel.Util.Directory.EnsureExists(dir);
            }

            //return new StreamWriter(path);
            return;
        }
    }
}
