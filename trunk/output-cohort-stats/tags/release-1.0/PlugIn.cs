//  Copyright 2008 Conservation Biology Institute
//  Authors:  Brendan C. Ward
//  License:  N/A

using Landis.AgeCohort;
using Landis.Landscape;
using Landis.RasterIO;
using Landis.Species;

using System.Collections.Generic;
using System;

namespace Landis.Output.CohortStats
{
    public class PlugIn
        : PlugIns.PlugIn
    {
        private PlugIns.ICore modelCore;
        private string sppagestats_mapNames;
        private string siteagestats_mapNames;
        private string sitesppstats_mapNames;
        private Dictionary<string, IEnumerable<ISpecies>> ageStatSpecies;
        private List<string> siteAgeStats;
        private List<string> siteSppStats;
        private ILandscapeCohorts cohorts;

        //---------------------------------------------------------------------

        public PlugIn()
            : base("Species Cohort Stats", new PlugIns.PlugInType("output"))
        {
        }

        //---------------------------------------------------------------------

        public override void Initialize(string        dataFile,
                                        PlugIns.ICore modelCore)
        {
            this.modelCore = modelCore;

            ParametersParser.SpeciesDataset = modelCore.Species;
            ParametersParser parser = new ParametersParser();
            IParameters parameters = Data.Load<IParameters>(dataFile,parser);

            if (parameters==null)
                throw new ApplicationException("Error: Missing required parameters.  Check the input parameter file");


            Timestep = parameters.Timestep;
            sppagestats_mapNames = parameters.SppAgeStats_MapNames;
            siteagestats_mapNames = parameters.SiteAgeStats_MapNames;
            sitesppstats_mapNames = parameters.SiteSppStats_MapNames;
            ageStatSpecies = parameters.AgeStatSpecies;
            siteAgeStats = parameters.SiteAgeStats;
            siteSppStats = parameters.SiteSppStats;

            cohorts = modelCore.SuccessionCohorts as ILandscapeCohorts;
            if (cohorts == null)
                throw new ApplicationException("Error: Cohorts don't support age-cohort interface");
        }

        //---------------------------------------------------------------------

        public override void Run()
        {
            
            IOutputRaster<AgePixel> map;
            AgePixel pixel;

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
                    map = CreateSppMap(species.Name, sppAgeStatIter.Key);
                    using (map)
                    {
                        foreach (Site site in modelCore.Landscape.AllSites)
                        {
                            pixel = new AgePixel();
                            if (!site.IsActive) // and has the spp we want
                                pixel.Band0 = 0;
                            else
                            {
                                //need to do a switch on statistic
                                pixel.Band0 = species_stat_func(cohorts[site][species]);
                            }
                            map.WritePixel(pixel);
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

                map = CreateSiteMap(siteagestats_mapNames, ageStatIter);
                using (map)
                {
                    pixel = new AgePixel();
                    foreach (Site site in modelCore.Landscape.AllSites)
                    {
                        if (!site.IsActive)
                            pixel.Band0 = 0;
                        else
                            pixel.Band0 = site_stat_func(cohorts[site]);
                           
                        map.WritePixel(pixel);
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

                map = CreateSiteMap(sitesppstats_mapNames, sppStatIter);
                using (map)
                {
                    pixel = new AgePixel();
                    foreach (Site site in modelCore.Landscape.AllSites)
                    {
                        if (!site.IsActive)
                            pixel.Band0 = 0;
                        else
                            pixel.Band0 = site_stat_func(cohorts[site]);

                        map.WritePixel(pixel);
                    }
                }
            }



        }

        //---------------------------------------------------------------------

        private IOutputRaster<AgePixel> CreateSppMap(string species,string statistic)
        {
            string path = SpeciesMapNames.ReplaceTemplateVars(sppagestats_mapNames, species, statistic,
                                                       modelCore.CurrentTime);
            UI.WriteLine("Writing age map to {0} ...", path);
            return modelCore.CreateRaster<AgePixel>(path,
                                                    modelCore.Landscape.Dimensions,
                                                    modelCore.LandscapeMapMetadata);
        }

        //---------------------------------------------------------------------

        private IOutputRaster<AgePixel> CreateSiteMap(string template, string statistic)
        {
            string path = SiteMapNames.ReplaceTemplateVars(template, statistic,
                                                       modelCore.CurrentTime);
            UI.WriteLine("Writing age map to {0} ...", path);
            return modelCore.CreateRaster<AgePixel>(path,
                                                    modelCore.Landscape.Dimensions,
                                                    modelCore.LandscapeMapMetadata);
        }
    }
}

