using Edu.Wisc.Forest.Flel.Util;
using Landis.Species;
//using Landis.Util;
using System.Collections.Generic;

namespace Landis.Output.BiomassAgeClass
{
    /// <summary>
    /// A parser that reads the plug-in's parameters from text input.
    /// </summary>
    public class InputParametersParser
        : Landis.TextParser<IInputParameters>
    {
        private IDataset speciesDataset;

        //---------------------------------------------------------------------

        public override string LandisDataValue
        {
            get {
                return "Biomass AgeClass";
            }
        }

        //---------------------------------------------------------------------

        public InputParametersParser(IDataset speciesDataset)
        {
            this.speciesDataset = speciesDataset;
        }

        //---------------------------------------------------------------------

        protected override IInputParameters Parse()
        {
            ReadLandisDataVar();

            InputParameters parameters = new InputParameters();

            InputVar<int> timestep = new InputVar<int>("Timestep");
            ReadVar(timestep);
            parameters.Timestep = timestep.Value;

            InputVar<string> mapNames = new InputVar<string>("MapNames");
            ReadVar(mapNames);
            parameters.SpeciesMapNames = mapNames.Value;

            InputVar<string> speciesNameVar = new InputVar<string>("Species");
            AgeClass ageClass = new AgeClass();
            Dictionary<string, List<AgeClass>> ageClasses = new Dictionary<string, List<AgeClass>>();
            string word = "";
            bool success  = false;
            int lineNumber = 0;
            StringReader currentLine = new StringReader(CurrentLine);

            TextReader.SkipWhitespace(currentLine);
            //first entry needs to be "Species" but ReadVar doesn't do what we want here and I can't figure out how to override
            word = TextReader.ReadWord(currentLine);
            if (word != speciesNameVar.Name)
            {
                throw new InputVariableException(speciesNameVar, "Found the name {0} but expected {1}", speciesNameVar.Name, word);
            }

            TextReader.SkipWhitespace(currentLine);
            word = TextReader.ReadWord(currentLine);
            if (word == "all" || word == "All")
            {//need to add handling of ageclasses here
                parameters.SelectedSpecies = speciesDataset;
                foreach (ISpecies species in speciesDataset)
                {
                    ageClasses.Add(species.Name, new List<AgeClass>());
                }
                if (currentLine.Peek() == -1)
                    throw new InputVariableException(speciesNameVar, "No age classes were defined on line {0}", LineNumber);
                while (currentLine.Peek() != -1)
                {
                    TextReader.SkipWhitespace(currentLine);
                    word = TextReader.ReadWord(currentLine);
                    if (word == "")
                        throw new InputVariableException(speciesNameVar, "No age classes were defined on line {0}",LineNumber);
                    ageClass = new AgeClass();
                    success = ageClass.Parse(word);
                    if (!success)
                        throw new InputVariableException(speciesNameVar, "Entry is not a valid age class: {0}", word);
                    foreach (ISpecies species in speciesDataset)
                    {
                        ageClasses[species.Name].Add(ageClass);
                    }
                }
                parameters.AgeClasses = ageClasses;
            }
            else
            {
                if(word=="")
                    throw new InputVariableException(speciesNameVar, "Missing species name on line {0}", LineNumber);

                ISpecies species = GetSpecies(word);
                List<ISpecies> selectedSpecies = new List<ISpecies>();
                selectedSpecies.Add(species);

                ageClasses.Add(species.Name, new List<AgeClass>());

                Dictionary<string, int> lineNumbers = new Dictionary<string, int>();
                lineNumbers[species.Name] = LineNumber;

                if (currentLine.Peek() == -1)
                    throw new InputVariableException(speciesNameVar, "No age classes were defined for species: {0}", species.Name);
                while (currentLine.Peek() != -1)
                {
                    TextReader.SkipWhitespace(currentLine);
                    word = TextReader.ReadWord(currentLine);
                    if (word == "")
                    {
                        if (!success)
                            throw new InputVariableException(speciesNameVar, "No age classes were defined for species: {0}", species.Name);
                        else
                            break;
                    }
                    ageClass = new AgeClass();
                    success = ageClass.Parse(word);
                    if (!success)
                        throw new InputVariableException(speciesNameVar, "Entry is not a valid age class: {0}", word);
                    ageClasses[species.Name].Add(ageClass);
                }
                GetNextLine();
                success = false;

                while (!AtEndOfInput)
                {
                    currentLine = new StringReader(CurrentLine);
                    TextReader.SkipWhitespace(currentLine);
                    word = TextReader.ReadWord(currentLine);

                    species = GetSpecies(word);
                    if (lineNumbers.TryGetValue(species.Name, out lineNumber))
                        throw new InputValueException(word,
                                                      "The species {0} was previously used on line {1}",
                                                      word, lineNumber);
                    lineNumbers[species.Name] = LineNumber;

                    selectedSpecies.Add(species);
                    //CheckNoDataAfter("the species name", currentLine);
                    ageClasses.Add(species.Name, new List<AgeClass>());
                    while (currentLine.Peek() != -1)
                    {
                        TextReader.SkipWhitespace(currentLine);
                        word = TextReader.ReadWord(currentLine);
                        if (word == "")
                        {
                            if (!success)
                                throw new InputVariableException(speciesNameVar, "No age classes were defined for species: {0}", species.Name);
                            else
                                break;
                        }
                        ageClass = new AgeClass();
                        success = ageClass.Parse(word);
                        if (!success)
                            throw new InputVariableException(speciesNameVar, "Entry is not a valid age class: {0}", word);
                        ageClasses[species.Name].Add(ageClass);
                    }
                    GetNextLine();
                }
                parameters.SelectedSpecies = selectedSpecies;
                parameters.AgeClasses = ageClasses;
            }
            return parameters; //.GetComplete();
        }

        //---------------------------------------------------------------------

        protected ISpecies GetSpecies(string name)
        {
            ISpecies species = speciesDataset[name];
            if (species == null)
                throw new InputValueException(name,
                                              "{0} is not a species name",
                                              name);
            return species;
        }
    }
}
