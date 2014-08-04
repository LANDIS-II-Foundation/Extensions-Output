using Landis.Core;
using Edu.Wisc.Forest.Flel.Util;
using Landis.Library.BiomassCohortsPnET;
using Landis.SpatialModeling;
using Landis.Extension.Succession.BiomassPnET;

using System;
using System.Collections.Generic;
using System.IO;


namespace Landis.Extension.Output.PnET
{
    public class OutputVariable
    {
        string MapNameTemplate;
        string units;
        OutputFilePerTStepPerSpecies pertstepperspecies;
        OutputTableSpecies outputtable;
        OutputTableEcoregions averageperecoregion;

        public static ISiteVar<int> ToInt(ISiteVar<Landis.Library.Biomass.Pool> v)
        {
            ISiteVar<int> litter = PlugIn.ModelCore.Landscape.NewSiteVar<int>();
            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                litter[site] += (int)System.Math.Round(v[site].Mass, 0);
            }
            return litter;
        }

        
        public void UpdateVariable(ISiteVar<int> values)
        {
            // Variable per site (map)
            new OutputMapSiteVar(MapNameTemplate, values);
        }
        public void UpdateVariable(ISiteVar<Landis.Library.Biomass.Pool> values)
        {
            // Variable per site (map)
            new OutputMapSiteVar(MapNameTemplate, ToInt(values));
        }
        public void UpdateVariable(ISiteVar<Landis.Library.Biomass.Species.AuxParm<List<int>>> Values, string label, int NrOfHistogramCohorts)
        {
            new OutputHistogramCohort(MapNameTemplate, label, NrOfHistogramCohorts).WriteOutputHist(Values);
        }
        
        public void UpdateVariable(ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> values, string label, int NrOfHistogramCohorts)
        {
            new OutputHistogramCohort(MapNameTemplate, label,  NrOfHistogramCohorts).WriteOutputHist(values);          
        }
        public void UpdateVariable(Landis.Library.Biomass.Species.AuxParm<int> Values_spc)
        {
            outputtable.WriteUpdate(PlugIn.ModelCore.CurrentTime, Values_spc);
        }

        public void UpdateVariable(ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> values)
        {
            // Variable per species and per site (multiple maps)
            foreach (ISpecies spc in PlugIn.SelectedSpecies)
            {
                new OutputMapSpecies(values, spc, MapNameTemplate);
            }
        }
        public void UpdateVariable(Landis.Library.Biomass.Species.AuxParm<float> Values_spc, double sum, float avg)
        {
            // Values per species each time step

            pertstepperspecies.Update(PlugIn.ModelCore.CurrentTime, Values_spc, sum, avg);

            
        }
        public void UpdateVariable(Landis.Library.Biomass.Species.AuxParm<int> Values_spc, int sum, float avg)
        {
            // Values per species each time step

            pertstepperspecies.Update(PlugIn.ModelCore.CurrentTime, Values_spc, sum, avg);

            
        }
        public void UpdateVariable(Library.Biomass.Ecoregions.AuxParm<float> AverageWater)
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

            outputtable = new OutputTableSpecies(MapNameTemplate);

            averageperecoregion = new OutputTableEcoregions(MapNameTemplate);
        }
        
    }
}

 