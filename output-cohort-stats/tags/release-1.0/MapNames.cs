//  Copyright 2008 Conservation Biology Institute
//  Authors:  Brendan C. Ward
//  License:  N/A


using Edu.Wisc.Forest.Flel.Util;
using Landis.Species;
using System.Collections.Generic;

namespace Landis.Output.CohortStats
{
	/// <summary>
	/// Methods for working with the template for map filenames.
	/// </summary>
	public static class SpeciesMapNames
	{
		public const string SpeciesVar = "species";
        public const string StatisticVar = "statistic";
		public const string TimestepVar = "timestep";

		private static IDictionary<string, bool> knownVars;
		private static IDictionary<string, string> varValues;

		//---------------------------------------------------------------------

        static SpeciesMapNames()
		{
			knownVars = new Dictionary<string, bool>();
			knownVars[SpeciesVar] = true;
            knownVars[StatisticVar] = true;
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
                                                 string statistic,
		                                         int    timestep)
		{
			varValues[SpeciesVar] = species;
            varValues[StatisticVar] = statistic;
			varValues[TimestepVar] = timestep.ToString();
			return OutputPath.ReplaceTemplateVars(template, varValues);
		}
	}

    //---------------------------------------------------------------------

    public static class SiteMapNames
    {
        public const string StatisticVar = "statistic";
        public const string TimestepVar = "timestep";

        private static IDictionary<string, bool> knownVars;
        private static IDictionary<string, string> varValues;

        //---------------------------------------------------------------------

        static SiteMapNames()
        {
            knownVars = new Dictionary<string, bool>();
            knownVars[StatisticVar] = true;
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
                                                 string statistic,
                                                 int timestep)
        {
            varValues[StatisticVar] = statistic;
            varValues[TimestepVar] = timestep.ToString();
            return OutputPath.ReplaceTemplateVars(template, varValues);
        }
    }


}
