//  Copyright 2005-2010 Portland State University, University of Wisconsin-Madison
//  Authors:  Robert M. Scheller, James Domingo

using Edu.Wisc.Forest.Flel.Util;
using Landis.Core;
using System.Collections.Generic;

namespace Landis.Extension.Output.MaxSpeciesAge
{
    /// <summary>
    /// Methods for working with the template for map filenames.
    /// </summary>
    public static class MapNameTemplates
    {
        public const string SpeciesVar = "species";
        public const string TimestepVar = "timestep";

        private static IDictionary<string, bool> knownVars;
        private static IDictionary<string, string> varValues;

        //---------------------------------------------------------------------

        static MapNameTemplates()
        {
            knownVars = new Dictionary<string, bool>();
            knownVars[SpeciesVar] = true;
            knownVars[TimestepVar] = true;

            varValues = new Dictionary<string, string>();
        }

        //---------------------------------------------------------------------

        public static void CheckTemplateVars(string template)
        {
            OutputPath.CheckTemplateVars(template, knownVars);
        }

        //---------------------------------------------------------------------

        public static string ReplaceTemplateVars(string template,
                                                 string species,
                                                 int    timestep)
        {
            varValues[SpeciesVar] = species;
            varValues[TimestepVar] = timestep.ToString();
            return OutputPath.ReplaceTemplateVars(template, varValues);
        }
    }
}
