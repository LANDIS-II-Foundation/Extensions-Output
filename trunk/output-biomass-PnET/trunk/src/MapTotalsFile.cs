using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Landis.Core;
using Edu.Wisc.Forest.Flel.Util;
using Landis.Library.BiomassCohortsPnET;
using Landis.SpatialModeling;
using Landis.Extension.Succession.BiomassPnET;
using System.IO;


namespace Landis.Extension.Output.BiomassPnET
{
    public class MapTotalsFile
    {
        List<string> FileContent = new List<string>();
        string FileName;
        
        DelegateFunctions.GetAllSpeciesSpecific getallspeciesspecific;
        DelegateFunctions.GetOverallAverage getoverallaverage;

        private void Initialize(string units)
        {
            FileContent.Add("time\ttotal("+units+"\t");
            foreach (ISpecies species in PlugIn.ModelCore.Species) FileContent[0] += species.Name +"("  + units +")" + "\t";
        }
        public float GetTotal(Landis.Library.Biomass.Species.AuxParm<float> Values)
        { 
            float total =0;
            foreach (ISpecies species in PlugIn.ModelCore.Species)
            {
                total += Values[species];
            }
            return total;
        }
        public void Update()
        {
            Landis.Library.Biomass.Species.AuxParm<float> Values = new Landis.Library.Biomass.Species.AuxParm<float>(PlugIn.ModelCore.Species);

            Values = getallspeciesspecific();

            float total = GetTotal(Values);

            string line = PlugIn.ModelCore.CurrentTime + "\t" + total + "\t";
            foreach (ISpecies species in PlugIn.ModelCore.Species) line += Values[species] + "\t";
            FileContent.Add(line);
            Write();
        }
        private void Write()
        {
            System.IO.File.WriteAllLines(FileName, FileContent.ToArray());
        }

        public MapTotalsFile(DelegateFunctions.GetAllSpeciesSpecific getallspeciesspecific, DelegateFunctions.GetOverallAverage getoverallaverage, string MapNameTemplate, string units)
        {
            FileName = FileNames.ReplaceTemplateVars(MapNameTemplate, "").Replace(".img", ".txt").Replace(".gis", ".txt");
            this.getallspeciesspecific = getallspeciesspecific;
            this.getoverallaverage = getoverallaverage;
            Initialize(units);
           
        }
    }
}
