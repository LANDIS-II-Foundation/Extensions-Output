﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Landis.Core;
using Landis.SpatialModeling;

namespace Landis.Extension.Output.BiomassPnET
{

    public class OutputTableSpecies
    {
        List<string> FileContent = new List<string>();
        string FileName;

        public OutputTableSpecies(Landis.Library.Biomass.Species.AuxParm<int> SpeciesSpecificValues, string MapNameTemplate)
        {
            FileName = FileNames.ReplaceTemplateVars(MapNameTemplate);
            FileNames.MakeFolders(FileName);

            string hdr = "time\t";
            foreach (ISpecies species in PlugIn.ModelCore.Species) hdr += species.Name + "\t";

            FileContent.Add(hdr);
        }
        public void WriteUpdate(int year, Landis.Library.Biomass.Species.AuxParm<int> values)
        {
            string line = year + "\t";
            foreach (ISpecies species in PlugIn.SelectedSpecies)
            {
                line += values[species] + "\t";
            }

            FileContent.Add(line);

             
            System.IO.File.WriteAllLines(FileName, FileContent.ToArray());

        }
        /*
        private static Landis.Library.Biomass.Species.AuxParm<int> SumSiteSpcVar(ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> SiteSpcValue)
        {
             
            Landis.Library.Biomass.Species.AuxParm<int> SpcValue = new Library.Biomass.Species.AuxParm<int>(PlugIn.ModelCore.Species);

            foreach (ISpecies species in PlugIn.ModelCore.Species)
            {
                SpcValue[species] = 0;
            }
            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                foreach (ISpecies species in PlugIn.ModelCore.Species) 
                {
                    SpcValue[species] += SiteSpcValue[site][species];
                }
            }
            return SpcValue;
        }
        */
        
    }
}