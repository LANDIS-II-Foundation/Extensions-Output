using Landis.Core;
using Landis.SpatialModeling;
using Landis.Library.BiomassCohortsPnET;
using System.Collections.Generic;
using Edu.Wisc.Forest.Flel.Util;
using System.IO;
using System;

namespace Landis.Extension.Output.BiomassPnET
{
    class BiomassPerEcoregion
    {
        string FileName;
        List<string> FileContent;
        FileProps p;
        public BiomassPerEcoregion(string FileName)
        {
            FileContent = new List<string>();
            this.FileName = FileName;

            if (FileName.Contains(".csv") || FileName.Contains(".CSV")) p = new FileProps(FileProps.FileDelimiters.comma);
            else if (FileName.Contains(".txt") || FileName.Contains(".TXT")) p = new FileProps(FileProps.FileDelimiters.comma);
            else throw new System.Exception("Output filename "+ FileName +" should have txt or csv extension");

            string hdr = "Time\t";
            foreach (IEcoregion ecoregion in PlugIn.ModelCore.Ecoregions)
            {
                hdr += ecoregion.Name + "\t";
            }
            FileContent.Add(hdr);
        }
        public void Write()
        {
            Landis.Extension.Succession.Biomass.Ecoregions.AuxParm<float> Biomass = GetBiomass();
            Landis.Extension.Succession.Biomass.Ecoregions.AuxParm<float> NrOfSites = GetNrOfSites();
            
            string line= PlugIn.ModelCore.CurrentTime.ToString() +"\t";
            foreach (IEcoregion ecoregion in PlugIn.ModelCore.Ecoregions)
            {
                if (NrOfSites[ecoregion] > 0) line += Biomass[ecoregion] / NrOfSites[ecoregion] + "\t";
                else line += "0" + "\t";
            }
            FileContent.Add(line);

            System.IO.File.WriteAllLines(FileName,FileContent.ToArray());
        }
        private static Landis.Extension.Succession.Biomass.Ecoregions.AuxParm<float> GetNrOfSites()
        { 
            Landis.Extension.Succession.Biomass.Ecoregions.AuxParm<float> NrOfSites = new Landis.Extension.Succession.Biomass.Ecoregions.AuxParm<float>(PlugIn.ModelCore.Ecoregions);
            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)NrOfSites[PlugIn.ModelCore.Ecoregion[site]]++;
            return NrOfSites;
        }
        private static Landis.Extension.Succession.Biomass.Ecoregions.AuxParm<float> GetBiomass()
        {
            Landis.Extension.Succession.Biomass.Ecoregions.AuxParm<float> Biomass =  new Landis.Extension.Succession.Biomass.Ecoregions.AuxParm<float>(PlugIn.ModelCore.Ecoregions);
            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
               
               foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site ])
               {
                   foreach (ICohort cohort in speciesCohorts)
                   {
                       Biomass[PlugIn.ModelCore.Ecoregion[site]] += cohort.Biomass;
                   }
                }
            }
            return Biomass;
        }
        
    }
}
