using Landis.Core;
using Edu.Wisc.Forest.Flel.Util;
using Landis.Library.BiomassCohortsPnET;
using Landis.SpatialModeling;
using Landis.Extension.Succession.BiomassPnET;
using System;
using System.Collections.Generic;
using System.IO;

namespace Landis.Extension.Output.BiomassPnET
{
    public static class OutputVariables
    {
        static IEnumerable<ISpecies> SelectedSpecies;

        public static OutputVariable Biomass;
        public static OutputVariable BelowGround;
        public static OutputVariable LAI;
        public static OutputVariable SpeciesEstablishment;
        public static OutputVariable Water;
        public static OutputVariable AnnualTranspiration;
        public static OutputVariable SubCanopyPAR;

        private delegate float GetSiteValue(Site s);
        private static float SiteAverage(GetSiteValue get)
        {
            float sum = 0;
            float c = 0;
            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                sum += get(site);
                c++;
            }
            return SiteTotal(get) / c; 
        }
        private static float SiteTotal(GetSiteValue get)
        {
            float sum = 0;
            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                sum += get(site);
            }
            return sum;
        }
        private static float SiteAverage(ISpecies species, DelegateFunctions.GetSpeciesSpecificValue get)
        {
            float sum = 0;
            float c = 0;
            float B = 0;
            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                B = get(species, site); 
                while (B<0 || sum < 0)
                {
                    B = get(species, site);
                }
                sum += B;
                c++;
            }
            return sum / c;
        }

        //----------------------------------
        public static Landis.Extension.Succession.Biomass.Species.AuxParm<float> GetAllSpeciesAverageBiomass()
        {
            Landis.Extension.Succession.Biomass.Species.AuxParm<float> sum = GetAllSpeciesBiomass();
            Landis.Extension.Succession.Biomass.Species.AuxParm<float> average = new Landis.Extension.Succession.Biomass.Species.AuxParm<float>(PlugIn.ModelCore.Species);
            foreach (ISpecies species in PlugIn.ModelCore.Species)
            { 
                average[species]= sum[species]/ PlugIn.ModelCore.Landscape.ActiveSiteCount;
            }
            return average;
        }
        public static Landis.Extension.Succession.Biomass.Species.AuxParm<float> GetAllSpeciesBiomass()
        {
            Landis.Extension.Succession.Biomass.Species.AuxParm<float> sum = new Landis.Extension.Succession.Biomass.Species.AuxParm<float>(PlugIn.ModelCore.Species);
            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
                {
                   //System.Console.WriteLine("species " + speciesCohorts.Species.Name);
                   foreach (ICohort cohort in speciesCohorts)
                   {
                       sum[speciesCohorts.Species] += cohort.Biomass;
                   }
                }
            }
             
            return sum;
        }
        public static Landis.Extension.Succession.Biomass.Species.AuxParm<float> GetAllSpeciesPest()
        {
            Landis.Extension.Succession.Biomass.Species.AuxParm<float> est = new Landis.Extension.Succession.Biomass.Species.AuxParm<float>(PlugIn.ModelCore.Species);
            
            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                //System.Console.WriteLine("species " + speciesCohorts.Species.Name);
                foreach (ISpecies species in PlugIn.ModelCore.Species)
                {
                    if (SiteVars.Establishments[site][species] == true)
                    {
                        est[species]++;
                    }
                }
            }

            return est;
        }
        public static int GetSpeciesSpecificPest(ISpecies species, Site site)
        {
            if(SiteVars.Establishments[site][species])return 1;
            else return 0;
        }
        public static int GetSpeciesSpecificBiomass(ISpecies species, Site site)
        {
            int sum = 0;
            foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
            {
                //System.Console.WriteLine("species " + speciesCohorts.Species.Name);
                foreach (ICohort cohort in speciesCohorts)
                {
                    if (speciesCohorts.Species == species) sum+= cohort.Biomass;
                }
            }
            return sum;
        }
        
        //----- Site values
        public static float GetSiteWater(Site site)
        {
            float v = SiteVars.SoilWater[site];
            return Convert.ToInt32(v);
            
        }
        
        public static int GetSiteSpeciesEst(Site site)
        {
            int est = 0;
            foreach (ISpecies species in SelectedSpecies)
            {
                if(SiteVars.Establishments[site][species])est ++;
            }
            return est;
        }
         
        public static float GetSiteLAI(Site site)
        {
            return 10 * SiteVars.CanopyLAImax[site]; // tenths of LAI!!
        }

        public static float GetSiteBelowGroundBiomass(Site site)
        {
            float Biomass = 0;
            foreach (ISpeciesCohorts spc in SiteVars.Cohorts[site])
            {
                foreach (ICohort c in spc)
                {
                    Biomass += c.Root;
                }
            }
            return Biomass;
        }
        public static float GetSiteBiomass(Site site)
        {
            float Biomass = 0;
            foreach (ISpeciesCohorts spc in SiteVars.Cohorts[site])
            {
                foreach (ICohort c in spc)
                {
                    Biomass += c.Biomass;
                }
            }
            return Biomass;
        }
        public static float GetSiteAnnualTranspiration(Site site)
        {
            return SiteVars.AnnualTranspiration[site];
        }
        public static float GetSiteSubCanopyPAR(Site site)
        {
            return SiteVars.SubCanopyPARmax[site];
        }

        // overall averages for the whole map
        public static float GetWater()
        {
            return SiteAverage(GetSiteWater);
        }
        public static float GetLAI()
        {
            return SiteAverage(GetSiteLAI);
        }
        public static float GetSubCanopyPAR()
        {
            return SiteAverage(GetSiteSubCanopyPAR);
        }
        public static float GetAnnualTranspiration()
        {
            return SiteAverage(GetSiteAnnualTranspiration);
        }
        public static float GetAverageBiomass()
        {
            return SiteAverage(GetSiteBiomass);
        }
        public static float GetTotalBiomass()
        {
            return SiteTotal(GetSiteBiomass);
        }
        //------------------------------

        public static float GetTotalBelowGroundBiomass()
        {
            return SiteTotal(GetSiteBelowGroundBiomass);
        }
         
        
        public static void Update()
        {
            BelowGround.Update();
            Biomass.Update();
            LAI.Update();
            SpeciesEstablishment.Update();
            Water.Update();
            AnnualTranspiration.Update();
            SubCanopyPAR.Update();
        }
        public static void Initialize(IEnumerable<ISpecies> selectedSpecies, IInputParameters parameters)
        {
            SelectedSpecies = selectedSpecies;

            BelowGround = new OutputVariable(selectedSpecies, null, GetTotalBelowGroundBiomass, GetSiteBiomass, null, parameters.BelowgroundMapNames, "BelowGround");
            Biomass = new OutputVariable(selectedSpecies, GetAllSpeciesAverageBiomass, GetTotalBiomass, GetSiteBiomass, GetSpeciesSpecificBiomass, parameters.SpeciesBiomMapNames, "Biomass");
            LAI = new OutputVariable(selectedSpecies, null, null, GetSiteLAI, null, parameters.SpeciesLAIMapNames, "LAI");
            SpeciesEstablishment = new OutputVariable(selectedSpecies, GetAllSpeciesPest, null, null, GetSpeciesSpecificPest, parameters.SpeciesEstMapNames, "SEP");
            Water = new OutputVariable(selectedSpecies, null, null, GetSiteWater, null, parameters.WaterMapNameTemplate, "Water");
            AnnualTranspiration = new OutputVariable(selectedSpecies, null, null, GetSiteAnnualTranspiration, null, parameters.AnnualTranspirationMapNames, "AnnualTranspiration");
            SubCanopyPAR = new OutputVariable(selectedSpecies, null, null, GetSiteSubCanopyPAR, null, parameters.SubCanopyPARMapNames, "SubCanopyPAR");

            
           
        }
    }
}
