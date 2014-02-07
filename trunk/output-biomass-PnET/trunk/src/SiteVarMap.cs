using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Landis.Core;
using Edu.Wisc.Forest.Flel.Util;
using Landis.Library.BiomassCohortsPnET;
using Landis.SpatialModeling;
using Landis.Extension.Succession.BiomassPnET;
using System.IO;

namespace Landis.Extension.Output.BiomassPnET
{
    public class SiteVarMap
    {
        DelegateFunctions.GetValue getvalue;
        
        string MapNameTemplate;
        string label;
        public SiteVarMap(DelegateFunctions.GetValue getvalue, string MapNameTemplate, string label)
        {
            this.getvalue = getvalue;
            this.MapNameTemplate = MapNameTemplate;
            this.label = label;
        }
        private string MakeMapName(string label)
        {
            return SpeciesMapNames.ReplaceTemplateVars(MapNameTemplate, label, PlugIn.ModelCore.CurrentTime);
        }
        private string MakeValueTableName(string label)
        {
            return  SpeciesMapNames.ReplaceTemplateVars(MapNameTemplate, label, PlugIn.ModelCore.CurrentTime).Replace(".img",".txt");
        }
        public void WriteValues()
        {
            List<string> values = new List<string>();
            values.Add("Ecoregion\t" + label);

            string path = "NO_PATHNAME";
            try
            {
                path = MakeValueTableName(label);
                 
                foreach (Site site in PlugIn.ModelCore.Landscape.AllSites)
                {
                    if (site.IsActive)values.Add(PlugIn.ModelCore.Ecoregion[site]+"\t" +getvalue(site).ToString());
                    
                }
                System.IO.File.WriteAllLines(path, values.ToArray());
            }
            catch (System.Exception e)
            {
                throw new System.Exception("Cannot write " + path + " " + e.Message);
            }

        }
        public void WriteMap()
        {
            string path="";
            try
            { 
                path = MakeMapName(label);
                using (IOutputRaster<IntPixel> outputRaster = PlugIn.ModelCore.CreateRaster<IntPixel>(path, PlugIn.ModelCore.Landscape.Dimensions))
                {
                    IntPixel pixel = outputRaster.BufferPixel;
                    foreach (Site site in PlugIn.ModelCore.Landscape.AllSites)
                    {
                        if (site.IsActive)
                        {
                            try
                            {
                                pixel.MapCode.Value = (int)getvalue(site);
                            }
                            catch (System.Exception e)
                            {
                                System.Console.WriteLine("Cannot write " + path + " " + e.Message);
                                double v = getvalue(site);
                            }
                        }
                        else pixel.MapCode.Value = 0;

                        outputRaster.WriteBufferPixel();
                    }
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Cannot write " + path + " " + e.Message);
                
            }
            
        }
    }
}
