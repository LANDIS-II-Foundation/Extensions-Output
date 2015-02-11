using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Landis.Core;
using Landis.SpatialModeling;

namespace Landis.Extension.Output.PnET
{
    public static class ISiteVar
    {
        public static ISiteVar<T> Copy<T>(this  ISiteVar<T> values)
        {
            ISiteVar<T> copy = PlugIn.ModelCore.Landscape.NewSiteVar<T>();

            foreach(ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                foreach (ISpecies species in PlugIn.ModelCore.Species)
                {
                    copy[site] = values[site];
                }
            }
            return copy;
        }
        public static ISiteVar<bool> ToBool(this ISiteVar<int> values)
        {
            ISiteVar<bool> var = PlugIn.ModelCore.Landscape.NewSiteVar<bool>();

            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                 var[site] =  ((int)values[site] > 0 )  ? true : false;
            }
            return var;
        }
        public static ISiteVar<int> Compare(this ISiteVar<bool> before,   ISiteVar<bool> after, MapComparison m)
        {
            ISiteVar<int> var = PlugIn.ModelCore.Landscape.NewSiteVar<int>();

            

            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                var[site] = m[before[site], after[site]];
            }
            return var;
        }
        public static ISiteVar<int> ToInt(this ISiteVar<float> values)
        {
            ISiteVar<int> var = PlugIn.ModelCore.Landscape.NewSiteVar<int>();

            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                var[site] = (int)values[site];
            }
            return var;
        }
        public static ISiteVar<int> ToInt(this ISiteVar<Landis.Library.Biomass.Pool> pool)
        {
            ISiteVar<int> var = PlugIn.ModelCore.Landscape.NewSiteVar<int>();

            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                var[site] = (int)pool[site].Mass;
            }
            return var;
        }

        public static double Average(this ISiteVar<Landis.Library.Biomass.Pool> values)
        {
            double sum = 0.0;
            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                double d = values[site].Mass;
                sum += d;
            }
            return sum / (float)PlugIn.ModelCore.Landscape.Count();
           
        }
        
        public static double Average<T>(this ISiteVar<T> values)
            where T : System.IComparable<T>
        {
            double sum = 0.0;
            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                double d = double.Parse(values[site].ToString());
                sum += d;
            }
            return sum / (float)PlugIn.ModelCore.Landscape.Count();
             
        }
        
       

    }
}
