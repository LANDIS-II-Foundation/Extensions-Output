using Landis.Core;
using Landis.SpatialModeling;
using System.Collections.Generic;


namespace Landis.Extension.Output.PnET
{
    public class OutputVariable
    {
        public string MapNameTemplate { get; private set; }
        string units;
        OutputFilePerTStepPerSpecies pertstepperspecies;
         
        OutputTableEcoregions averageperecoregion;

         
        public void UpdateVariable(ISiteVar<Landis.Library.Parameters.Species.AuxParm<int>> values)
        {
            // Variable per species and per site (multiple maps)
            foreach (ISpecies spc in PlugIn.SelectedSpecies)
            {
                new OutputMapSpecies(values, spc, MapNameTemplate);
            }
        }
        public void UpdateVariable(Landis.Library.Parameters.Species.AuxParm<float> Values_spc, double sum, float avg)
        {
            // Values per species each time step

            pertstepperspecies.Update(PlugIn.ModelCore.CurrentTime, Values_spc, sum, avg);

            
        }
        public void UpdateVariable(Landis.Library.Parameters.Species.AuxParm<int> Values_spc, int sum, float avg)
        {
            // Values per species each time step

            pertstepperspecies.Update(PlugIn.ModelCore.CurrentTime, Values_spc, sum, avg);

            
        }
        public void UpdateVariable(Landis.Library.Parameters.Ecoregions.AuxParm<float> AverageWater)
        {
            // Values per species each time step
            averageperecoregion.WriteUpdate(PlugIn.ModelCore.CurrentTime, AverageWater);
            
        }

        

        public OutputVariable(string MapNameTemplate, 
                              string units)
        {
            if (!MapNameTemplate.Contains(".img")) throw new System.Exception("MapNameTemplate " + MapNameTemplate+" does not have an extension '.img'");

            if (MapNameTemplate.Length == 0) throw new System.Exception("Error initializing output maps, no template name available");
            this.MapNameTemplate = MapNameTemplate;
            
            this.units = units;
            pertstepperspecies = new OutputFilePerTStepPerSpecies(MapNameTemplate, units);

           

            averageperecoregion = new OutputTableEcoregions(MapNameTemplate);
        }
        
    }
}

 