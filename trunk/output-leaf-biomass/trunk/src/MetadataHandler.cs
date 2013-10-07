using System;
using System.Collections.Generic;
using System.Linq;
//using System.Data;
using System.Text;
using Landis.Library.Metadata;
using Edu.Wisc.Forest.Flel.Util;
using Landis.Core;

namespace Landis.Extension.Output.LeafBiomass
{
    public static class MetadataHandler
    {
        
        public static ExtensionMetadata Extension {get; set;}

        public static void InitializeMetadata(int Timestep, IEnumerable<ISpecies> selectedSpecies, string sppMapNames, ICore mCore)
        {
            ScenarioReplicationMetadata scenRep = new ScenarioReplicationMetadata() {
                //String outputFolder = OutputPath.ReplaceTemplateVars("", FINISH ME LATER);
                FolderName = System.IO.Directory.GetCurrentDirectory().Split("\\".ToCharArray()).Last(),
                RasterOutCellArea = PlugIn.ModelCore.CellArea,
                TimeMin = PlugIn.ModelCore.StartTime,
                TimeMax = PlugIn.ModelCore.EndTime,
                ProjectionFilePath = "Projection.?" //How do we get projections???
            };

            Extension = new ExtensionMetadata(mCore){
                Name = PlugIn.ExtensionName,
                TimeInterval = Timestep, 
                ScenarioReplicationMetadata = scenRep
            };

            //---------------------------------------
            //          table outputs:   
            //---------------------------------------

            PlugIn.sppBiomassLog = new MetadataTable<SppBiomassLog>("spp-biomass-log.csv");

            OutputMetadata tblOut_events = new OutputMetadata()
            {
                Type = OutputType.Table,
                Name = "SppBiomassLog",
                FilePath = PlugIn.sppBiomassLog.FilePath//,
                //MetadataFilePath = @"Base-Wind\EventLog.xml"
            };
            tblOut_events.RetriveFields(typeof(SppBiomassLog));
            Extension.OutputMetadatas.Add(tblOut_events);
            


            //---------------------------------------            
            //          map outputs:         
            //---------------------------------------
            //PlugIn.ModelCore.UI.WriteLine("   Writing biomass maps ...");
            foreach (ISpecies species in selectedSpecies)
            {
                string sppMapPath = MapNames.ReplaceTemplateVars(sppMapNames, species.Name);

                OutputMetadata mapOut_Severity = new OutputMetadata()
                {
                    Type = OutputType.Map,
                    Name = ("Species Biomass Map: " + species.Name),
                    FilePath = @sppMapPath,
                    Map_DataType = MapDataType.Nominal,
                    Map_Unit = FiledUnits.g_B_m_2
                };
                Extension.OutputMetadatas.Add(mapOut_Severity);
            }
            //---------------------------------------
            MetadataProvider mp = new MetadataProvider(Extension);
            mp.WriteMetadataToXMLFile("Metadata", Extension.Name, Extension.Name);




        }
    }
}
