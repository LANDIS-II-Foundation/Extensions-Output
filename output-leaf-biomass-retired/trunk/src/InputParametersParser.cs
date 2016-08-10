//  Copyright 2005-2013 Portland State University
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
        public override string LandisDataValue
        {
            get
            {
                return PlugIn.ExtensionName;
            }
        }

        protected override IInputParameters Parse()
        {
            ReadLandisDataVar();

            InputParameters parameters = new InputParameters();

            InputVar<int> timestep = new InputVar<int>("Timestep");
            ReadVar(timestep);
            parameters.Timestep = timestep.Value;

            InputVar<bool> makeMaps = new InputVar<bool>("MakeMaps");
            ReadVar(makeMaps);
            parameters.MakeMaps = makeMaps.Value;

            InputVar<bool> makeTable = new InputVar<bool>("MakeTable");
            ReadOptionalVar(makeTable);
            //parameters.MakeTable = makeTable.Value;

            //  Check for optional pair of parameters for species:
            //      Species
            //      MapNames
            InputVar<string> speciesName = new InputVar<string>("Species");
            InputVar<string> mapNames = new InputVar<string>("MapNames");
            int lineNumber = LineNumber;
            //bool speciesParmPresent = ;
            if (ReadOptionalVar(speciesName)) 
            {
                if (speciesName.Value.Actual == "all") 
                {
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
            //foreach (ISpecies species in parameters.SelectedSpecies)
            //{
            //    PlugIn.ModelCore.UI.WriteLine("   Selected species includes {0} ...", species.Name);
            //}


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
