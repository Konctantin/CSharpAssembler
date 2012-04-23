using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpAssembler.OpcodeWriter
{
	/// <summary>
	/// Dispenses <see cref="SpecFactory"/> objects for specific platforms.
	/// </summary>
	public class SpecFactoryDispenser
	{
		/// <summary>
		/// A dictionary with factories.
		/// </summary>
		private Dictionary<string, SpecFactory> factories = new Dictionary<string, SpecFactory>();

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="SpecFactoryDispenser"/> class.
		/// </summary>
		public SpecFactoryDispenser()
		{ /* Nothing to do. */ }
		#endregion

		/// <summary>
		/// Registers a new factory with the specified platform ID.
		/// </summary>
		/// <param name="platformID">The ID of the platform.</param>
		/// <param name="factory">The factory instance.</param>
		public void Register(string platformID, SpecFactory factory)
		{
			this.factories.Add(platformID, factory);
		}

		/// <summary>
		/// Gets a factory with the specified platform ID.
		/// </summary>
		/// <param name="platformID">The platform ID.</param>
		/// <returns>The factory instance.</returns>
		public SpecFactory Get(string platformID)
		{
			return this.factories[platformID];
		}
	}
}
