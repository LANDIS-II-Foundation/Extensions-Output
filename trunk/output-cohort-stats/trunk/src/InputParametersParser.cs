//  Copyright 2008-2010  Portland State University, Conservation Biology Institute
//  Authors:  Brendan C. Ward, Robert M. Scheller

using Edu.Wisc.Forest.Flel.Util;
using Landis.Core;
using Landis.SpatialModeling;
using System.Collections.Generic;

namespace Landis.Extension.Output.CohortStats
{

    /// <summary>
    /// A parser that reads the plug-in's parameters from text input.
    /// </summary>
    public class InputParameterParser
        : TextParser<IInputParameters>
    {
        //---------------------------------------------------------------------

        public InputParameterParser()
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
            parameters.AgeStatSpecies = new Dictionary<string, IEnumerable<ISpecies>>();
            parameters.SiteAgeStats = new List<string>();
            parameters.SiteSppStats = new List<string>();
            Dictionary<string,int> statLines = new Dictionary<string, int>();
            InputVar<string> sectionNameVar = new InputVar<string>("Section Names");
            InputVar<string> statLineVar = new InputVar<string>("");

            int lineNumber = 0;
            StringReader currentLine; 
            string word="";
            string stat = "";

            List<string> sectionNames = new List<string>();
            sectionNames.Add("SpeciesAgeStats");
            sectionNames.Add("SiteAgeStats");
            sectionNames.Add("SiteSpeciesStats");

            List<string> spp_age_statsNames = new List<string>();
            spp_age_statsNames.Add("MAX");
            spp_age_statsNames.Add("MIN");
            spp_age_statsNames.Add("MED");
            spp_age_statsNames.Add("AVG");
            spp_age_statsNames.Add("SD");

            List<string> site_age_statsNames = new List<string>();
            site_age_statsNames.Add("MAX");
            site_age_statsNames.Add("MIN");
            site_age_statsNames.Add("MED");
            site_age_statsNames.Add("AVG");
            site_age_statsNames.Add("SD");
            site_age_statsNames.Add("RICH");
            site_age_statsNames.Add("EVEN");

            List<string> site_spp_statsNames = new List<string>();
            site_spp_statsNames.Add("RICH");

            InputVar<int> timestep = new InputVar<int>("Timestep");
            ReadVar(timestep);
            parameters.Timestep = timestep.Value;

            SkipBlankLines();
            while (!AtEndOfInput)
            {
                currentLine = new StringReader(CurrentLine);
                TextReader.SkipWhitespace(currentLine);
                word = TextReader.ReadWord(currentLine);

                if (word != "")
                {
                    if (!sectionNames.Contains(word))
                        throw new InputVariableException(sectionNameVar, "Found the name {0} but expected one of {1}", word, ListToString(sectionNames));

                    GetNextLine();
                    SkipBlankLines();

                    if (word == "SpeciesAgeStats")
                    {
                        ISpecies species;
                        List<ISpecies> selectedSpecies;
                        statLines = new Dictionary<string, int>();


                        InputVar<string> mapNames = new InputVar<string>("MapNames");
                        ReadVar(mapNames);
                        parameters.SppAgeStats_MapNames = mapNames.Value;

                        while (!AtEndOfInput && !sectionNames.Contains(CurrentName))
                        {
                            selectedSpecies = new List<ISpecies>();

                            currentLine = new StringReader(CurrentLine);
                            TextReader.SkipWhitespace(currentLine);
                            stat = TextReader.ReadWord(currentLine);

                            if (!spp_age_statsNames.Contains(stat))
                                throw new InputVariableException(statLineVar, "Found the name {0} but expected one of {1}", stat, ListToString(spp_age_statsNames));

                            if (statLines.TryGetValue(stat, out lineNumber))
                            {
                                throw new InputVariableException(statLineVar, "The statistic {0} was previously used on line {1}",
                                                              stat, lineNumber);
                            }
                            statLines[stat] = LineNumber;
                            TextReader.SkipWhitespace(currentLine);
                            word = TextReader.ReadWord(currentLine);
                            if (word == "all" || word == "All")
                            {
                                parameters.AgeStatSpecies[word] = PlugIn.ModelCore.Species;
                                CheckNoDataAfter("the " + word + " parameter");
                            }
                            else if (word!="")
                            {
                                List<string> species_added = new List<string>();
                                species = GetSpecies(word);
                                selectedSpecies.Add(species);
                                species_added.Add(word);
                                TextReader.SkipWhitespace(currentLine);

                                while (currentLine.Peek() != -1)
                                {
                                    word = TextReader.ReadWord(currentLine);
                                    if (word == "all" || word == "All")
                                        throw new InputVariableException(statLineVar, "Line {0}: Cannot use the keyword {1} with a list of species",
                                                                      LineNumber, word);
                                    if (species_added.Contains(word))
                                        throw new InputVariableException(statLineVar, "The species {0} was previously used on line {1}",
                                                                      word, LineNumber);
                                    species = GetSpecies(word);
                                    selectedSpecies.Add(species);
                                    species_added.Add(word);
                                    TextReader.SkipWhitespace(currentLine);
                                }
                                parameters.AgeStatSpecies[stat] = selectedSpecies;
                            }
                            GetNextLine();
                            SkipBlankLines();
                            if (selectedSpecies.Count == 0)
                            {
                                throw new InputVariableException(statLineVar, "No species were entered for the statistic {0} on line {1}",
                                                                      stat, LineNumber);
                            }
                        }
                    }
                    else if (word == "SiteAgeStats")
                    {
                        statLines = new Dictionary<string, int>();
                        List<string> selectedAgeStats = new List<string>();

                        InputVar<string> mapNames = new InputVar<string>("MapNames");
                        ReadVar(mapNames);
                        parameters.SiteAgeStats_MapNames = mapNames.Value;

                        while (!AtEndOfInput && !sectionNames.Contains(CurrentName))
                        {
                            currentLine = new StringReader(CurrentLine);
                            TextReader.SkipWhitespace(currentLine);
                            stat = TextReader.ReadWord(currentLine);

                            if (!site_age_statsNames.Contains(stat))
                                throw new InputVariableException(statLineVar, "Found the name {0} but expected one of {1}", stat, ListToString(site_age_statsNames));

                            if (statLines.TryGetValue(stat, out lineNumber))
                            {
                                throw new InputVariableException(statLineVar, "The statistic {0} was previously used on line {1}",
                                                              stat, lineNumber);
                            }
                            statLines[stat] = LineNumber;
                            selectedAgeStats.Add(stat);

                            GetNextLine();
                            SkipBlankLines();
                        }
                        parameters.SiteAgeStats = selectedAgeStats;
                    }
                    else if (word == "SiteSpeciesStats")
                    {
                        statLines = new Dictionary<string, int>();
                        List<string> selectedSiteSppStats = new List<string>();

                        InputVar<string> mapNames = new InputVar<string>("MapNames");
                        ReadVar(mapNames);
                        parameters.SiteSppStats_MapNames = mapNames.Value;

                        while (!AtEndOfInput && !sectionNames.Contains(CurrentName))
                        {
                            currentLine = new StringReader(CurrentLine);
                            TextReader.SkipWhitespace(currentLine);
                            stat = TextReader.ReadWord(currentLine);

                            if (!site_spp_statsNames.Contains(stat))
                                throw new InputVariableException(statLineVar, "Found the name {0} but expected one of {1}", stat, ListToString(site_spp_statsNames));

                            if (statLines.TryGetValue(stat, out lineNumber))
                            {
                                throw new InputVariableException(statLineVar, "The statistic {0} was previously used on line {1}",
                                                              stat, lineNumber);
                            }
                            statLines[stat] = LineNumber;
                            selectedSiteSppStats.Add(stat);

                            GetNextLine();
                            SkipBlankLines();
                        }
                        parameters.SiteSppStats = selectedSiteSppStats;
                    }
                }
                else
                {
                    GetNextLine();
                    SkipBlankLines();
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
                                              "{0} is not a species name.",
                                              name.String);
            return species;
        }

        //---------------------------------------------------------------------

        protected ISpecies GetSpecies(string name)
        {
            ISpecies species = PlugIn.ModelCore.Species[name];
            if (species == null)
                throw new InputValueException(name,
                                              "{0} is not a species name.",
                                              name);
            return species;
        }

        //---------------------------------------------------------------------

        private string ListToString(List<string> curList)
        {
            string enum_string = "";
            foreach (string entry in curList)
            {
                enum_string += entry + " ";
            }
            return enum_string;
        }

        //---------------------------------------------------------------------

        private void SkipBlankLines()
        {
            while (!AtEndOfInput)
            {
                StringReader currentLine = new StringReader(CurrentLine);
                TextReader.SkipWhitespace(currentLine);
                string word = TextReader.ReadWord(currentLine);
                if (word != "")
                    return;
                GetNextLine();
            }
        }
    }
}
