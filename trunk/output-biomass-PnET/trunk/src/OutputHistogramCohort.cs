using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Landis.Core;
using Landis.SpatialModeling;

namespace Landis.Extension.Output.BiomassPnET
{
    public class OutputHistogramCohort
    {
        string FileName;
        int NrOfCohorts;
        List<string> FileContent;
        List<float> running_cat_min = new List<float>();
        List<float> running_cat_max = new List<float>();
        List<int> cat_count = new List<int>();

        public OutputHistogramCohort(string filenametemplate, int NrOfCohorts)
        {
            FileContent = new List<string>();
            this.NrOfCohorts = NrOfCohorts;
            FileName = FileNames.OutputHistogramCohortName(filenametemplate);
        }

        private static float[] Extremes(ISiteVar<Landis.Library.Biomass.Species.AuxParm<List<int>>> values)
        {
            float[] extremes = new float[2];
            extremes[0] = float.MaxValue;
            extremes[1] = float.MinValue;

            foreach(ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                foreach(ISpecies spc in PlugIn.ModelCore.Species)
                {
                    foreach (int var in values[site][spc])
                    {
                        if (var > extremes[1]) extremes[1] = var;
                        if (var < extremes[0]) extremes[0] = var;
                    }
                }
            }
            return extremes;   
        }
        private static float[] Extremes(ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> values)
        {
            float[] extremes = new float[2];
            extremes[0] = float.MaxValue;
            extremes[1] = float.MinValue;

            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                foreach (ISpecies species in PlugIn.ModelCore.Species)
                {
                    int var = values[site][species];

                    if (var > extremes[1]) extremes[1] = var;
                    if (var < extremes[0]) extremes[0] = var;
                }
            }
            return extremes;
        }
        private float CohortWidth(float[] extremes)
        {
            float cohort_width = 1F / (float)NrOfCohorts * (extremes[1] - extremes[0]);
            return cohort_width;
        }
        private string hdr()
        {
            string line="Species\t";
            for (int f = 0; f < running_cat_min.Count;f++ )
            {
                line += "[" + running_cat_min[f] + "_" + running_cat_max[f] + "]\t";
            }
            
            return line;
        }
        public void SetCategorieBounds(float[] extremes)
        {
            float cat_min = extremes[0];
            float cohort_width = CohortWidth(extremes);
            while (cat_min < extremes[1])
            {
                running_cat_min.Add(cat_min);
                running_cat_max.Add(cat_min + cohort_width);
                cat_count.Add(0);
                cat_min += cohort_width;
            }
        }
        public void Write(ISiteVar<Landis.Library.Biomass.Species.AuxParm<List<int>>> values)
        {
            float[] extremes = Extremes(values);

            SetCategorieBounds(extremes);

            FileContent.Add(hdr());

            foreach (ISpecies species in PlugIn.ModelCore.Species)
            {
                string line = species.Name + "\t";
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    List<int> lvar = values[site][species];

                    for (int c = 0; c < running_cat_max.Count; c++)
                    {
                        foreach (int var in lvar)
                        {
                            if (var >= running_cat_min[c] && var < running_cat_max[c])
                            {
                                cat_count[c]++;
                            }
                        }
                    }
                }

                for (int c = 0; c < cat_count.Count(); c++)
                {
                    line += cat_count[c].ToString() + "\t";
                    cat_count[c] = 0;
                }

                FileContent.Add(line);
            }
            System.IO.File.WriteAllLines(FileName, FileContent.ToArray());
        
        }
        public void Write(ISiteVar<Landis.Library.Biomass.Species.AuxParm<int>> values)
        {
            float[] extremes = Extremes(values);

            SetCategorieBounds(extremes);

            FileContent.Add(hdr());

            

            foreach (ISpecies species in PlugIn.ModelCore.Species)  
            {
                string line = species.Name + "\t";
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    int var = values[site][species];

                    for (int c = 0; c < running_cat_max.Count; c++)
                    {
                        if (var >= running_cat_min[c] && var < running_cat_max[c])
                        {
                            cat_count[c]++;
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


            System.IO.File.WriteAllLines(FileName, FileContent.ToArray());
        }
    }
}
