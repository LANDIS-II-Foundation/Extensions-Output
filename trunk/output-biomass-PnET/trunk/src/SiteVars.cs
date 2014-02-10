//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Robert M. Scheller, James B. Domingo

using Landis.Core;
using Landis.Library.BiomassCohortsPnET;
using Landis.Extension.Succession.BiomassPnET;
using System.Collections.Generic;
using Landis.SpatialModeling;

namespace Landis.Extension.Output.BiomassPnET
{
    /// <summary>
    /// The pools of dead biomass for the landscape's sites.
    /// </summary>
    public static class SiteVars
    {

        private static ISiteVar<Landis.Extension.Succession.Biomass.Pool> woodyDebris;
        private static ISiteVar<Landis.Extension.Succession.Biomass.Pool> litter;
        private static ISiteVar<Landis.Extension.Succession.Biomass.Species.AuxParm<int>> deadcohorts;
        private static ISiteVar<Landis.Extension.Succession.Biomass.Species.AuxParm<List<int>>> deadcohortages;
        
        private static ISiteVar<Landis.Extension.Succession.Biomass.Species.AuxParm<int>> newcohorts;

        private static ISiteVar<ISiteCohorts> cohorts;
        private static ISiteVar<Landis.Extension.Succession.Biomass.Species.AuxParm<int>> establishments;
        
        private static ISiteVar<float> soilwater;
        private static ISiteVar<float> annualtranspiration;
        private static ISiteVar<float> canopylaimax;
        private static ISiteVar<float> subcanopyparmax;
        
        
        //---------------------------------------------------------------------

        /// <summary>
        /// Initializes the module.
        /// </summary>
        public static void Initialize()
        {
            deadcohorts = PlugIn.ModelCore.GetSiteVar <Landis.Extension.Succession.Biomass.Species.AuxParm<int>>("Succession.DeadCohorts");
            deadcohortages = PlugIn.ModelCore.GetSiteVar<Landis.Extension.Succession.Biomass.Species.AuxParm<List<int>>>("Succession.DeadCohortAges");
            newcohorts = PlugIn.ModelCore.GetSiteVar<Landis.Extension.Succession.Biomass.Species.AuxParm<int>>("Succession.NewCohorts");

            woodyDebris = PlugIn.ModelCore.GetSiteVar<Landis.Extension.Succession.Biomass.Pool>("Succession.WoodyDebris");
            litter = PlugIn.ModelCore.GetSiteVar<Landis.Extension.Succession.Biomass.Pool>("Succession.Litter");
            establishments = PlugIn.ModelCore.GetSiteVar<Landis.Extension.Succession.Biomass.Species.AuxParm<int>>("Succession.Establishments");
            cohorts = PlugIn.ModelCore.GetSiteVar<ISiteCohorts>("Succession.BiomassCohortsPnET");
            
            if (cohorts == null)
            {
                string mesg = string.Format("Cohorts are empty.  Please double-check that this extension is compatible with your chosen succession extension.");
                throw new System.ApplicationException(mesg);
            }
            annualtranspiration = PlugIn.ModelCore.GetSiteVar<float>("Succession.AnnualTranspiration");
            subcanopyparmax = PlugIn.ModelCore.GetSiteVar<float>("Succession.SubCanopyPARmax");
            canopylaimax = PlugIn.ModelCore.GetSiteVar<float>("Succession.CanopyLAImax");
            soilwater = PlugIn.ModelCore.GetSiteVar<float>("Succession.SoilWater");
        }

        //---------------------------------------------------------------------
        public static ISiteVar<ISiteCohorts> Cohorts
        {
            get
            {
                if (cohorts == null)
                {
                    string mesg = string.Format("Cohorts are empty.  Please double-check that this extension is compatible with your chosen succession extension.");
                    throw new System.ApplicationException(mesg);
                }
                return cohorts;
            }
        }

        public static ISiteVar<Landis.Extension.Succession.Biomass.Species.AuxParm<int>> DeadCohorts
        {
            get
            {
                return deadcohorts;
            }
        }

        
        
        public static ISiteVar<Landis.Extension.Succession.Biomass.Species.AuxParm<List<int>>> DeadCohortAges
        {
            get
            {
                return deadcohortages ;
            }
        }
        public static ISiteVar<Landis.Extension.Succession.Biomass.Species.AuxParm<int>> NewCohorts
        {
            get
            {
                return newcohorts;
            }
        }
        public static ISiteVar<float> SubCanopyPARmax
        {
            get
            {
                return subcanopyparmax;
            }
        }
        public static ISiteVar<float> CanopyLAImax
        {
            get
            {
                return canopylaimax;
            }
        }
        public static ISiteVar<float> AnnualTranspiration
        {
            get
            {
                return annualtranspiration;
            }
        }
        public static ISiteVar<float> SoilWater
        {
            get
            {
                return soilwater;
            }
        }
        public static ISiteVar<Landis.Extension.Succession.Biomass.Species.AuxParm<int>> Establishments
        {
            get
            {
                return establishments;
            }
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// The intact dead woody pools for the landscape's sites.
        /// </summary>
        public static ISiteVar<Landis.Extension.Succession.Biomass.Pool> WoodyDebris
        {
            get {
                return woodyDebris;
            }
        }
        
        //---------------------------------------------------------------------
        /// <summary>
        /// The dead non-woody pools for the landscape's sites.
        /// </summary>
        public static ISiteVar<Landis.Extension.Succession.Biomass.Pool> Litter
        {
            get {
                return litter;
            }
        }

    }
}
