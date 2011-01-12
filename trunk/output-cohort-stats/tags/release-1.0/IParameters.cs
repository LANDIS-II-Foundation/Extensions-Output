//  Copyright 2008 Conservation Biology Institute
//  Authors:  Brendan C. Ward
//  License:  N/A


using Landis.Species;
using System.Collections.Generic;

namespace Landis.Output.CohortStats
{
	/// <summary>
	/// The parameters for the plug-in.
	/// </summary>
	public interface IParameters
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
