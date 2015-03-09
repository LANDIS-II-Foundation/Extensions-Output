using System.Collections.Generic;
using System.Linq;
using Landis.Core;
using Landis.SpatialModeling;

namespace Landis.Extension.Output.PnET
{
    public class OutputHistogramCohort<T> where T : System.IComparable
    {
        string FileName;
        int NrOfCohorts;
        List<string> FileContent;
        
        List<int> cat_count = new List<int>();
        List<int> cat_count_tot = new List<int>();
        string label;
        public OutputHistogramCohort(string filenametemplate, string label, int NrOfCohorts)
        {
            FileContent = new List<string>();
            this.NrOfCohorts = NrOfCohorts;
            FileName = FileNames.ReplaceTemplateVars(filenametemplate, "", PlugIn.ModelCore.CurrentTime).Replace(".img", "Histogram.txt");
             
            this.label = label;
        }

        private static T[] Extremes(ISiteVar<Landis.Library.Parameters.Species.AuxParm<T>> values)
        {
            T[] extremes = new T[2];

            if (typeof(T) == typeof(double)) { extremes[0] = (T)System.Convert.ChangeType(double.MaxValue, typeof(T)); extremes[1] = (T)System.Convert.ChangeType(double.MinValue, typeof(T)); }
            else if (typeof(T) == typeof(float)) { extremes[0] = (T)System.Convert.ChangeType(float.MaxValue, typeof(T)); extremes[1] = (T)System.Convert.ChangeType(float.MinValue, typeof(T)); }
            else if (typeof(T) == typeof(int)) { extremes[0] = (T)System.Convert.ChangeType(int.MaxValue, typeof(T)); extremes[1] = (T)System.Convert.ChangeType(int.MinValue, typeof(T)); }
            else if (typeof(T) == typeof(ushort)) { extremes[0] = (T)System.Convert.ChangeType(ushort.MaxValue, typeof(T)); extremes[1] = (T)System.Convert.ChangeType(ushort.MinValue, typeof(T)); }
            else if (typeof(T) == typeof(byte)) { extremes[0] = (T)System.Convert.ChangeType(byte.MaxValue, typeof(T)); extremes[1] = (T)System.Convert.ChangeType(byte.MinValue, typeof(T)); }
            else
            {
                throw new System.Exception("Cannot calculate Extremes for type " + typeof(T).ToString());
            }

            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                foreach (ISpecies spc in PlugIn.ModelCore.Species)
                {
                    if (values[site][spc] == null) continue;


                    if (values[site][spc].CompareTo(extremes[1]) > 0) extremes[1] = values[site][spc];
                    if (values[site][spc].CompareTo(extremes[0]) < 0) extremes[0] = values[site][spc];
                    
                }
            }
            return extremes;   
        
        }
        private static T[] Extremes(ISiteVar<Landis.Library.Parameters.Species.AuxParm<List<T>>> values)
        
        {
            T[] extremes = new T[2];

            if (typeof(T) == typeof(double)) { extremes[0] = (T)System.Convert.ChangeType(double.MaxValue, typeof(T)); extremes[1] = (T)System.Convert.ChangeType(double.MinValue, typeof(T)); }
            else if (typeof(T) == typeof(float)) { extremes[0] = (T)System.Convert.ChangeType(float.MaxValue, typeof(T)); extremes[1] = (T)System.Convert.ChangeType(float.MinValue, typeof(T)); }
            else if (typeof(T) == typeof(int)) { extremes[0] = (T)System.Convert.ChangeType(int.MaxValue, typeof(T)); extremes[1] = (T)System.Convert.ChangeType(int.MinValue, typeof(T)); }
            else if (typeof(T) == typeof(ushort)) { extremes[0] = (T)System.Convert.ChangeType(ushort.MaxValue, typeof(T)); extremes[1] = (T)System.Convert.ChangeType(ushort.MinValue, typeof(T)); }
            else if (typeof(T) == typeof(byte)) { extremes[0] = (T)System.Convert.ChangeType(byte.MaxValue, typeof(T)); extremes[1] = (T)System.Convert.ChangeType(byte.MinValue, typeof(T)); }
            else
            {
                throw new System.Exception("Cannot calculate Extremes for type " + typeof(T).ToString());
            }

            foreach(ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                foreach(ISpecies spc in PlugIn.ModelCore.Species)
                {
                    if (values[site][spc] == null) continue;
                    foreach (T var in values[site][spc])
                    {

                        if (var.CompareTo(extremes[1]) > 0) extremes[1] = var;
                        if (var.CompareTo(extremes[0]) < 0) extremes[0] = var;
                    }
                }
            }
            return extremes;   
        }
        
