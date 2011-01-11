namespace Landis.Output.MaxSpeciesAge
{
	/// <summary>
	/// The parameters for the plug-in.
	/// </summary>
	public interface IParameters
	{
		/// <summary>
		/// Timestep (years)
		/// </summary>
		int Timestep
		{
			get;
		}
	}
}
