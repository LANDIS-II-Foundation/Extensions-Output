using Edu.Wisc.Forest.Flel.Util;
using Landis.Species;
using System.Collections.Generic;

namespace Landis.Output.BiomassAgeClass
{
    /// <summary>
    /// Methods for working with the template for filenames of species biomass
    /// maps.
    /// </summary>
    public static class MapNames
    {
        public const string SpeciesVar = "species";
        public const string AgeClassVar = "ageclass";
        public const string TimestepVar = "timestep";

        private static IDictionary<string, bool> knownVars;
        private static IDictionary<string, string> varValues;

        //---------------------------------------------------------------------

        static MapNames()
        {
            knownVars = new Dictionary<string, bool>();
            knownVars[SpeciesVar] = true;
            knownVars[AgeClassVar] = true;
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
                                                 string ageclass,
                                                 int    timestep)
        {
            varValues[SpeciesVar] = species;
            varValues[AgeClassVar] = ageclass;
            varValues[TimestepVar] = timestep.ToString();
            return OutputPath.ReplaceTemplateVars(template, varValues);
        }
    }
}
