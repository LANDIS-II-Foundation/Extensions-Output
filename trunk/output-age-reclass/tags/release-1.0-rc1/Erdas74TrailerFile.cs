using System.IO;

namespace Landis.Output.Reclass
{
	/// <summary>
	/// Trailer file (.trl) for Erdas 7.4
	/// </summary>
	public static class Erdas74TrailerFile
	{
		public const string HeaderWord = "TRAIL74";
		public const string Class0Name = "inactive";

		private static byte[] Trail74Record;
		private static byte[] allZeroesRecord;
		private static byte[] classNameBuffer;

		private static byte[] colorValues;
		private static byte[] red;
		private static byte[] green;
		private static byte[] blue;

		//---------------------------------------------------------------------

		static Erdas74TrailerFile()
		{
			Trail74Record = new byte[128];
			for (int i = 0; i < HeaderWord.Length; i++)
				Trail74Record[i] = (byte) HeaderWord[i];

			allZeroesRecord = new byte[128];
			classNameBuffer = new byte[32];

			colorValues = new byte[256];
			red = new byte[] {
				0, 0, 100, 150, 200, 0, 0, 0, 150, 0, 150, 255, 80, 150, 255
			};
			green = new byte[] {
				0, 0, 0, 0, 0, 100, 150, 255, 0, 150, 150, 255, 80, 150, 255
			};
			blue = new byte[] {
				0, 150, 0, 0, 0, 0, 0, 0, 150, 150, 0, 0, 80, 150, 255
			};
		}

		//---------------------------------------------------------------------

		/// <summary>
		/// Writes a trailer file.
		/// </summary>
		/// <param name="gisFilePath">
		/// Path of the .gis file that the trailer file is associated with.
		/// The name of the trailer file is the path with the ".gis"
		/// extension replaced with ".trl".
		/// </param>
		/// <param name="forestTypes">
		/// List of forest types assigned to classes 1, 2, ...
		/// </param>
		public static void Write(string        gisFilePath,
		                         IForestType[] forestTypes)
		{
			string trlFilePath = Path.ChangeExtension(gisFilePath, ".trl");
			using (BinaryWriter writer = new BinaryWriter(File.Open(trlFilePath, FileMode.Create))) {
				//	Record 1
				writer.Write(Trail74Record);

				//	Records 2 to 7 (color scheme)
				//		record  color values
				//  	------	------------
				//			2	green classes 0 - 127 (doc says 0-128 but that's 129 values)
				//			3	green classes 128 - 255 (doc says 129-255 but that's only 127 values)
				//			4	red classes 0 - 127 (see note for green)
				//			5	red classes 128 - 255 (see note for green)
				//			6	blue classes 0 - 127 (see note for green)
				//			7	blue classes 128 - 255 (see note for green)

				writer.Write(PadTo256Values(green));
				writer.Write(PadTo256Values(red));
				writer.Write(PadTo256Values(blue));

				//	Record 8
				writer.Write(Trail74Record);

				//	Records 9 to 16 (histogram values)
				for (int record = 9; record <= 16; record++)
					writer.Write(allZeroesRecord);

				//	Records 17 on up (class names: 4 per record)
				writer.Write(MakeClassName(Class0Name));
				foreach (IForestType forestType in forestTypes)
					writer.Write(MakeClassName(forestType.Name));
			}
		}

		//---------------------------------------------------------------------

		public static byte[] PadTo256Values(byte[] values)
		{
			int countToCopy = (values.Length > 256) ? 256 : values.Length;
			for (int i = 0; i < countToCopy; i++)
				colorValues[i] = values[i];
			for (int i = countToCopy; i < 256; i++)
				colorValues[i] = 0;
			return colorValues;
		}

		//---------------------------------------------------------------------

		public static byte[] MakeClassName(string name)
		{
			if (name.Length > 31)
				name = name.Substring(0, 31);
			name = name + "~";
			for (int i = 0; i < name.Length; i++)
				classNameBuffer[i] = (byte) name[i];
			for (int i = name.Length; i < 32; i++)
				classNameBuffer[i] = 0;
			return classNameBuffer;
		}
	}
}
