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
        MapTotalsFile SpeciesTotals = null;
        Landis.Extension.Succession.Biomass.Species.AuxParm<SpeciesMap> speciesmap=null;
        SiteVarMap sitevarmap = null;
        IEnumerable<ISpecies> SelectedSpecies;
        
        public void Update()
        {
            if (sitevarmap != null)
            {
                sitevarmap.WriteMap();
                sitevarmap.WriteValues();
            }
            if (SpeciesTotals != null) SpeciesTotals.Update();

            foreach (ISpecies species in SelectedSpecies)
            {
                if (speciesmap!=null) speciesmap[species].WriteMap(species);

            }
            
        }
        public OutputVariable(IEnumerable<ISpecies> SelectedSpecies, 
                                DelegateFunctions.GetAllSpeciesSpecific GetAllSpeciesSpecific,
                                DelegateFunctions.GetOverallAverage getoverallaverage, 
                                DelegateFunctions.GetValue GetSiteValue, 
                                DelegateFunctions.GetSpeciesSpecificValue GetSpeciesSpecificValue,
                                string MapNameTemplate, 
                                string VarLabel)
        {
            this.SelectedSpecies = SelectedSpecies;
            if (getoverallaverage != null && GetAllSpeciesSpecific != null) SpeciesTotals = new MapTotalsFile(GetAllSpeciesSpecific, getoverallaverage, MapNameTemplate);

            foreach (ISpecies species in SelectedSpecies)
            {
                if (GetSpeciesSpecificValue != null)
                {
                    if (speciesmap == null) speciesmap = new Succession.Biomass.Species.AuxParm<SpeciesMap>(PlugIn.ModelCore.Species);
                    speciesmap[species] = new SpeciesMap(GetSpeciesSpecificValue, species, MapNameTemplate);
                }
            }
              

            if (GetSiteValue != null) sitevarmap = new SiteVarMap(GetSiteValue, MapNameTemplate, "TOTAL" + VarLabel);
        }
        
    }
}

 