        private double CohortWidth(double min, double max)
        {
            double cohort_width = 1.0 / (float)NrOfCohorts * (double.Parse(max.ToString()) - double.Parse(min.ToString()));
            return cohort_width;
        }
        private string hdr(string HdrExplanation, List<double> running_cat_min, List<double> running_cat_max)
        {
            string line= HdrExplanation + "\t";
            for (int f = 0; f < running_cat_min.Count;f++ )
            {
                line += "[" + running_cat_min[f] + "_" + running_cat_max[f] + "]\t";
            }
            
            return line;
        }
        public List<double>[] GetCategorieBounds(T[] extremes)
        {
            List<double>[] running_cat = new List<double>[2] { new List<double>(),  new List<double>() };
            
            double min =  (extremes[0].CompareTo(extremes[1]) == 0) ? 0.9 * double.Parse(extremes[0].ToString()) : double.Parse(extremes[0].ToString());
            double max =  (extremes[0].CompareTo(extremes[1]) == 0) ? 1.1 * double.Parse(extremes[1].ToString()) : double.Parse(extremes[1].ToString());
             
            double cat_min = min;
            double cohort_width = CohortWidth(min, max);
            while (cat_min.CompareTo(max)<0)
            {
                running_cat[0].Add(cat_min);

                running_cat[1].Add(cat_min + cohort_width);

                cat_count.Add(0);
                cat_count_tot.Add(0);

                cat_min = running_cat[1][running_cat[1].Count() - 1];
            }
            return running_cat;
        }
        
        public void WriteOutputHist(ISiteVar<Landis.Library.Parameters.Species.AuxParm<List<T>>> values)
        {
            T[] extremes = Extremes(values);

            List<double>[] categorybounds = GetCategorieBounds(extremes);

            FileContent.Add(hdr(label, categorybounds[0], categorybounds[1]));

            if (cat_count_tot.Count() == 0)return;
             
            foreach (ISpecies species in PlugIn.ModelCore.Species)
            {
                string line = species.Name + "\t";
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    if (values[site][species] == null) continue;

                    for (int c = 0; c < categorybounds[1].Count; c++)
                    {
                        foreach (T var in values[site][species])
                        {
                            T min  = (T)System.Convert.ChangeType(categorybounds[0][c], typeof(T));
                            T max = (T)System.Convert.ChangeType(categorybounds[1][c], typeof(T));
                            
                            if (var.CompareTo(min) >= 0 && var.CompareTo(max) < 0 || var.CompareTo(extremes[1]) == 0)
                            {
                                cat_count[c]++;
                                cat_count_tot[c]++;
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
            string linetot = "Total\t";
            for (int c = 0; c < cat_count.Count(); c++)
            {
                linetot += cat_count_tot[c].ToString() + "\t";
                cat_count[c] = 0;
            }
            FileContent.Add(linetot);


            System.IO.File.WriteAllLines(FileName, FileContent.ToArray());
        
        }
        public void WriteOutputHist(ISiteVar<Landis.Library.Parameters.Species.AuxParm<T>> values)
        {
            

            T[] extremes = Extremes(values);

            List<double>[] categorybounds =  GetCategorieBounds(extremes);

            if (cat_count_tot.Count() == 0) return;


            FileContent.Add(hdr(label, categorybounds[0], categorybounds[1]));

            foreach (ISpecies species in PlugIn.ModelCore.Species)  
            {
                string line = species.Name + "\t";
                foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
                {
                    T var = values[site][species];

                    for (int c = 0; c < categorybounds[1].Count; c++)
                    {
                        T min  = (T)System.Convert.ChangeType(categorybounds[0][c], typeof(T));
                        T max = (T)System.Convert.ChangeType(categorybounds[1][c], typeof(T));

                        if (var.CompareTo(min) >= 0 && var.CompareTo(max) < 0)
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
