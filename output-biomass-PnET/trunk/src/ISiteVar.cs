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
       
         
        
       

    }
}
