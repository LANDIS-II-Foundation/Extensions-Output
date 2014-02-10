using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Landis.Core;
using Landis.SpatialModeling;

namespace Landis.Extension.Output.BiomassPnET
{
    public class SpeciesFrequency
    {
        List<string> FileContent = new List<string>();
        string outputfile;

        public SpeciesFrequency(string filename)
        {
            outputfile= filename ;
            string hdr = "time\t";
            foreach (ISpecies species in PlugIn.ModelCore.Species) hdr += species.Name + "\t";

            FileContent.Add(hdr);
        }

        private static Landis.Extension.Succession.Biomass.Species.AuxParm<int> SumSiteSpcVar(ISiteVar<Landis.Extension.Succession.Biomass.Species.AuxParm<int>> SiteSpcValue)
        {
             
            Landis.Extension.Succession.Biomass.Species.AuxParm<int> SpcValue = new Succession.Biomass.Species.AuxParm<int>(PlugIn.ModelCore.Species);

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

        public void WriteUpdate(int year, ISiteVar<Landis.Extension.Succession.Biomass.Species.AuxParm<int>> Var)
        {
            Landis.Extension.Succession.Biomass.Species.AuxParm<int> PerSpc = SumSiteSpcVar(Var);
            string line = year +"\t";
            foreach (ISpecies species in PlugIn.ModelCore.Species) line += PerSpc[species] + "\t";
            line += "\n";

            FileContent.Add(line);


            MakeFolders.Make(outputfile);
            System.IO.File.WriteAllLines(outputfile, FileContent.ToArray());

        }
    }
}
