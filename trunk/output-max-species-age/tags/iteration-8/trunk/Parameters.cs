namespace Landis.Output.MaxSpeciesAge
{
	/// <summary>
	/// The parameters for the plug-in.
	/// </summary>
	public class Parameters
		: IParameters
	{
		private int timestep;

		//---------------------------------------------------------------------

		public int Timestep
		{
			get {
				return timestep;
			}
		}

		//---------------------------------------------------------------------

		public Parameters(int timestep)
		{
			this.timestep = timestep;
		}
	}
}
