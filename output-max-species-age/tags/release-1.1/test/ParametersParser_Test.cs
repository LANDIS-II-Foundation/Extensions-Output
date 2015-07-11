using Edu.Wisc.Forest.Flel.Util;
using Landis.Output.MaxSpeciesAge;
using NUnit.Framework;
using System.Collections.Generic;

namespace Landis.Test.Output.MaxSpeciesAge
{
	[TestFixture]
	public class ParametersParser_Test
	{
		private ParametersParser parser;
		private LineReader reader;

		private const string dataDirPlaceholder = "{data folder}";

		//---------------------------------------------------------------------

		[TestFixtureSetUp]
		public void Init()
		{
			parser = new ParametersParser();

			Species.DatasetParser speciesParser = new Species.DatasetParser();
			LineReader speciesReader = OpenFile("SpeciesDataset.txt");
			try {
				ParametersParser.SpeciesDataset = speciesParser.Parse(speciesReader);
			}
			finally {
				speciesReader.Close();
			}

			Data.Output.WriteLine("{0} = \"{1}\"", dataDirPlaceholder, Data.Directory);
			Data.Output.WriteLine();
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
				Data.Output.WriteLine(e.Message.Replace(Data.Directory, dataDirPlaceholder));
				LineReaderException lrExc = e as LineReaderException;
				if (lrExc != null)
					Assert.AreEqual(errorLineNum, lrExc.LineNumber);
				Data.Output.WriteLine();
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
		public void MapNames_Missing()
		{
			TryParse("MapNames-Missing.txt", LineReader.EndOfInput);
		}

		//---------------------------------------------------------------------

		[Test]
		[ExpectedException(typeof(LineReaderException))]
		public void MapNames_NoSpecies()
		{
			TryParse("MapNames-NoSpecies.txt", 8);
		}

		//---------------------------------------------------------------------

		[Test]
		[ExpectedException(typeof(LineReaderException))]
		public void MapNames_NoTimestep()
		{
			TryParse("MapNames-NoTimestep.txt", 8);
		}

		//---------------------------------------------------------------------

		[Test]
		[ExpectedException(typeof(LineReaderException))]
		public void MapNames_UnknownVar()
		{
			TryParse("MapNames-UnknownVar.txt", 8);
		}

		//---------------------------------------------------------------------

		[Test]
		[ExpectedException(typeof(LineReaderException))]
		public void MapNames_BothVarsMissing()
		{
			TryParse("MapNames-BothVarsMissing.txt", 8);
		}

		//---------------------------------------------------------------------

		[Test]
		[ExpectedException(typeof(LineReaderException))]
		public void Species_Missing()
		{
			TryParse("Species-Missing.txt", LineReader.EndOfInput);
		}

		//---------------------------------------------------------------------

		[Test]
		[ExpectedException(typeof(LineReaderException))]
		public void Species_FirstUnknown()
		{
			TryParse("Species-FirstUnknown.txt", 10);
		}

		//---------------------------------------------------------------------

		[Test]
		[ExpectedException(typeof(LineReaderException))]
		public void Species_Unknown()
		{
			TryParse("Species-Unknown.txt", 13);
		}

		//---------------------------------------------------------------------

		[Test]
		[ExpectedException(typeof(LineReaderException))]
		public void Species_Extra()
		{
			TryParse("Species-Extra.txt", 11);
		}

		//---------------------------------------------------------------------

		[Test]
		[ExpectedException(typeof(LineReaderException))]
		public void Species_Repeated()
		{
			TryParse("Species-Repeated.txt", 13);
		}

		//---------------------------------------------------------------------

		private IParameters ParseFile(string filename)
		{
			reader = OpenFile(filename);
			IParameters parameters = parser.Parse(reader);
			reader.Close();
			return parameters;
		}

		//---------------------------------------------------------------------

		[Test]
		public void Species_All()
		{
			IParameters parameters = ParseFile("Species-All.txt");
			Assert.AreSame(ParametersParser.SpeciesDataset,
			               parameters.SelectedSpecies);
		}

		//---------------------------------------------------------------------

		[Test]
		public void Species_One()
		{
			IParameters parameters = ParseFile("Species-One.txt");
			string[] expectedSpecies = new string[] { "poputrem" };
			int i = 0;
			foreach (Species.ISpecies species in parameters.SelectedSpecies) {
				Assert.AreEqual(expectedSpecies[i], species.Name);
				i++;
			}
			Assert.AreEqual(expectedSpecies.Length, i);
		}

		//---------------------------------------------------------------------

		[Test]
		public void Species_Multiple()
		{
			IParameters parameters = ParseFile("Species-Multiple.txt");
			string[] expectedSpecies = new string[] { "abiebals",
													  "betualle",
													  "poputrem",
													  "querelli" };
			int i = 0;
			foreach (Species.ISpecies species in parameters.SelectedSpecies) {
				Assert.AreEqual(expectedSpecies[i], species.Name);
				i++;
			}
			Assert.AreEqual(expectedSpecies.Length, i);
		}
	}
}
