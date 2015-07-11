using Edu.Wisc.Forest.Flel.Util;
using Landis.Output.Biomass;
using NUnit.Framework;
using System.Collections.Generic;

using ISpecies = Landis.Species.ISpecies;

namespace Landis.Test.Output.Biomass
{
    [TestFixture]
    public class ParametersParser_Test
    {
        private Species.IDataset speciesDataset;
        private ParametersParser parser;
        private LineReader reader;

        //---------------------------------------------------------------------

        [TestFixtureSetUp]
        public void Init()
        {
            Species.DatasetParser speciesParser = new Species.DatasetParser();
            reader = OpenFile("Species.txt");
            try {
                speciesDataset = speciesParser.Parse(reader);
            }
            finally {
                reader.Close();
            }

            parser = new ParametersParser(speciesDataset);
        }

        //---------------------------------------------------------------------

        private FileLineReader OpenFile(string filename)
        {
            string path = Data.MakeInputPath(filename);
            return Landis.Data.OpenTextFile(path);
        }

        //---------------------------------------------------------------------

        private IParameters ParseFile(string filename)
        {
            try {
                reader = OpenFile(filename);
                return parser.Parse(reader);
            }
            finally {
                reader.Close();
            }
        }

        //---------------------------------------------------------------------

        [Test]
        public void JustSpecies()
        {
            IParameters parameters = ParseFile("JustSpecies.txt");
            Assert.AreEqual(5, parameters.Timestep);

            AssertAreEqual(ExpectedSpecies("betupapy",
                                           "acerrubr",
                                           "tiliamer",
                                           "querrubr",
                                           "tsugcana"),
                           parameters.SelectedSpecies);
            Assert.AreEqual("output/biomass/{species}-{timestep}.gis",
                            parameters.SpeciesMapNames);

            Assert.AreEqual(SelectedDeadPools.None, parameters.SelectedPools);
            Assert.IsNull(parameters.PoolMapNames);
        }

        //---------------------------------------------------------------------

        private void AssertAreEqual(string[]              expectedSpecies,
                                    IEnumerable<ISpecies> selectedSpecies)
        {
            List<string> actualSpecies = new List<string>();
            foreach (ISpecies species in selectedSpecies)
                actualSpecies.Add(species.Name);
            actualSpecies.Sort();

            Assert.AreEqual(expectedSpecies.Length, actualSpecies.Count);
            for (int i = 0; i < expectedSpecies.Length; i++)
                Assert.AreEqual(expectedSpecies[i], actualSpecies[i]);
        }

        //---------------------------------------------------------------------

        private string[] ExpectedSpecies(params string[] names)
        {
            if (names == null)
                return new string[0];
            else {
                System.Array.Sort<string>(names);
                return names;
            }
        }

        //---------------------------------------------------------------------

        [Test]
        public void AllSpecies()
        {
            IParameters parameters = ParseFile("AllSpecies.txt");
            Assert.AreEqual(5, parameters.Timestep);

            List<string> allSpecies = new List<string>();
            foreach (ISpecies species in speciesDataset)
                allSpecies.Add(species.Name);
            allSpecies.Sort();
            AssertAreEqual(allSpecies.ToArray(),
                           parameters.SelectedSpecies);
            Assert.AreEqual("output/biomass/{species}-{timestep}.gis",
                            parameters.SpeciesMapNames);

            Assert.AreEqual(SelectedDeadPools.None, parameters.SelectedPools);
            Assert.IsNull(parameters.PoolMapNames);
        }

        //---------------------------------------------------------------------

        [Test]
        public void DeadPools_Woody()
        {
            IParameters parameters = ParseFile("DeadPools-Woody.txt");
            CheckDeadPools(parameters,
                           SelectedDeadPools.Woody,
                           "output/biomass/woody-{timestep}.gis");
        }

        //---------------------------------------------------------------------

        [Test]
        public void DeadPools_Woody_PoolVar()
        {
            IParameters parameters = ParseFile("DeadPools-Woody-PoolVar.txt");
            CheckDeadPools(parameters,
                           SelectedDeadPools.Woody,
                           "output/biomass/{pool}-{timestep}.gis");
        }

        //---------------------------------------------------------------------

        private void CheckDeadPools(IParameters       parameters,
                                    SelectedDeadPools expectedPools,
                                    string            expectedMapNames)
        {
            Assert.AreEqual(10, parameters.Timestep);

            Assert.IsNull(parameters.SelectedSpecies);
            Assert.IsNull(parameters.SpeciesMapNames);

            Assert.AreEqual(expectedPools, parameters.SelectedPools);
            Assert.AreEqual(expectedMapNames, parameters.PoolMapNames);
        }

        //---------------------------------------------------------------------

