//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Robert M. Scheller, James B. Domingo

using Landis.Core;
using Edu.Wisc.Forest.Flel.Util;
using Landis.Library.LeafBiomassCohorts;
using Landis.SpatialModeling;

using System.Collections.Generic;

namespace Landis.Extension.Output.LeafBiomass
{
    /// <summary>
    /// Methods for working with the template for filenames of species biomass
    /// maps.
    /// </summary>
    public static class MapNames
    {
        public const string SpeciesVar = "species";
        public const string TimestepVar = "timestep";

        private static IDictionary<string, bool> knownVars;
        private static IDictionary<string, string> varValues;

        //---------------------------------------------------------------------

        static MapNames()
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
        //---------------------------------------------------------------------

        public static string ReplaceTemplateVars(string template,
                                                 string species)
        {
            varValues[SpeciesVar] = species;
            varValues[TimestepVar] = "{timestep}";
            return OutputPath.ReplaceTemplateVars(template, varValues);
        }
    }
}
