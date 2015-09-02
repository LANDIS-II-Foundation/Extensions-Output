//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Robert M. Scheller, James B. Domingo

using Landis.Core;
using Edu.Wisc.Forest.Flel.Util;
using System.Collections.Generic;

namespace Landis.Extension.Output.Biomass
{
    /// <summary>
    /// Methods for working with the template for filenames of dead biomass
    /// maps.
    /// </summary>
    public static class PoolMapNames
    {
        public const string PoolVar = "pool";
        public const string TimestepVar = "timestep";

        private static IDictionary<string, bool> knownVars;
        private static IDictionary<string, string> varValues;

        //---------------------------------------------------------------------

        static PoolMapNames()
        {
            knownVars = new Dictionary<string, bool>();
            knownVars[PoolVar] = true;
            knownVars[TimestepVar] = true;

            varValues = new Dictionary<string, string>();
        }

        //---------------------------------------------------------------------

        public static void CheckTemplateVars(string            template)
                                             //string selectedPools)
        {
            //if (selectedPools == ("woody" | "non-woody"))
            //    knownVars[PoolVar] = true;
            //else
            //    knownVars[PoolVar] = false;
            OutputPath.CheckTemplateVars(template, knownVars);
        }

        //---------------------------------------------------------------------

        public static string ReplaceTemplateVars(string template,
                                                 string pool,
                                                 int    timestep)
        {
            varValues[PoolVar] = pool;
            varValues[TimestepVar] = timestep.ToString();
            return OutputPath.ReplaceTemplateVars(template, varValues);
        }
    }
}
