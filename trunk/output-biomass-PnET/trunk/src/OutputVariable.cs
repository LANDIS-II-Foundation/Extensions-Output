using Landis.Core;
using Landis.SpatialModeling;
using System.Collections.Generic;


namespace Landis.Extension.Output.PnET
{
    public class OutputVariable
    {
        public string MapNameTemplate { get; private set; }
        string units;
        
         
        public void UpdateVariable(Landis.Library.Parameters.Species.AuxParm<float> Values_spc, double sum, float avg)
        {
            // Values per species each time step

            new OutputFilePerTStepPerSpecies(MapNameTemplate, units).Update(PlugIn.ModelCore.CurrentTime, Values_spc, sum, avg);

            
        }
        public void UpdateVariable(Landis.Library.Parameters.Species.AuxParm<int> Values_spc, int sum, float avg)
        {
            // Values per species each time step

            new OutputFilePerTStepPerSpecies(MapNameTemplate, units).Update(PlugIn.ModelCore.CurrentTime, Values_spc, sum, avg);

            
        }
       
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

 