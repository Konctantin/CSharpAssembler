using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace SharpAssembler.OpcodeWriter
{
	/// <summary>
	/// An annotation.
	/// </summary>
	public sealed class Annotation
	{
		private string propertyName;
		/// <summary>
		/// Gets the name of the property to be set.
		/// </summary>
		/// <value>The name of the property.</value>
		public string PropertyName
		{
			get
			{
				#region Contract
				Contract.Ensures(!String.IsNullOrWhiteSpace(Contract.Result<string>()));
				#endregion
				return propertyName;
			}
		}

		private object value;
		/// <summary>
		/// Gets the new value of the property.
		/// </summary>
		/// <value>A value, which may be <see langword="null"/>.</value>
		public object Value
		{
			get { return value; }
		}

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Annotation"/> class.
		/// </summary>
		/// <param name="name">The name of the property.</param>
		/// <param name="value">The value of the property.</param>
		public Annotation(string name, object value)
		{
			#region Contract
			Contract.Requires<ArgumentException>(!String.IsNullOrWhiteSpace(name));
			#endregion

			this.propertyName = name;
			this.value = value;
		}
		#endregion

		/// <summary>
		/// Sets a property on the specified object.
		/// </summary>
		/// <param name="target">The target object.</param>
		/// <param name="name">The name of the property.</param>
		/// <param name="value">The value of the property.</param>
		public void SetOn(object target)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(target != null);
			#endregion

			var type = target.GetType();
			var property = type.GetProperty(propertyName);

			if (property == null)
				throw new ScriptException(String.Format("A property {0} could not be found on {1}.", propertyName, type));

			property.SetValue(target, ConvertToType(property.PropertyType, value), null);
		}

		/// <summary>
		/// Converts the specified value to a value that fits the target type.
		/// </summary>
		/// <param name="targetType">The target <see cref="Type"/>.</param>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted value.</returns>
		private static object ConvertToType(Type targetType, object value)
		{
			if (targetType.IsAssignableFrom(value.GetType()))
				return value;

			if (targetType.IsEnum)
			{
				if (!Enum.IsDefined(targetType, value.ToString()))
					throw new ScriptException(String.Format("The value '{0}' is not a member of {1}.", value, targetType));

				return Enum.Parse(targetType, value.ToString(), true);
			}

			if (targetType.Equals(typeof(Byte)))
				return Convert.ToByte(value);
			if (targetType.Equals(typeof(SByte)))
				return Convert.ToSByte(value);
			if (targetType.Equals(typeof(Int16)))
				return Convert.ToInt16(value);
			if (targetType.Equals(typeof(UInt16)))
				return Convert.ToUInt16(value);
			if (targetType.Equals(typeof(Int32)))
				return Convert.ToInt32(value);
			if (targetType.Equals(typeof(UInt32)))
				return Convert.ToUInt32(value);
			if (targetType.Equals(typeof(Int64)))
				return Convert.ToInt64(value);
			if (targetType.Equals(typeof(UInt64)))
				return Convert.ToUInt64(value);

			throw new ScriptException(String.Format("Could not convert from {0} to {1}.", value.GetType(), targetType));
		}

		#region Invariants
		/// <summary>
		/// Asserts the invariants of this type.
		/// </summary>
		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(!String.IsNullOrWhiteSpace(this.propertyName));
		}
		#endregion
	}
}
