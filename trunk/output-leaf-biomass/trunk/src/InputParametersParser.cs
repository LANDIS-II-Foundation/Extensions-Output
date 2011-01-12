//  Copyright 2005-2010 Portland State University, University of Wisconsin
//  Authors:  Robert M. Scheller, James B. Domingo

using Landis.Core;
using Edu.Wisc.Forest.Flel.Util;
using Landis.Library.LeafBiomassCohorts;
using Landis.SpatialModeling;
using System.Collections.Generic;


namespace Landis.Extension.Output.LeafBiomass
{
    /// <summary>
    /// A parser that reads the plug-in's parameters from text input.
    /// </summary>
    public class InputParametersParser
        : TextParser<IInputParameters>
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

            InputVar<bool> makeMaps = new InputVar<bool>("MakeMaps");
            ReadVar(makeMaps);
            parameters.MakeMaps = makeMaps.Value;

            InputVar<bool> makeTable = new InputVar<bool>("MakeTable");
            ReadVar(makeTable);
            parameters.MakeTable = makeTable.Value;

            //  Check for optional pair of parameters for species:
            //      Species
            //      MapNames
            InputVar<string> speciesName = new InputVar<string>("Species");
            InputVar<string> mapNames = new InputVar<string>("MapNames");
            //const string DeadPoolsName = "DeadPools";
            int lineNumber = LineNumber;
            bool speciesParmPresent = ReadOptionalVar(speciesName);
            if (speciesParmPresent) {
                if (speciesName.Value.Actual == "all") {
                    parameters.SelectedSpecies = PlugIn.ModelCore.Species;
                }
                else {
                    ISpecies species = GetSpecies(speciesName.Value);
                    List<ISpecies> selectedSpecies = new List<ISpecies>(); 
                    selectedSpecies.Add(species);
                    parameters.SelectedSpecies = selectedSpecies;
    
                    Dictionary<string, int> lineNumbers = new Dictionary<string, int>();
                    lineNumbers[species.Name] = lineNumber;
    
                    while (! AtEndOfInput && CurrentName != mapNames.Name) {
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

                ReadVar(mapNames);
                parameters.SpeciesMaps = mapNames.Value;
            }

            //  Check for optional pair of parameters for dead pools:
            //      DeadPools
            //      MapNames
            //  Only optional if species parameters are present.
            
            /*InputVar<SelectedDeadPools> deadPools = new InputVar<SelectedDeadPools>(DeadPoolsName,
                                                                                    SelectedDeadPoolsUtil.Parse);
            bool deadPoolsPresent;
            if (speciesParmPresent)
                deadPoolsPresent = ReadOptionalVar(deadPools);
            else {
                ReadVar(deadPools);
                deadPoolsPresent = true;
            }
            if (deadPoolsPresent) {
                parameters.SelectedPools = deadPools.Value;
                ReadVar(mapNames);
                parameters.PoolMapNames = mapNames.Value;

                CheckNoDataAfter("the " + mapNames.Name + " parameter");
            }*/

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
