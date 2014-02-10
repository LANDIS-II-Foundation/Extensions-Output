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
    public class SpeciesMap
    {

        DelegateFunctions.GetSpeciesSpecificValue getvalue;
        ISpecies species;
        string MapNameTemplate;

        public SpeciesMap(DelegateFunctions.GetSpeciesSpecificValue getvalue, ISpecies species, string MapNameTemplate)
        {
            this.getvalue = getvalue;
            this.MapNameTemplate = MapNameTemplate;
            this.species = species;
        }

        public string MakeSpeciesMapName(string species)
        {
            return FileNames.ReplaceTemplateVars(MapNameTemplate, species, PlugIn.ModelCore.CurrentTime);
        }
        public float SpeciesBiomass(Site site, ISpecies species, DelegateFunctions.GetSpeciesSpecificValue getvalue)
        {
            ISpeciesCohorts cohorts = SiteVars.Cohorts[site][species];
            float total = 0;
            if (cohorts != null)
                foreach (ICohort cohort in cohorts)
                {
                    total += getvalue(species,site);
                }
            return total;
        }
        public void WriteMap(ISpecies species)
        {
            string path = MakeSpeciesMapName(species.Name);

            Console.WriteLine("   Writing {0} biomass map to {1} ...", species.Name, path);

            using (IOutputRaster<IntPixel> outputRaster = PlugIn.ModelCore.CreateRaster<IntPixel>(path, PlugIn.ModelCore.Landscape.Dimensions))
            {
                IntPixel pixel = outputRaster.BufferPixel;
                foreach (Site site in PlugIn.ModelCore.Landscape.AllSites)
                {
                    if (site.IsActive)
                    {
                        pixel.MapCode.Value = (int)getvalue(species, site);
                    }
                    else pixel.MapCode.Value = 0;
                         
                    outputRaster.WriteBufferPixel();
                }
            }
        }
    }
}


