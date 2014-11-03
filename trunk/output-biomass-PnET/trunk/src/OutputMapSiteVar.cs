using Landis.SpatialModeling;

namespace Landis.Extension.Output.PnET
{
    public class OutputMapSiteVar
    {
        string FileName ;
        
        public OutputMapSiteVar(string MapNameTemplate, string label,  ISiteVar<int> values)
        {
            if (MapNameTemplate == null) throw new System.Exception("Cannot initialize maps with label " + MapNameTemplate );

            FileName = FileNames.ReplaceTemplateVars(MapNameTemplate, label, PlugIn.ModelCore.CurrentTime);
             
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
        
        
        
    }
}
