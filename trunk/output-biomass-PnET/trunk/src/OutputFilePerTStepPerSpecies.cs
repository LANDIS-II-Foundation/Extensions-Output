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
    public class OutputFilePerTStepPerSpecies
    {
        string FileName;
        List<string> Content;
        public void Update(int TStep, Landis.Library.Biomass.Species.AuxParm<float> Values_spc)
        {
            string line = TStep + "\t" + SiteVars.Biomass_sum + "\t" + SiteVars.Biomass_av + "\t";
            foreach (ISpecies spc in PlugIn.ModelCore.Species)
            {
                line += Values_spc[spc] + "\t";
            }
            Content.Add(line);
            System.IO.File.WriteAllLines(FileName, Content.ToArray());
        }
        public void Update(int TStep, Landis.Library.Biomass.Species.AuxParm<int> Values_spc, int sum, float avg)
        {
            string line = TStep + "\t" + sum + "\t" + avg + "\t";
            foreach (ISpecies spc in PlugIn.ModelCore.Species)
            {
                line += Values_spc[spc] + "\t";
            }
            Content.Add(line);
            System.IO.File.WriteAllLines(FileName, Content.ToArray());
        }
        public OutputFilePerTStepPerSpecies(string MapNameTemplate)
        {
            FileName = FileNames.OutputTableSpeciesName(MapNameTemplate);
            FileNames.MakeFolders(FileName);
            Content = new List<string>();
            string hdr = "Time" + "\t" + "Sum" + "\t" + "Avg" + "\t";
            foreach (ISpecies spc in PlugIn.ModelCore.Species)
            {
                hdr += spc.Name + "\t";
            }
            Content.Add(hdr);
        }
    }
}
