//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:   Robert M. Scheller, James B. Domingo

using Edu.Wisc.Forest.Flel.Util;
using System.Collections.Generic;

namespace Landis.Extension.Output.AgeReclass
{
	/// <summary>
	/// The parameters for the plug-in.
	/// </summary>
	public interface IInputParameters
	{
		/// <summary>
		/// Timestep (years)
		/// </summary>
		int Timestep
		{
			get;
		}

		//---------------------------------------------------------------------

		/// <summary>
		/// Reclass coefficients for species
		/// </summary>
		double[] ReclassCoefficients
		{
			get;
		}

		//---------------------------------------------------------------------

		/// <summary>
		/// Reclass maps
		/// </summary>
		List<IMapDefinition> ReclassMaps
		{
			get;
		}

		//---------------------------------------------------------------------

		/// <summary>
		/// Template for the filenames for reclass maps.
		/// </summary>
		string MapFileNames
		{
			get;
		}
	}
}
