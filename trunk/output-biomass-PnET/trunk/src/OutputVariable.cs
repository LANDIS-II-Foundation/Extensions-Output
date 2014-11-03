using Landis.Core;
using Landis.SpatialModeling;
using System.Collections.Generic;


namespace Landis.Extension.Output.PnET
{
    public class OutputVariable
    {
        public string MapNameTemplate { get; private set; }
        public string units { get; private set; }
        
        
        public OutputVariable(string MapNameTemplate, 
                              string units)
        {
            this.MapNameTemplate = MapNameTemplate;
            this.units = units;

            if (!MapNameTemplate.Contains(".img")) throw new System.Exception("MapNameTemplate " + MapNameTemplate+" does not have an extension '.img'");
            if (MapNameTemplate.Length == 0) throw new System.Exception("Error initializing output maps, no template name available");
            
         

             
        }
        
    }
}

 