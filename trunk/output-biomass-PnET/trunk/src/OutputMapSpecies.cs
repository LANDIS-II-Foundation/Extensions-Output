using Landis.Core;
using Landis.SpatialModeling;
using System;

namespace Landis.Extension.Output.PnET
{
    public class OutputMapSpecies
    {
        ISpecies species;
        
        string FileName;
        public OutputMapSpecies(ISiteVar<Landis.Library.Parameters.Species.AuxParm<int>> values, ISpecies species, string MapNameTemplate)
        {
            this.species = species;
            FileName= FileNames.ReplaceTemplateVars(MapNameTemplate, species.Name, PlugIn.ModelCore.CurrentTime);
            WriteMap(values);
        }
        private void WriteMap(ISiteVar<Landis.Library.Parameters.Species.AuxParm<int>> values)
        {

            Console.WriteLine("   Writing {0} map to {1} ...", species.Name, FileName);

            using (IOutputRaster<IntPixel> outputRaster = PlugIn.ModelCore.CreateRaster<IntPixel>(FileName, PlugIn.ModelCore.Landscape.Dimensions))
            {
                IntPixel pixel = outputRaster.BufferPixel;
                foreach (Site site in PlugIn.ModelCore.Landscape.AllSites)
                {
                    if (site.IsActive)
                    {
                        pixel.MapCode.Value = (int)values[site][species];
                    }
                    else pixel.MapCode.Value = 0;

                    outputRaster.WriteBufferPixel();
                }
            }
        }
        
        
    }
}


