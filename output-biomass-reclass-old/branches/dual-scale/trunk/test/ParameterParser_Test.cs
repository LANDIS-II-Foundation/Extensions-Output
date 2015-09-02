using Edu.Wisc.Forest.Flel.Util;
using Landis.Output.Reclass;
using NUnit.Framework;
using System.Collections.Generic;

namespace Landis.Test.Output.BiomassReclass
{
    [TestFixture]
    public class ParameterParser_Test
    {
        private ParametersParser parser;
        private LineReader reader;

        //---------------------------------------------------------------------

        [TestFixtureSetUp]
        public void Init()
        {
            parser = new ParametersParser();

            Species.DatasetParser speciesParser = new Species.DatasetParser();
            reader = OpenFile("Species.txt");
            try {
                ParametersParser.SpeciesDataset = speciesParser.Parse(reader);
            }
            finally {
                reader.Close();
            }
        }

        //---------------------------------------------------------------------

        private FileLineReader OpenFile(string filename)
        {
            string path = System.IO.Path.Combine(Data.Directory, filename);
            return Landis.Data.OpenTextFile(path);
        }

        //---------------------------------------------------------------------

        private void TryParse(string filename,
                              int    errorLineNum)
        {
            try {
                reader = OpenFile(filename);
                IParameters parameters = parser.Parse(reader);
            }
            catch (System.Exception e) {
                Data.Output.WriteLine();
                Data.Output.WriteLine(e.Message.Replace(Data.Directory, Data.DirPlaceholder));
                LineReaderException lrExc = e as LineReaderException;
                if (lrExc != null)
                    Assert.AreEqual(errorLineNum, lrExc.LineNumber);
                throw;
            }
            finally {
                reader.Close();
            }
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void Empty()
        {
            TryParse("empty.txt", LineReader.EndOfInput);
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void LandisData_WrongValue()
        {
            TryParse("LandisData-WrongValue.txt", 3);
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void Timestep_Missing()
        {
            TryParse("Timestep-Missing.txt", LineReader.EndOfInput);
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void Timestep_Negative()
        {
            TryParse("Timestep-Negative.txt", 6);
        }

        //---------------------------------------------------------------------


        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void Maps_None()
        {
            TryParse("Maps-None.txt", LineReader.EndOfInput);
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void Maps_1stLineNoDelimiter()
        {
            TryParse("Maps-1stLineNoDelimiter.txt", 19);
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void Maps_DelimiterInName()
        {
            TryParse("Maps-DelimiterInName.txt", 19);
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void Maps_ForestType_Missing()
        {
            TryParse("Maps-ForestType-Missing.txt", 19);
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void Maps_ForestType_Repeated()
        {
            TryParse("Maps-ForestType-Repeated.txt", 21);
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void Maps_Species_Missing()
        {
            TryParse("Maps-Species-Missing.txt", 19);
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void Maps_Species_Unknown()
        {
            TryParse("Maps-Species-Unknown.txt", 19);
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void Maps_Species_Repeated()
        {
            TryParse("Maps-Species-Repeated.txt", 19);
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void Maps_Species_JustMinus()
        {
            TryParse("Maps-Species-JustMinus.txt", 19);
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void Maps_Repeated()
        {
            TryParse("Maps-Repeated.txt", 26);
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void MapFileNames_Missing()
        {
            TryParse("MapFileNames-Missing.txt", LineReader.EndOfInput);
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void MapFileNames_NoMapName()
        {
            TryParse("MapFileNames-NoMapName.txt", 27);
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void MapFileNames_NoTimestep()
        {
            TryParse("MapFileNames-NoTimestep.txt", 27);
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void MapFileNames_UnknownVar()
        {
            TryParse("MapFileNames-UnknownVar.txt", 27);
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void ExtraAfterLastParm()
        {
            TryParse("ExtraAfterLastParm.txt", 29);
        }

        //---------------------------------------------------------------------

        [Test]
        public void Reclass()
        {
            reader = OpenFile("Reclass.txt");
            IParameters parameters = parser.Parse(reader);

            Assert.AreEqual(20, parameters.Timestep);

            int speciesCount = ParametersParser.SpeciesDataset.Count;

            Assert.AreEqual(speciesCount, parameters.ReclassCoefficients.Length);
            foreach (Species.ISpecies species in ParametersParser.SpeciesDataset) {
                double expectedCoeff = 0.0;
                if (species.Name == "abiebals")
                    expectedCoeff = 0.5;
                else if (species.Name == "tiliamer")
                    expectedCoeff = 0.1;
                Assert.AreEqual(expectedCoeff,
                                parameters.ReclassCoefficients[species.Index]);
            }

            Assert.AreEqual(2, parameters.ReclassMaps.Length);
            foreach (IMapDefinition mapDefn in parameters.ReclassMaps) {
                if (mapDefn.Name == "reclass1") {
                    foreach (IForestType forestType in mapDefn.ForestTypes) {
                        if (forestType.Name == "MapleHardwood")
                            CheckForestType(forestType,
                                            new string[] {"acersacc", "betualle"},
                                            new string[] {"pinubank"});
                        else if (forestType.Name == "Other")
                            CheckForestType(forestType,
                                            new string[] {"pinustro", "poputrem", "betupapy"},
                                            null);
                        else
                            Assert.Fail("Forest type is not MapleHardwood or Other");
                    }
                }
                else if (mapDefn.Name == "reclass2") {
                    foreach (IForestType forestType in mapDefn.ForestTypes) {
                        if (forestType.Name == "MapleHardwood")
                            CheckForestType(forestType,
                                            new string[] {"acersacc", "betualle", "tiliamer", "fraxamer"},
                                            null);
                        else if (forestType.Name == "EarlySucc")
                            CheckForestType(forestType,
                                            new string[] {"poputrem", "betupapy", "pinubank"},
                                            null);
                        else if (forestType.Name == "Other")
                            CheckForestType(forestType,
                                            new string[] {"pinustro"},
                                            null);
                        else
                            Assert.Fail("Forest type is not MapleHardwood, EarlySucc or Other");
                    }
                }
                else
                    Assert.Fail("Name of map definition is not reclass1 or reclass2");
            }
        }

        //---------------------------------------------------------------------

        private void CheckForestType(IForestType forestType,
                                     string[]    speciesWithMultiplier1,
                                     string[]    speciesWithMultiplierNeg1)
        {
            foreach (Species.ISpecies species in ParametersParser.SpeciesDataset) {
                int expectedMultiplier = 0;
                if (InArray(speciesWithMultiplier1, species.Name))
                    expectedMultiplier = 1;
                else if (InArray(speciesWithMultiplierNeg1, species.Name))
                    expectedMultiplier = -1;
                Assert.AreEqual(expectedMultiplier,
                                forestType[species.Index]);
            }
        }

        //---------------------------------------------------------------------

        private bool InArray(string[] values,
                             string   value)
        {
            if (values == null)
                return false;
            foreach (string item in values)
                if (item == value)
                    return true;
            return false;
        }
    }
}
