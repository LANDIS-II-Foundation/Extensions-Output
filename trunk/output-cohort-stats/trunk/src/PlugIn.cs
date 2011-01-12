//  Copyright 2008-2010  Portland State University, Conservation Biology Institute
//  Authors:  Brendan C. Ward, Robert M. Scheller

using Landis.Library.AgeOnlyCohorts;
using Landis.Core;
using Landis.SpatialModeling;

using System.Collections.Generic;
using System;

namespace Landis.Extension.Output.CohortStats
{
    public class PlugIn
        : ExtensionMain
    {
        public static readonly ExtensionType Type = new ExtensionType("output");
        public static readonly string ExtensionName = "Output Cohort Statistics";
        
        private static ICore modelCore;
        private string sppagestats_mapNames;
        private string siteagestats_mapNames;
        private string sitesppstats_mapNames;
        private Dictionary<string, IEnumerable<ISpecies>> ageStatSpecies;
        private List<string> siteAgeStats;
        private List<string> siteSppStats;
        private IInputParameters parameters;

        //---------------------------------------------------------------------

        public PlugIn()
            : base(ExtensionName, Type)
        {
        }

        //---------------------------------------------------------------------

        public static ICore ModelCore
        {
            get
            {
                return modelCore;
            }
        }
        //---------------------------------------------------------------------

        public override void LoadParameters(string dataFile, ICore mCore)
        {
            modelCore = mCore;
            SiteVars.Initialize();
            InputParameterParser parser = new InputParameterParser();
            parameters = mCore.Load<IInputParameters>(dataFile, parser);
            
            if (parameters == null)
                throw new ApplicationException("Error: Missing required parameters.  Check the input parameter file");

        }
        //---------------------------------------------------------------------

        public override void Initialize(string dataFile)
        {

            Timestep = parameters.Timestep;
            sppagestats_mapNames = parameters.SppAgeStats_MapNames;
            siteagestats_mapNames = parameters.SiteAgeStats_MapNames;
            sitesppstats_mapNames = parameters.SiteSppStats_MapNames;
            ageStatSpecies = parameters.AgeStatSpecies;
            siteAgeStats = parameters.SiteAgeStats;
            siteSppStats = parameters.SiteSppStats;

        }

        //---------------------------------------------------------------------

        public override void Run()
        {
            
            //1) Create the output species age stats maps
            foreach (KeyValuePair<string, IEnumerable<ISpecies>> sppAgeStatIter in ageStatSpecies)
            {
                //statIter.Key = statistic name
                //set a function pointer here for the statistic, so we don't have to do the switch operation for every single pixel every single time??
                CohortUtils.SpeciesCohortStatDelegate species_stat_func;
                
                switch (sppAgeStatIter.Key)
                {
                    case "MAX":
                        species_stat_func = new CohortUtils.SpeciesCohortStatDelegate(CohortUtils.GetMaxAge);
                        break;
                    case "MIN":
                        species_stat_func = new CohortUtils.SpeciesCohortStatDelegate(CohortUtils.GetMinAge);                     
                        break;
                    case "MED":
                        species_stat_func = new CohortUtils.SpeciesCohortStatDelegate(CohortUtils.GetMedianAge);
                        break;
                    case "AVG":
                        species_stat_func = new CohortUtils.SpeciesCohortStatDelegate(CohortUtils.GetAvgAge);
                        break;
                    case "SD":
                        species_stat_func = new CohortUtils.SpeciesCohortStatDelegate(CohortUtils.GetStdDevAge);
                        break;

                    default:
                        //this shouldn't ever occur
                        System.Console.WriteLine("Unhandled statistic: {0}, using MaxAge Instead",sppAgeStatIter.Key);
                        species_stat_func = new CohortUtils.SpeciesCohortStatDelegate(CohortUtils.GetMaxAge);
                        break;
                }

                foreach (ISpecies species in sppAgeStatIter.Value)
                {
                    string path = SpeciesMapNames.ReplaceTemplateVars(sppagestats_mapNames, species.Name, sppAgeStatIter.Key, modelCore.CurrentTime);
                    ModelCore.Log.WriteLine("   Writing {0} map for {1} to {2} ...", sppAgeStatIter.Key, species.Name, path);
                    using (IOutputRaster<UShortPixel> outputRaster = modelCore.CreateRaster<UShortPixel>(path, modelCore.Landscape.Dimensions))
                    {
                        UShortPixel pixel = outputRaster.BufferPixel;
                        foreach (Site site in modelCore.Landscape.AllSites)
                        {
                            if (!site.IsActive) // and has the spp we want
                                pixel.MapCode.Value = 0;
                            else
                            {
                                //need to do a switch on statistic
                                pixel.MapCode.Value = species_stat_func(SiteVars.Cohorts[site][species]);
                            }

                            outputRaster.WriteBufferPixel();
                        }
                    }
                }
            }

            //2) Create the output site age stats maps
            foreach(string ageStatIter in siteAgeStats)
            {
                CohortUtils.SiteCohortStatDelegate site_stat_func;
                switch (ageStatIter)
                {
                    case "MAX":
                        site_stat_func = new CohortUtils.SiteCohortStatDelegate(CohortUtils.GetMaxAge);
                        break;
                    case "MIN":
                        site_stat_func = new CohortUtils.SiteCohortStatDelegate(CohortUtils.GetMinAge);
                        break;
                    case "MED":
                        site_stat_func = new CohortUtils.SiteCohortStatDelegate(CohortUtils.GetMedianAge);
                        break;
                    case "AVG":
                        site_stat_func = new CohortUtils.SiteCohortStatDelegate(CohortUtils.GetAvgAge);
                        break;
                    case "SD":
                        site_stat_func = new CohortUtils.SiteCohortStatDelegate(CohortUtils.GetStdDevAge);
                        break;
                    case "RICH":
                        //FIXME
                        site_stat_func = new CohortUtils.SiteCohortStatDelegate(CohortUtils.GetAgeRichness);
                        break;
                    case "EVEN":

                        //FIXME!
                        site_stat_func = new CohortUtils.SiteCohortStatDelegate(CohortUtils.GetAgeEvenness);
                        break;                        

                    default:
                        System.Console.WriteLine("Unhandled statistic: {0}, using MaxAge Instead", ageStatIter);
                        site_stat_func = new CohortUtils.SiteCohortStatDelegate(CohortUtils.GetMaxAge);
                        break;
                }

                string path = SiteMapNames.ReplaceTemplateVars(siteagestats_mapNames, ageStatIter, modelCore.CurrentTime);
                ModelCore.Log.WriteLine("   Writing {0} site map to {1} ...", ageStatIter, path);
                using (IOutputRaster<UShortPixel> outputRaster = modelCore.CreateRaster<UShortPixel>(path, modelCore.Landscape.Dimensions))
                {
                    UShortPixel pixel = outputRaster.BufferPixel;
                    foreach (Site site in modelCore.Landscape.AllSites)
                    {
                        if (!site.IsActive)
                            pixel.MapCode.Value = 0;
                        else
                            pixel.MapCode.Value = site_stat_func(SiteVars.Cohorts[site]);

                        outputRaster.WriteBufferPixel();
                    }
                }
            }

            //3) Create the output site species stats maps
            foreach (string sppStatIter in siteSppStats)
            {
                CohortUtils.SiteCohortStatDelegate site_stat_func;
                switch (sppStatIter)
                {
                    case "RICH":
                        //FIXME
                        site_stat_func = new CohortUtils.SiteCohortStatDelegate(CohortUtils.GetSppRichness);
                        break;
                    //add in richness
                    default:
                        System.Console.WriteLine("Unhandled statistic: {0}, using Species Richness Instead", sppStatIter);
                        site_stat_func = new CohortUtils.SiteCohortStatDelegate(CohortUtils.GetSppRichness);
                        break;
                }

                string path = SiteMapNames.ReplaceTemplateVars(sitesppstats_mapNames, sppStatIter, modelCore.CurrentTime);
                ModelCore.Log.WriteLine("   Writing {0} site map to {1} ...", sppStatIter, path);
                using (IOutputRaster<UShortPixel> outputRaster = modelCore.CreateRaster<UShortPixel>(path, modelCore.Landscape.Dimensions))
                {
                    UShortPixel pixel = outputRaster.BufferPixel;
                    foreach (Site site in modelCore.Landscape.AllSites)
                    {
                        if (!site.IsActive)
                            pixel.MapCode.Value = 0;
                        else
                            pixel.MapCode.Value = site_stat_func(SiteVars.Cohorts[site]);

                        outputRaster.WriteBufferPixel();
                    }
                }
            }



        }

        //---------------------------------------------------------------------
/*
        private IOutputRaster<AgePixel> CreateSppMap(string species,string statistic)
        {
            string path = SpeciesMapNames.ReplaceTemplateVars(sppagestats_mapNames, species, statistic,
                                                       modelCore.CurrentTime);
            ModelCore.Log.WriteLine("Writing age map to {0} ...", path);
            return modelCore.CreateRaster<AgePixel>(path,
                                                    modelCore.Landscape.Dimensions,
                                                    modelCore.LandscapeMapMetadata);
        }

        //---------------------------------------------------------------------

        private IOutputRaster<AgePixel> CreateSiteMap(string template, string statistic)
        {
            string path = SiteMapNames.ReplaceTemplateVars(template, statistic,
                                                       modelCore.CurrentTime);
            ModelCore.Log.WriteLine("Writing age map to {0} ...", path);
            return modelCore.CreateRaster<AgePixel>(path,
                                                    modelCore.Landscape.Dimensions,
                                                    modelCore.LandscapeMapMetadata);
        }*/
    }
}

