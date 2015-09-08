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
    public class MetadataHandler
    {
        
        public static ExtensionMetadata Extension {get; set;}

        public static void InitializeMetadata(int Timestep, IEnumerable<ISpecies> selectedSpecies, string sppMapNames, ICore mCore)
        {
            ScenarioReplicationMetadata scenRep = new ScenarioReplicationMetadata() {
                RasterOutCellArea = PlugIn.ModelCore.CellArea,
                TimeMin = PlugIn.ModelCore.StartTime,
                TimeMax = PlugIn.ModelCore.EndTime
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
                FilePath = PlugIn.sppBiomassLog.FilePath,
                Visualize = true
            };
            tblOut_events.RetriveFields(typeof(SppBiomassLog));
            Extension.OutputMetadatas.Add(tblOut_events);

            PlugIn.individualBiomassLog = new MetadataTable<SppBiomassLog>[50];
            int selectSppCnt = 0;

            foreach (ISpecies species in selectedSpecies)
            {
                PlugIn.individualBiomassLog[selectSppCnt] = new MetadataTable<SppBiomassLog>(species.Name + "-biomass-log.csv");
                selectSppCnt++;

                tblOut_events = new OutputMetadata()
                {
                    Type = OutputType.Table,
                    Name = (species.Name + "BiomassLog"),
                    FilePath = PlugIn.sppBiomassLog.FilePath,
                    Visualize = true
                };
                tblOut_events.RetriveFields(typeof(SppBiomassLog));
                Extension.OutputMetadatas.Add(tblOut_events);
            }

            //---------------------------------------            
            //          map outputs:         
            //---------------------------------------
            //PlugIn.ModelCore.UI.WriteLine("   Writing biomass maps ...");
            if(PlugIn.MakeMaps)
            {
                foreach (ISpecies species in selectedSpecies)
                {
                    string sppMapPath = MapNames.ReplaceTemplateVars(sppMapNames, species.Name);

                    OutputMetadata mapOut_SppBiomass = new OutputMetadata()
                    {
                        Type = OutputType.Map,
                        Name = ("Species Biomass Map: " + species.Name),
                        FilePath = @sppMapPath,
                        Map_DataType = MapDataType.Continuous,
                        Map_Unit = FieldUnits.g_B_m2,
                        Visualize = true
                    };
                    Extension.OutputMetadatas.Add(mapOut_SppBiomass);
                }
            }

            string totalBioMapPath = MapNames.ReplaceTemplateVars(sppMapNames, "TotalBiomass");

            OutputMetadata mapOut_TotalBiomass = new OutputMetadata()
            {
                Type = OutputType.Map,
                Name = ("Total Biomass Map"),
                FilePath = @totalBioMapPath,
                Map_DataType = MapDataType.Continuous,
                Map_Unit = FieldUnits.g_B_m2,
                Visualize = true
            };
            Extension.OutputMetadatas.Add(mapOut_TotalBiomass);
            
            //---------------------------------------
            MetadataProvider mp = new MetadataProvider(Extension);
            mp.WriteMetadataToXMLFile("Metadata", Extension.Name, Extension.Name);




        }
    }
}
