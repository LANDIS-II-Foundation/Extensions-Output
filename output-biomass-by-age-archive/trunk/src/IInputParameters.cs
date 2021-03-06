//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Robert M. Scheller, James B. Domingo

using Landis.Core;
using System.Collections.Generic;

namespace Landis.Extension.Output.BiomassByAge
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
			get;set;
		}

		//---------------------------------------------------------------------

		/// <summary>
		/// Collection of species for which biomass maps are generated.
		/// </summary>
		/// <remarks>
		/// null if no species are selected.
		/// </remarks>
		IEnumerable<ISpecies> SelectedSpecies
		{
			get;set;
		}

		//---------------------------------------------------------------------

		/// <summary>
		/// Template for the filenames for species biomass maps.
		/// </summary>
		/// <remarks>
		/// null if no species are selected.
		/// </remarks>
		string SpeciesMapNames
		{
			get;set;
		}

		//---------------------------------------------------------------------
        /// <summary>
        /// Container to hold the ageclasses for each species
        /// </summary>
        /// <remarks>
        /// null if no species are selected.
        /// </remarks>
        Dictionary<string, List<AgeClass>> AgeClasses
        {
            get;set;
        }
	}
}
