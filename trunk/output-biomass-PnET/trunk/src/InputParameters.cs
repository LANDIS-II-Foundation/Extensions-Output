//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Robert M. Scheller, James B. Domingo

using Landis.Core;
using Edu.Wisc.Forest.Flel.Util;
using System.Collections.Generic;

namespace Landis.Extension.Output.BiomassPnET
{
    /// <summary>
    /// The input parameters for the plug-in.
    /// </summary>
    public class InputParameters
        : IInputParameters
    {
        private int timestep;
        private IEnumerable<ISpecies> selectedSpecies;
        private string speciesBiomMapNames;
        private string speciesLAIMapNames;
        private string speciesEestMapNames;
        private string annualtranspirationmapNames;
        private string woodydebrismapnames;
        private string littermapnames;
        private string agedistributionfilenames;
        private string speciesspecestfilename;
        private string cohortdeathfreqfilename;
        private string deathagedistributionfileNames;
        private string belowgroundmapnames;
        private string subcanopyPARmapnames;
        private string poolMapNames;
        private string cohortbalancefilename;
        private string biomassperecoregionfilename;

        
        private bool makeTable;
        string waterMapNameTemplate;
        
        //---------------------------------------------------------------------

        public int Timestep
        {
            get {
                return timestep;
            }
            set {
                if (value < 0)
                    throw new InputValueException(value.ToString(), "Value must be = or > 0");
                timestep = value;
            }
        }

        public string BiomassPerEcoregionFileName
        {
            get {
                return biomassperecoregionfilename;
            }
            set {
                biomassperecoregionfilename = value;
            }
        }
        public string CohortBalanceFileName
        {
            get {
                return cohortbalancefilename;
            }
            set {
                cohortbalancefilename = value;
            }
        }
        public IEnumerable<ISpecies> SelectedSpecies
        {
            get {
                return selectedSpecies;
            }
            set {
                selectedSpecies = value;
            }
        }
        public string WoodyDebrisMapNames
        {
            get
            {
                return woodydebrismapnames;
            }
            set
            {
                woodydebrismapnames = value;
            }
        }
        
        public string DeathAgeDistributionFileNames
        {
            get
            {
                return deathagedistributionfileNames;
            }
            set
            {
                deathagedistributionfileNames = value;
            }
        }
        public string AgeDistributionFileNames
        {
            get
            {
                return agedistributionfilenames;
            }
            set
            {
                agedistributionfilenames = value;
            }
        }
        public string SpeciesSpecEstFileName
        {
            get
            {
                return speciesspecestfilename;
            }
            set
            {
                speciesspecestfilename = value;
            }
        }
        
        public string CohortDeathFreqFileName
        {
            get
            {
                return cohortdeathfreqfilename;
            }
            set
            {
                cohortdeathfreqfilename = value;
            }
        }
          
        public string LitterMapNames
        {
            get
            {
                return littermapnames;
            }
            set
            {
                littermapnames = value;
            }
        }
        public string BelowgroundMapNames
        {
            get
            {
                return belowgroundmapnames;
            }
            set
            {
                belowgroundmapnames = value;
            }
        }
        
        public string AnnualTranspirationMapNames
        {
            get {
                return annualtranspirationmapNames;
            }
            set {
                annualtranspirationmapNames = value;
            }
        }
        public string SubCanopyPARMapNames
        {
            get {
                return subcanopyPARmapnames;
            }
            set {
                subcanopyPARmapnames = value;
            }
        }


        public string SpeciesEstMapNames
        {
            get {
                return speciesEestMapNames;
            }
            set {
                speciesEestMapNames = value;
            }
        }
        public string SpeciesLAIMapNames
        {
            get {
                return speciesLAIMapNames;
            }
            set {
                //Biomass.SpeciesMapNames.CheckTemplateVars(value);
                speciesLAIMapNames = value;
            }
        }
        
        public string WaterMapNameTemplate
        {
            get
            {
                return waterMapNameTemplate;
            }
            set 
            {
                waterMapNameTemplate = value;
            }
        }

        public string SpeciesBiomMapNames
        {
            get {
                return speciesBiomMapNames;
            }
            set {
                BiomassPnET.FileNames.CheckTemplateVars(value);
                speciesBiomMapNames = value;
            }
        }

       
        //---------------------------------------------------------------------

        public string PoolMapNames
        {
            get {
                return poolMapNames;
            }
            set {
                BiomassPnET.FileNames.CheckTemplateVars(value); //, selectedPools);
                poolMapNames = value;
            }
        }

        public InputParameters()
        {
        }
        //---------------------------------------------------------------------

        public bool MakeTable
        {
            get
            {
                return makeTable;
            }
            set
            {
                makeTable = value;
            }
        }
    }
}
