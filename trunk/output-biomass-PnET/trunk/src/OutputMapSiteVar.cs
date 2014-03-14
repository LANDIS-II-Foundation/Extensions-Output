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
    public class OutputMapSiteVar
    {
        string FileName;
        //string label;
        public OutputMapSiteVar(string MapNameTemplate, ISiteVar<int> values)
        {
            if (MapNameTemplate == null) throw new System.Exception("Cannot initialize maps with label " + MapNameTemplate );

            FileName = FileNames.MakeMapName(MapNameTemplate);
            //this.label = label;

            WriteMap(values);
        }
        
        private void WriteMap(ISiteVar<int> values)
        {
             
            try
            {
                using (IOutputRaster<IntPixel> outputRaster = PlugIn.ModelCore.CreateRaster<IntPixel>(FileName, PlugIn.ModelCore.Landscape.Dimensions))
                {
                    IntPixel pixel = outputRaster.BufferPixel;
                    foreach (Site site in PlugIn.ModelCore.Landscape.AllSites)
                    {
                        if (site.IsActive)
                        {
                            try
                            {
                                pixel.MapCode.Value = (int)values[site];
                            }
                            catch (System.Exception e)
                            {
                                System.Console.WriteLine("Cannot write " + FileName + " " + e.Message);
                                double v = values[site];
                            }
                        }
                        else pixel.MapCode.Value = 0;

                        outputRaster.WriteBufferPixel();
                    }
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Cannot write " + FileName + " " + e.Message);

            }

        }
        
        /*
        public void WriteValues(DelegateFunctions.GetSiteValue getsitevalue)
        {
            List<string> values = new List<string>();
            values.Add("Ecoregion\t" + label);

            string path = "NO_PATHNAME";
            try
            {
                path = MakeValueTableName(label);
                 
                foreach (Site site in PlugIn.ModelCore.Landscape.AllSites)
                {
                    if (site.IsActive) values.Add(PlugIn.ModelCore.Ecoregion[site] + "\t" + getsitevalue(site).ToString());
                    
                }
                System.IO.File.WriteAllLines(path, values.ToArray());
            }
            catch (System.Exception e)
            {
                throw new System.Exception("Cannot write " + path + " " + e.Message);
            }

        }
         */
        
    }
}
