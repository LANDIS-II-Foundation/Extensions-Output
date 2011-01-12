//  Copyright 2008-2010  Portland State University, Conservation Biology Institute
//  Authors:  Brendan C. Ward, Robert M. Scheller

using Landis.Core;
using System.Collections.Generic;

namespace Landis.Extension.Output.CohortStats
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
		/// Template for the filenames for output maps.
		/// </summary>
        string SppAgeStats_MapNames
		{
			get;
		}

        //---------------------------------------------------------------------

        string SiteAgeStats_MapNames
        {
            get;
        }

        //---------------------------------------------------------------------
 
        string SiteSppStats_MapNames
        {
            get;
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Collection of species for which output maps are generated, by statistic.
        /// </summary>
        Dictionary<string, IEnumerable<ISpecies>> AgeStatSpecies
        {
            get;
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// List of age statistics to calculate for each site (across all species).
        /// </summary>
        List<string> SiteAgeStats
        {
            get;
        }

        /// <summary>
        /// List of species statistics to calculate for each site (across all species).
        /// </summary>
        List<string> SiteSppStats
        {
            get;
        }


	}
}
