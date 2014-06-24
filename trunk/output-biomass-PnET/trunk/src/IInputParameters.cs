//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Robert M. Scheller, James B. Domingo

using Landis.Core;
using System.Collections.Generic;

namespace Landis.Extension.Output.PnET
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
			get;
		}

		//---------------------------------------------------------------------
        string AgeDistribution
        {
            get;
        }
        string CohortBalance
        {
            get;
        }

        string DeadCohortNumbers
        {
            get;
      
        }
        string DeadCohortAges
        {
            get;
      
        }
        string SpeciesEst
        {
            get; 
        }
        string Water
        {
            get; 
        }
        string AnnualTranspiration
        {
            get; 
        }

        string SubCanopyPAR
        {
            get; 
        }
        string LeafAreaIndex
        {
            get;
        }
		string SpeciesBiom
		{
			get; 
		}
        string CohortsPerSpecies
        {
            get;
        }
        string BelowgroundBiomass
		{
			get; 
		}
        string WoodyDebris
        {
            get; 
        }
        string Litter
        {
            get;
             
        }
		
    }
}
