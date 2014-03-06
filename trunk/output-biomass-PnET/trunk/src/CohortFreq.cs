using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Landis.Core;
using Landis.SpatialModeling;

namespace Landis.Extension.Output.BiomassPnET
{
    public class CohortFreq
    {
        string filenametemplate;
        int timestep;
        public CohortFreq(int timestep, string filenametemplate)
        {
            this.timestep = timestep;
            this.filenametemplate = filenametemplate;
        }

        private static int MaxAge(ISiteVar<Landis.Library.Biomass.Species.AuxParm<List<int>>> variable)
        {
            int maxage = int.MinValue;
            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                foreach (ISpecies species in PlugIn.ModelCore.Species)
                {
                    List<int> ages = variable[site][species];
                    foreach (int age in ages)
                    {
                        if (age > maxage) maxage = age;
                    }
                }
            }
            return maxage;
        }
        private static string hdr(int maxage, int timestep)
        {
            int running_cat_min = 0;
            int runnint_cat_max = timestep;
            string line="Species_Age\t";
            while (running_cat_min < maxage)
            {
                line += "[" + running_cat_min + "_" + runnint_cat_max +"]\t";

                running_cat_min += timestep;
                runnint_cat_max += timestep;
            }
            return line;
        }
        public void Write( ISiteVar<Landis.Library.Biomass.Species.AuxParm<List<int>>> variable)
        {
            List<string> FileContent = new List<string>();

            int maxage = MaxAge(variable);

            FileContent.Add(hdr(maxage, timestep));

            List<int> running_cat_min = new List<int>();
            List<int> running_cat_max = new List<int>();
            List<int> cat_count = new List<int>();

            int cat_min = 0;
            while (cat_min < maxage)
            {
                running_cat_min.Add(cat_min);
                running_cat_max.Add(cat_min + timestep);
                cat_count.Add(0);
                cat_min += timestep;
            }


            foreach (ISpecies species in PlugIn.ModelCore.Species)  
            {
                string line = species.Name + "\t";
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    
                    List<int> ages = variable[site][species];

                    if (ages.Count == 0) continue;

                    foreach (int age in ages)
                    {
                        for (int c = 0; c < running_cat_max.Count; c++)
                        {
                            if (age >= running_cat_min[c] && age < running_cat_max[c])
                            {
                                cat_count[c]++;
                            }
                        }
                    }
  
                }

                for (int c = 0; c < cat_count.Count();c++ )
                {
                    line += cat_count[c].ToString() + "\t";
                    cat_count[c] = 0;
                }
                
                FileContent.Add(line);
            }

            string fn = FileNames.ReplaceTemplateVars(filenametemplate, timestep);
            MakeFolders.Make(fn);
            System.IO.File.WriteAllLines(fn, FileContent.ToArray());
        }
    }
}
