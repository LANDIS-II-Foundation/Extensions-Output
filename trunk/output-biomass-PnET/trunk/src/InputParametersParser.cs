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

          

            //  Check for optional pair of parameters for species:
            //      Species
            //      MapNames
            InputVar<string> speciesName = new InputVar<string>("Species");
            InputVar<string> biomass = new InputVar<string>("Biomass");
            InputVar<string> LeafAreaIndex = new InputVar<string>("LeafAreaIndex");
            InputVar<string> Establishment = new InputVar<string>("Establishment");
            InputVar<string> Water = new InputVar<string>("Water");
            InputVar<string> AnnualTranspiration= new InputVar<string>("AnnualTranspiration");
            InputVar<string> SubCanopyPAR = new InputVar<string>("SubCanopyPAR");
            InputVar<string> BelowgroundBiomass = new InputVar<string>("BelowgroundBiomass");
            InputVar<string> CohortsPerSpecies = new InputVar<string>("CohortsPerSpecies");
            InputVar<string> WoodyDebris = new InputVar<string>("WoodyDebris");
            InputVar<string> Litter = new InputVar<string>("Litter");
            InputVar<string> AgeDistribution = new InputVar<string>("AgeDistribution");
            InputVar<string> DeadCohortAges = new InputVar<string>("DeadCohortAges");
            InputVar<string> DeadCohortNumbers = new InputVar<string>("DeadCohortNumbers");
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

            //System.Console.WriteLine("ASSIGNING KEYWORDS OUTPUT MODULE");

            while (!AtEndOfInput)
            {
                bool FoundVariable = false;

                
                if (ReadOptionalVar(biomass))
                {
                    parameters.SpeciesBiom = biomass.Value;
                    FoundVariable = true;
                }
                if (ReadOptionalVar(LeafAreaIndex))
                {
                    parameters.LeafAreaIndex = LeafAreaIndex.Value;
                    FoundVariable = true;
                }
                if (ReadOptionalVar(Establishment))
                {
                    parameters.SpeciesEst = Establishment.Value;
                    FoundVariable = true;
                }
                if (ReadOptionalVar(Water))
                {
                    parameters.Water = Water.Value;
                    FoundVariable = true;
                }
                if (ReadOptionalVar(AnnualTranspiration))
                {
                    parameters.AnnualTranspiration = AnnualTranspiration.Value;
                    FoundVariable = true;
                }
                if (ReadOptionalVar(SubCanopyPAR))
                {
                    parameters.SubCanopyPAR = SubCanopyPAR.Value;
                   FoundVariable = true;
                }

                if (ReadOptionalVar(CohortsPerSpecies))
                {
                    parameters.CohortsPerSpecies = CohortsPerSpecies.Value;
                    FoundVariable = true;
                }
                if (ReadOptionalVar(BelowgroundBiomass))
                {
                    parameters.BelowgroundBiomass = BelowgroundBiomass.Value;
                    FoundVariable = true;
                }
                if (ReadOptionalVar(WoodyDebris))
                {
                    parameters.WoodyDebris  = WoodyDebris.Value;
                    FoundVariable = true;
                }
                if (ReadOptionalVar(Litter))
                {
                    parameters.Litter = Litter.Value;
                    FoundVariable = true;
                }
                if (ReadOptionalVar(AgeDistribution))
                {
                    parameters.AgeDistribution = AgeDistribution.Value;
                    FoundVariable = true;
                }
                if (ReadOptionalVar(DeadCohortAges))
                {
                    parameters.DeadCohortAges = DeadCohortAges.Value;
                    FoundVariable = true;
                }
                if (ReadOptionalVar(DeadCohortNumbers))
                {
                    parameters.DeadCohortNumbers = DeadCohortNumbers.Value;
                    FoundVariable = true;
                }
                if (ReadOptionalVar(CohortBalance))
                {
                    parameters.CohortBalance = CohortBalance.Value;
                    FoundVariable = true;
                }

                //System.Console.WriteLine("SUCCESSFULLY ASSIGNED KEYWORD OUTPUT MODULE:" + new StringReader(CurrentLine).ReadToEnd());


                if (!FoundVariable)
                {
                    throw new System.Exception("Error in Output PnET cannot assign variable" + new StringReader(CurrentLine).ReadToEnd());
                }

            }
            //System.Console.WriteLine("READY ASSIGNING KEYWORDS OUTPUT MODULE");
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
