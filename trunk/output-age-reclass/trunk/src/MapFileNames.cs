//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Robert M. Scheller, James B. Domingo

using Edu.Wisc.Forest.Flel.Util;
using System.Collections.Generic;

namespace Landis.Extension.Output.AgeReclass
{
	/// <summary>
	/// Methods for working with the template for filenames of reclass maps.
	/// </summary>
	public static class MapFiles
	{
		public const string MapNameVar = "reclass-map-name";
		public const string TimestepVar = "timestep";

		private static IDictionary<string, bool> knownVars;
		private static IDictionary<string, string> varValues;

		//---------------------------------------------------------------------

		static MapFiles()
		{
			knownVars = new Dictionary<string, bool>();
			knownVars[MapNameVar] = true;
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
		                                         string reclassMapName,
		                                         int    timestep)
		{
			varValues[MapNameVar] = reclassMapName;
			varValues[TimestepVar] = timestep.ToString();
			return OutputPath.ReplaceTemplateVars(template, varValues);
		}
	}
}
