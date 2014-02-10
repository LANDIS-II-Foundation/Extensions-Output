//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Robert M. Scheller, James B. Domingo

using Landis.Core;
using Edu.Wisc.Forest.Flel.Util;
using System.Collections.Generic;

namespace Landis.Extension.Output.BiomassPnET
{
    /// <summary>
    /// A parser that reads the plug-in's parameters from text input.
    /// </summary>
    public class InputParametersParser
        : Edu.Wisc.Forest.Flel.Util.TextParser<IInputParameters>
    {

        //---------------------------------------------------------------------

        public InputParametersParser()
        {
        }

        //---------------------------------------------------------------------

        protected override IInputParameters Parse()
        {
            InputVar<string> landisData = new InputVar<string>("LandisData");
            ReadVar(landisData);
            if (landisData.Value.Actual != PlugIn.ExtensionName)
                throw new InputValueException(landisData.Value.String, "The value is not \"{0}\"", PlugIn.ExtensionName);

            InputParameters parameters = new InputParameters();

            InputVar<int> timestep = new InputVar<int>("Timestep");
            ReadVar(timestep);
            parameters.Timestep = timestep.Value;

            InputVar<bool> makeTable = new InputVar<bool>("MakeTable");
            if (ReadOptionalVar(makeTable))
                parameters.MakeTable = makeTable.Value;
            else
                parameters.MakeTable = false;


            //  Check for optional pair of parameters for species:
            //      Species
            //      MapNames
            InputVar<string> speciesName = new InputVar<string>("Species");
            InputVar<string> biomassMapNames = new InputVar<string>("BiomassMapNames");
            InputVar<string> laiMapNames = new InputVar<string>("LaiMapNames");
            InputVar<string> EstMapNames = new InputVar<string>("EstMapNames");
            InputVar<string> WaterMapNames = new InputVar<string>("WaterMapNames");
            InputVar<string> AnnualTranspirationMapNames = new InputVar<string>("AnnualTranspirationMapNames");
            InputVar<string> SubCanopyPARMapNames = new InputVar<string>("SubCanopyPARMapNames");
            InputVar<string> BelowgroundMapNames = new InputVar<string>("BelowgroundMapNames");
            InputVar<string> WoodyDebrisMapNames = new InputVar<string>("WoodyDebrisMapNames");
            InputVar<string> LitterMapNames = new InputVar<string>("LitterMapNames");
            InputVar<string> AgeDistributionFileNames = new InputVar<string>("AgeDistributionFileNames");
            InputVar<string> DeathAgeDistributionFileNames = new InputVar<string>("DeathAgeDistributionFileNames");
            InputVar<string> SpeciesSpecEstFileName = new InputVar<string>("SpeciesSpecEstFileName");
            InputVar<string> CohortDeathFreqFileName = new InputVar<string>("CohortDeathFreqFileName");
            InputVar<string> CohortBalanceFilename = new InputVar<string>("CohortBalanceFileName");
            InputVar<string> BiomassPerEcoregionFileName = new InputVar<string>("BiomassPerEcoregionFileName");
           
            int lineNumber = LineNumber;
            ReadVar(speciesName);
             
            if (System.String.Compare(speciesName.Value.Actual, "all", System.StringComparison.OrdinalIgnoreCase) == 0) 
            {
                parameters.SelectedSpecies = PlugIn.ModelCore.Species;
            }
            else if (System.String.Compare(speciesName.Value.Actual, "none", System.StringComparison.OrdinalIgnoreCase) == 0) 
            {
                parameters.SelectedSpecies = new List<ISpecies>();
            }
            else {
                ISpecies species = GetSpecies(speciesName.Value);
                List<ISpecies> selectedSpecies = new List<ISpecies>();
                selectedSpecies.Add(species);
                parameters.SelectedSpecies = selectedSpecies;

                Dictionary<string, int> lineNumbers = new Dictionary<string, int>();
                lineNumbers[species.Name] = lineNumber;

                while (!AtEndOfInput && CurrentName != biomassMapNames.Name)
                {
                    StringReader currentLine = new StringReader(CurrentLine);

                    ReadValue(speciesName, currentLine);
                    species = GetSpecies(speciesName.Value);
                    if (lineNumbers.TryGetValue(species.Name, out lineNumber))
                        throw new InputValueException(speciesName.Value.String,
                                                      "The species {0} was previously used on line {1}",
                                                      speciesName.Value.String, lineNumber);
                    lineNumbers[species.Name] = LineNumber;

                    selectedSpecies.Add(species);
                    CheckNoDataAfter("the species name", currentLine);
                    GetNextLine();
                }
            }

            while (!AtEndOfInput)
            {
                bool FoundVariable = false;

                if (ReadOptionalVar(biomassMapNames))
                {
                    parameters.SpeciesBiomMapNames = biomassMapNames.Value;
                    FoundVariable = true;
                }
                if (ReadOptionalVar(laiMapNames))
                {
                    parameters.SpeciesLAIMapNames = laiMapNames.Value;
                    FoundVariable = true;
                }
                if (ReadOptionalVar(EstMapNames))
                {
                    parameters.SpeciesEstMapNames = EstMapNames.Value;
                    FoundVariable = true;
                }
                if (ReadOptionalVar(WaterMapNames))
                {
                    parameters.WaterMapNameTemplate = WaterMapNames.Value;
                    FoundVariable = true;
                }
                if (ReadOptionalVar(AnnualTranspirationMapNames))
                {
                    parameters.AnnualTranspirationMapNames = AnnualTranspirationMapNames.Value;
                    FoundVariable = true;
                }
                if (ReadOptionalVar(SubCanopyPARMapNames))
                {
                    parameters.SubCanopyPARMapNames = SubCanopyPARMapNames.Value;
                   FoundVariable = true;
                }
                if (ReadOptionalVar(BelowgroundMapNames))
                {
                    parameters.BelowgroundMapNames = BelowgroundMapNames.Value;
                    FoundVariable = true;
                }
                if (ReadOptionalVar(WoodyDebrisMapNames))
                {
                    parameters.WoodyDebrisMapNames = WoodyDebrisMapNames.Value;
                    FoundVariable = true;
                }
                if (ReadOptionalVar(LitterMapNames))
                {
                    parameters.LitterMapNames = LitterMapNames.Value;
                    FoundVariable = true;
                }
                if (ReadOptionalVar(AgeDistributionFileNames))
                {
                    parameters.AgeDistributionFileNames = AgeDistributionFileNames.Value;
                    FoundVariable = true;
                }
                if (ReadOptionalVar(DeathAgeDistributionFileNames))
                {
                    parameters.DeathAgeDistributionFileNames = DeathAgeDistributionFileNames.Value;
                    FoundVariable = true;
                }
                 
                if (ReadOptionalVar(SpeciesSpecEstFileName))
                {
                    parameters.SpeciesSpecEstFileName = SpeciesSpecEstFileName.Value;
                    FoundVariable = true;
                }
                if (ReadOptionalVar(CohortDeathFreqFileName))
                {
                    parameters.CohortDeathFreqFileName = CohortDeathFreqFileName.Value;
                    FoundVariable = true;
                }
                if (ReadOptionalVar(CohortBalanceFilename))
                {
                    parameters.CohortBalanceFileName = CohortBalanceFilename.Value;
                    FoundVariable = true;
                }
                if (ReadOptionalVar(BiomassPerEcoregionFileName))
                {
                    parameters.BiomassPerEcoregionFileName = BiomassPerEcoregionFileName.Value;
                    FoundVariable = true;
                }
                   

                if (!FoundVariable)
                {
                    throw new System.Exception("Error in Output PnET");
                }
            }               
             
            return parameters; //.GetComplete();
        }

        //---------------------------------------------------------------------


        protected ISpecies GetSpecies(InputValue<string> name)
        {
            ISpecies species = PlugIn.ModelCore.Species[name.Actual];
            if (species == null)
                throw new InputValueException(name.String,
                                              "{0} is not a species name",
                                              name.String);
            return species;
        }
    }
}
