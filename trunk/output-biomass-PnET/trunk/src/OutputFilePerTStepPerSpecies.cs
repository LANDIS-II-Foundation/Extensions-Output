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
        public void Update(int TStep, Landis.Library.Biomass.Species.AuxParm<int> Values_spc)
        {
            string line = TStep +"\t";
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
            string hdr = "Time\t";
            foreach (ISpecies spc in PlugIn.ModelCore.Species)
            {
                hdr += spc.Name + "\t";
            }
            Content.Add(hdr);
        }
    }
}
