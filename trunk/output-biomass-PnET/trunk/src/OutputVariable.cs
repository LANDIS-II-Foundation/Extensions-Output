using Landis.Core;
using Edu.Wisc.Forest.Flel.Util;
using Landis.Library.BiomassCohortsPnET;
using Landis.SpatialModeling;
using Landis.Extension.Succession.BiomassPnET;

using System;
using System.Collections.Generic;
using System.IO;


namespace Landis.Extension.Output.BiomassPnET
{
    public class OutputVariable
    {
        string MapNameTemplate;
        string units;
        OutputFilePerTStepPerSpecies pertstepperspecies;
        
        public void UpdateVariable(ISiteVar<int> values)
        {
            // Variable per site (map)
            new OutputMapSiteVar(MapNameTemplate, values);
        }

        public void UpdateVariable(ISiteVar<Landis.Library.Biomass.Species.AuxParm<List<int>>> Values, int NrOfHistogramCohorts)
        {
            new OutputHistogramCohort(MapNameTemplate, NrOfHistogramCohorts).Write(Values);
        }
        public void UpdateVariable(ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> values, int NrOfHistogramCohorts)
        {
            new OutputHistogramCohort(MapNameTemplate, NrOfHistogramCohorts).Write(values);          
        }
        public void UpdateVariable(Landis.Library.Biomass.Species.AuxParm<int> Values_spc, int NrOfHistogramCohorts)
        {
            // Histogram (overwritten each time step)
            new OutputTableSpecies(Values_spc, MapNameTemplate);
        }

        public void UpdateVariable(ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> values)
        {
            // Variable per species and per site (multiple maps)
            foreach (ISpecies spc in PlugIn.SelectedSpecies)
            {
                new OutputMapSpecies(values, spc, MapNameTemplate);
            }
        }
        public void UpdateVariable(Landis.Library.Biomass.Species.AuxParm<int> Values_spc)
        {
            // Values per species each time step

            pertstepperspecies.Update(PlugIn.ModelCore.CurrentTime, Values_spc);

            
        }
        public OutputVariable(string MapNameTemplate, 
                              string units)
        {
            if (!MapNameTemplate.Contains(".img")) throw new System.Exception("MapNameTemplate " + MapNameTemplate+" does not have an extension '.img'");

            if (MapNameTemplate.Length == 0) throw new System.Exception("Error initializing output maps, no template name available");
            this.MapNameTemplate = MapNameTemplate;
            
            this.units = units;
            pertstepperspecies = new OutputFilePerTStepPerSpecies(MapNameTemplate);

            
           
        }
        
    }
}

 