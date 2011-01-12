//  Copyright 2008-2010  Portland State University, Conservation Biology Institute
//  Authors:  Brendan C. Ward, Robert M. Scheller

using Landis.Core;
using System.Collections.Generic;
using Edu.Wisc.Forest.Flel.Util;

namespace Landis.Extension.Output.CohortStats
{
	/// <summary>
	/// The parameters for the plug-in.
	/// </summary>
	public class InputParameters
		: IInputParameters
	{
		private int timestep;
		private string sppagestats_mapNames = "";
        private string siteagestats_mapNames = "";
        private string sitesppstats_mapNames = "";
        private Dictionary<string, IEnumerable<ISpecies>> ageStatSpecies = new Dictionary<string, IEnumerable<ISpecies>>();
        private List<string> siteAgeStats = new List<string>();
        private List<string> siteSppStats = new List<string>();
		//---------------------------------------------------------------------

		public int Timestep
		{
			get {
				return timestep;
			}
            set
            {
                if (value < 0)
                    throw new InputValueException("","Value must be = or > 0.");
                timestep = value;
            }   
        }

		//---------------------------------------------------------------------

        public string SppAgeStats_MapNames
		{
			get {
                return sppagestats_mapNames;
			}
            set
            {
                if (value != null)
                {
                    SpeciesMapNames.CheckTemplateVars(value);
                }
                sppagestats_mapNames = value;
            }
        }

        //---------------------------------------------------------------------

        public string SiteAgeStats_MapNames
        {
            get
            {
                return siteagestats_mapNames;
            }
            set
            {
                if (value != null)
                {
                    SiteMapNames.CheckTemplateVars(value);
                }
                siteagestats_mapNames = value;
            }
        }
        //---------------------------------------------------------------------

        public string SiteSppStats_MapNames
        {
            get
            {
                return sitesppstats_mapNames;
            }
            set
            {
                if (value != null)
                {
                    SiteMapNames.CheckTemplateVars(value);
                }
                sitesppstats_mapNames = value;
            }       
        }
		//---------------------------------------------------------------------

        public Dictionary<string, IEnumerable<ISpecies>> AgeStatSpecies
        {
            get
            {
                return ageStatSpecies;
            }
            set
            {
                ageStatSpecies = value;
            }
        }

        //---------------------------------------------------------------------

        public List<string> SiteAgeStats
        {
            get
            {
                return siteAgeStats;
            }
            set
            {
                siteAgeStats = value;
            }
        }

		//---------------------------------------------------------------------

        public List<string> SiteSppStats
        {
            get
            {
                return siteSppStats;
            }
            set
            {
                siteSppStats = value;
            }
        }

        //---------------------------------------------------------------------


        public InputParameters() { }

	}
}
