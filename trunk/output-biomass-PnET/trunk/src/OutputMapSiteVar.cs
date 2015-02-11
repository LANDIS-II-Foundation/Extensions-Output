using Landis.SpatialModeling;

namespace Landis.Extension.Output.PnET
{
    public class OutputMapSiteVar<T>
    {
        string FileName ;

        public OutputMapSiteVar(string MapNameTemplate, string label, ISiteVar<T> values)
        {
            if (MapNameTemplate == null) throw new System.Exception("Cannot initialize maps with label " + MapNameTemplate );

            FileName = FileNames.ReplaceTemplateVars(MapNameTemplate, label, PlugIn.ModelCore.CurrentTime);
             
            WriteMap(values);
        }

        private void WriteMap(ISiteVar<T> values)
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
                                pixel.MapCode.Value = int.Parse(values[site].ToString());
                             
                            }
                            catch (System.Exception e)
                            {
                                System.Console.WriteLine("Cannot write " + FileName + " " + e.Message);
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
