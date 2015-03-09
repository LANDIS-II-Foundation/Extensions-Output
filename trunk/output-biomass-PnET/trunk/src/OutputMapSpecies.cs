using Landis.Core;
using Landis.SpatialModeling;
using System;

namespace Landis.Extension.Output.PnET
{
    public class OutputMapSpecies
    {
        

        public string FileName
        {
            get;
            private set;
        }
        public OutputMapSpecies(ISiteVar<int> values, ISpecies species, string MapNameTemplate)
        {
            
            FileName= FileNames.ReplaceTemplateVars(MapNameTemplate, species.Name, PlugIn.ModelCore.CurrentTime);

            Console.WriteLine("   Writing {0} map to {1} ...", species.Name, FileName);

            using (IOutputRaster<IntPixel> outputRaster = PlugIn.ModelCore.CreateRaster<IntPixel>(FileName, PlugIn.ModelCore.Landscape.Dimensions))
            {
                IntPixel pixel = outputRaster.BufferPixel;
                foreach (Site site in PlugIn.ModelCore.Landscape.AllSites)
                {
                    if (site.IsActive)
                    {
                        pixel.MapCode.Value = (int)values[site];
                    }
                    else pixel.MapCode.Value = 0;

                    outputRaster.WriteBufferPixel();
                }
            }
        }
       
        
    }
}


