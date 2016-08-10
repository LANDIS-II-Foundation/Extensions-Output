//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Arjan de Bruijn 

using Landis.Core;
using Edu.Wisc.Forest.Flel.Util;
using System.Collections.Generic;

namespace Landis.Extension.Output.PnET
{
    /// <summary>
    /// A parser that reads the plug-in's parameters from text input.
    /// </summary>
    public class InputParametersParser
        : Edu.Wisc.Forest.Flel.Util.TextParser<InputParameters>
    {

        //---------------------------------------------------------------------

        public InputParametersParser()
        {
        }

        //---------------------------------------------------------------------

        protected override InputParameters Parse()
        {
            InputVar<string> landisData = new InputVar<string>("LandisData");
            ReadVar(landisData);
            if (landisData.Value.Actual != PlugIn.ExtensionName)
                throw new InputValueException(landisData.Value.String, "The value is not \"{0}\"", PlugIn.ExtensionName);

            InputParameters parameters = new InputParameters();

            InputVar<int> timestep = new InputVar<int>("Timestep");
            ReadVar(timestep);
            parameters.Timestep = timestep.Value;

          

            //  Check for optional pair of parameters for species:
            //      Species
            //      MapNames
            InputVar<string> speciesName = new InputVar<string>("Species");
            InputVar<string> biomass = new InputVar<string>("Biomass");
            InputVar<string> abovegroundBiomass = new InputVar<string>("AbovegroundBiomass");
            InputVar<string> woodySenescence = new InputVar<string>("WoodySenescence");
            InputVar<string> foliageSenescence = new InputVar<string>("FoliageSenescence");
            InputVar<string> LeafAreaIndex = new InputVar<string>("LeafAreaIndex");
            InputVar<string> Establishment = new InputVar<string>("Establishment");
            InputVar<string> EstablishmentProbability = new InputVar<string>("EstablishmentProbability");
            InputVar<string> MonthlyNetPsn = new InputVar<string>("MonthlyNetPsn");
            InputVar<string> MonthlyFolResp = new InputVar<string>("MonthlyFolResp");
            InputVar<string> MonthlyGrossPsn = new InputVar<string>("MonthlyGrossPsn");
            InputVar<string> MonthlyMaintResp = new InputVar<string>("MonthlyMaintResp");
            InputVar<string> Water = new InputVar<string>("Water");
            InputVar<string> SubCanopyPAR = new InputVar<string>("SubCanopyPAR");
            InputVar<string> BelowgroundBiomass = new InputVar<string>("BelowgroundBiomass");
            InputVar<string> CohortsPerSpecies = new InputVar<string>("CohortsPerSpecies");
            InputVar<string> AnnualPsn = new InputVar<string>("AnnualPsn");
            InputVar<string> WoodyDebris = new InputVar<string>("WoodyDebris");
            InputVar<string> Litter = new InputVar<string>("Litter");
            InputVar<string> AgeDistribution = new InputVar<string>("AgeDistribution");
            InputVar<string> CohortBalance = new InputVar<string>("CohortBalance");
           
           
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

                while (!AtEndOfInput && CurrentName != biomass.Name)
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

                if (ReadOptionalVar(biomass))
                {
                    parameters.SpeciesBiom = biomass.Value;
                    continue;
                }
                if (ReadOptionalVar(abovegroundBiomass))
                {
                    parameters.SpeciesAbovegroundBiom = abovegroundBiomass.Value;
                    continue;
                }
                if (ReadOptionalVar(woodySenescence))
                {
                    parameters.SpeciesWoodySenescence = woodySenescence.Value;
                    continue;
                }
                if (ReadOptionalVar(foliageSenescence))
                {
                    parameters.SpeciesFoliageSenescence = foliageSenescence.Value;
                    continue;
                }
                if (ReadOptionalVar(MonthlyNetPsn))
                {
                    parameters.MonthlyNetPsn = MonthlyNetPsn.Value;
                    continue;
                }
                
                if (ReadOptionalVar(MonthlyFolResp))
                {
                    parameters.MonthlyFolResp = MonthlyFolResp.Value;
                    continue;
                }
                if (ReadOptionalVar(MonthlyGrossPsn))
                {
                    parameters.MonthlyGrossPsn = MonthlyGrossPsn.Value;
                    continue;
                }
                if (ReadOptionalVar(MonthlyMaintResp))
                {
                    parameters.MonthlyMaintResp = MonthlyMaintResp.Value;
                    continue;
                }
       
                if (ReadOptionalVar(LeafAreaIndex))
                {
                    parameters.LeafAreaIndex = LeafAreaIndex.Value;
                    continue;
                }
                if (ReadOptionalVar(Establishment))
                {
                    parameters.SpeciesEst = Establishment.Value;
                    continue;
                }
                if (ReadOptionalVar(EstablishmentProbability))
                {
                    parameters.EstablishmentProbability = EstablishmentProbability.Value;
                    continue;
                }
                if (ReadOptionalVar(Water))
                {
                    parameters.Water = Water.Value;
                    continue;
                }
               
                if (ReadOptionalVar(SubCanopyPAR))
                {
                    parameters.SubCanopyPAR = SubCanopyPAR.Value;
                    continue;
                }
                if (ReadOptionalVar(AnnualPsn))
                {
                    parameters.AnnualPsn = AnnualPsn.Value;
                    continue;
                }
                if (ReadOptionalVar(CohortsPerSpecies))
                {
                    parameters.CohortsPerSpecies = CohortsPerSpecies.Value;
                    continue;
                }
                if (ReadOptionalVar(BelowgroundBiomass))
                {
                    parameters.BelowgroundBiomass = BelowgroundBiomass.Value;
                    continue;
                }
                if (ReadOptionalVar(WoodyDebris))
                {
                    parameters.WoodyDebris  = WoodyDebris.Value;
                    continue;
                }
                if (ReadOptionalVar(Litter))
                {
                    parameters.Litter = Litter.Value;
                    continue;
                }
                if (ReadOptionalVar(AgeDistribution))
                {
                    parameters.AgeDistribution = AgeDistribution.Value;
                    continue;
                }
                if (ReadOptionalVar(CohortBalance))
                {
                    parameters.CohortBalance = CohortBalance.Value;
                    continue;
                }

                throw new System.Exception("Error in Output PnET cannot assign variable " + new StringReader(CurrentLine).ReadToEnd());

            }
            return parameters;  
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
