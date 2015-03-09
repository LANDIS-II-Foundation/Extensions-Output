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
        public static ISiteVar<T> GetIsiteVar<T, M>(this ISiteVar<M> sitevar, Func<M, T> func)
        {
            ISiteVar<T> d = PlugIn.ModelCore.Landscape.NewSiteVar<T>();
            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                d[site] = (T)Convert.ChangeType(func(sitevar[site]), typeof(T));
            }
            return d;
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
