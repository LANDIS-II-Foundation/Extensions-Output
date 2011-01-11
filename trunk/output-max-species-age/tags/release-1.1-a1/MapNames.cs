//  Copyright 2005 University of Wisconsin-Madison
//  Authors:  Jimm Domingo, FLEL
//  License:  Available at  
//  http://landis.forest.wisc.edu/developers/LANDIS-IISourceCodeLicenseAgreement.pdf

using Edu.Wisc.Forest.Flel.Util;
using Landis.Species;
using System.Collections.Generic;

namespace Landis.Output.MaxSpeciesAge
{
	/// <summary>
	/// Methods for working with the template for map filenames.
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
	}
}