        [Test]
        public void DeadPools_NonWoody()
        {
            IParameters parameters = ParseFile("DeadPools-NonWoody.txt");
            CheckDeadPools(parameters,
                           SelectedDeadPools.NonWoody,
                           "output/biomass/non-woody-{timestep}.gis");
        }

        //---------------------------------------------------------------------

        [Test]
        public void DeadPools_NonWoody_PoolVar()
        {
            IParameters parameters = ParseFile("DeadPools-NonWoody-PoolVar.txt");
            CheckDeadPools(parameters,
                           SelectedDeadPools.NonWoody,
                           "output/biomass/{pool}-{timestep}.gis");
        }

        //---------------------------------------------------------------------

        [Test]
        public void DeadPools_Both()
        {
            IParameters parameters = ParseFile("DeadPools-Both.txt");
            CheckDeadPools(parameters,
                           SelectedDeadPools.NonWoody | SelectedDeadPools.Woody,
                           "output/biomass/{pool}-{timestep}.gis");
        }

        //---------------------------------------------------------------------

        [Test]
        public void SpeciesAndPools()
        {
            IParameters parameters = ParseFile("SpeciesAndPools.txt");
            Assert.AreEqual(5, parameters.Timestep);

            AssertAreEqual(ExpectedSpecies("betupapy",
                                           "acerrubr",
                                           "tiliamer",
                                           "querrubr",
                                           "tsugcana"),
                           parameters.SelectedSpecies);
            Assert.AreEqual("output/biomass/{species}-{timestep}.gis",
                            parameters.SpeciesMapNames);

            Assert.AreEqual(SelectedDeadPools.NonWoody | SelectedDeadPools.Woody,
                            parameters.SelectedPools);
            Assert.AreEqual("output/biomass/{pool}-{timestep}.gis",
                            parameters.PoolMapNames);
        }

        //---------------------------------------------------------------------

        private void TryParse(string filename)
        {
            int? errorLineNum = Testing.FindErrorMarker(Data.MakeInputPath(filename));
            try {
                reader = OpenFile(filename);
                IParameters parameters = parser.Parse(reader);
            }
            catch (System.Exception e) {
                Data.Output.WriteLine();
                Data.Output.WriteLine(e.Message.Replace(Data.Directory, Data.DirPlaceholder));
                LineReaderException lrExc = e as LineReaderException;
                if (lrExc != null && errorLineNum.HasValue)
                    Assert.AreEqual(errorLineNum.Value, lrExc.LineNumber);
                throw;
            }
            finally {
                reader.Close();
            }
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void LandisData_WrongValue()
        {
            TryParse("LandisData-WrongValue.txt");
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void Timestep_Missing()
        {
            TryParse("Timestep-Missing.txt");
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void Timestep_Negative()
        {
            TryParse("Timestep-Negative.txt");
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void NoSpeciesOrPools()
        {
            TryParse("NoSpeciesOrPools.txt");
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void Species_MissingValue()
        {
            TryParse("Species-MissingValue.txt");
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void Species_Repeated()
        {
            TryParse("Species-Repeated.txt");
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void Species_Unknown()
        {
            TryParse("Species-Unknown.txt");
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void Species_NameAfterAll()
        {
            TryParse("Species-NameAfterAll.txt");
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void SpeciesMapNames_Missing()
        {
            TryParse("SpeciesMapNames-Missing.txt");
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void SpeciesMapNames_Missing_BeforePools()
        {
            TryParse("SpeciesMapNames-Missing-BeforePools.txt");
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void SpeciesMapNames_MissingValue()
        {
            TryParse("SpeciesMapNames-MissingValue.txt");
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void SpeciesMapNames_NoVars()
        {
            TryParse("SpeciesMapNames-NoVars.txt");
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void SpeciesMapNames_NoSpeciesVar()
        {
            TryParse("SpeciesMapNames-NoSpeciesVar.txt");
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void SpeciesMapNames_NoTimestepVar()
        {
            TryParse("SpeciesMapNames-NoTimestepVar.txt");
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void PoolMapNames_Missing()
        {
            TryParse("PoolMapNames-Missing.txt");
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void PoolMapNames_NoTimestepVar()
        {
            TryParse("PoolMapNames-NoTimestepVar.txt");
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void PoolMapNames_NoPoolVar()
        {
            TryParse("PoolMapNames-NoPoolVar.txt");
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void PoolMapNames_NoVars()
        {
            TryParse("PoolMapNames-NoVars.txt");
        }

        //---------------------------------------------------------------------

        [Test]
        [ExpectedException(typeof(LineReaderException))]
        public void ExtraParameter()
        {
            TryParse("ExtraParameter.txt");
        }
    }
}
