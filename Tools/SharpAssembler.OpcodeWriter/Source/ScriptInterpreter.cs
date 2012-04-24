using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using System.Globalization;

namespace SharpAssembler.OpcodeWriter
{
	/// <summary>
	/// Reads a test script.
	/// </summary>
	public class ScriptInterpreter : IScriptInterpreter
	{
		#region Constructors
		///// <summary>
		///// Initializes a new instance of the <see cref="ScriptInterpreter"/> class.
		///// </summary>
		//public ScriptInterpreter()
		//    :this(new ScriptTokenizer(), new SpecFactory())
		//{ /* Nothing to do. */ }

		/// <summary>
		/// Initializes a new instance of the <see cref="ScriptInterpreter"/> class.
		/// </summary>
		/// <param name="tokenizer">The tokenizer to use.</param>
		/// <param name="factoryDispenser">The factory dispenser to use.</param>
		public ScriptInterpreter(ScriptTokenizer tokenizer, SpecFactoryDispenser factoryDispenser)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(tokenizer != null);
			Contract.Requires<ArgumentNullException>(factoryDispenser != null);
			#endregion

			this.tokenizer = tokenizer;
			this.factoryDispenser = factoryDispenser;
		}
		#endregion

		/// <inheritdoc />
		public IEnumerable<OpcodeSpec> ReadFrom(string path)
		{
			// CONTRACT: IScriptReader

			using (var stream = File.OpenRead(path))
			{
				return ReadFrom(stream);
			}
		}

		/// <inheritdoc />
		public IEnumerable<OpcodeSpec> ReadFrom(Stream stream)
		{
			// CONTRACT: IScriptReader

			// TODO: Ensure that 'stream' is not closed afterwards.
			using (var reader = new StreamReader(stream))
			{
				return ReadFrom(reader);
			}
		}

		/// <inheritdoc />
		public IEnumerable<OpcodeSpec> ReadFrom(TextReader reader)
		{
			// CONTRACT: IScriptReader

			return Read(reader.ReadToEnd());
		}

		/// <inheritdoc />
		public IEnumerable<OpcodeSpec> Read(string script)
		{
			// CONTRACT: IScriptReader

			this.opcodespecs = new List<OpcodeSpec>();
			this.reader = new ScriptReader(this.tokenizer.Tokenize(script));
			Execute();
			return opcodespecs;
		}

		/// <summary>
		/// The tokenizer to use.
		/// </summary>
		private ScriptTokenizer tokenizer;

		/// <summary>
		/// A list to which all read <see cref="OpcodeSpec"/> objects are added.
		/// </summary>
		private List<OpcodeSpec> opcodespecs;

		/// <summary>
		/// A script reader.
		/// </summary>
		private ScriptReader reader;

		/// <summary>
		/// The <see cref="SpecFactoryDispenser"/>.
		/// </summary>
		private readonly SpecFactoryDispenser factoryDispenser;

		/// <summary>
		/// The current <see cref="SpecFactory"/>.
		/// </summary>
		private SpecFactory factory;

		/// <summary>
		/// Aliases.
		/// </summary>
		private Dictionary<string, string> aliases = new Dictionary<string, string>();

		/// <summary>
		/// Annotations applied to a specific object.
		/// </summary>
		private Dictionary<object, IList<Annotation>> annotationAssociations = new Dictionary<object, IList<Annotation>>();

		/// <summary>
		/// Annotations that were read, but not yet applied to an object.
		/// </summary>
		private IList<Annotation> readAnnotations =  new List<Annotation>();

		
		/// <summary>
		/// Executes the state machine.
		/// </summary>
		private void Execute()
		{
			this.aliases = new Dictionary<string, string>();
			this.annotationAssociations = new Dictionary<object, IList<Annotation>>();
			this.readAnnotations = new List<Annotation>();

			while (this.reader.Peek() != null)
			{
				string keyword = this.reader.Peek();
				switch (keyword)
				{
					case "opcode":
						ReadOpcodeDefinition();
						break;
					case "alias":
						ReadAlias();
						break;
					case "[":
						ReadAnnotations();
						break;
					default:
						throw new ScriptException(String.Format("Unknown keyword {0}, expected one of: opcode, alias, [", keyword));
				}
			}

			ApplyAllAnnotations();
		}

		/// <summary>
		/// Reads an alias.
		/// </summary>
		private void ReadAlias()
		{
			this.reader.Read();

			string from = ReadIdentifier();

			string equalsSign = this.reader.Read();
			if (!equalsSign.Equals("="))
				throw new ScriptException(String.Format("Expected '=' operator, got {0}.", equalsSign));

			string to = ReadIdentifier();

			string terminator = this.reader.Read();
			if (!terminator.Equals(";"))
				throw new ScriptException(String.Format("Expected ';' terminator, got {0}.", terminator));

			this.aliases[from] = to;
		}

		/// <summary>
		/// Reads an identifier, and applies the alias if possible.
		/// </summary>
		/// <returns>The identifier.</returns>
		private string ReadIdentifier()
		{
			string oldIdentifier = this.reader.ReadIdentifier();
			string newIdentifier;
			if (!this.aliases.TryGetValue(oldIdentifier, out newIdentifier))
				newIdentifier = oldIdentifier;
			return newIdentifier;
		}

		/// <summary>
		/// Sets the currently used <see cref="SpecFactory"/> to the correct implementation
		/// based on the platform ID.
		/// </summary>
		/// <param name="platform">The platform ID.</param>
		private void SetFactoryForPlatform(string platform)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(platform != null);
			#endregion
			this.factory = this.factoryDispenser.Get(platform);
		}

		/// <summary>
		/// Reads an opcode definition from the file.
		/// </summary>
		private void ReadOpcodeDefinition()
		{
			this.reader.Read();

			string platform = ReadIdentifier();
			SetFactoryForPlatform(platform);
			var opcodeSpec = this.factory.CreateOpcodeSpec();
			this.opcodespecs.Add(opcodeSpec);
			opcodeSpec.Mnemonic = ReadIdentifier();

			ApplyAnnotations(opcodeSpec);

			this.reader.ReadRegionAndRepeat(ScriptReader.RegionType.Body, () =>
			{
				string keyword = this.reader.Peek();
				switch (keyword)
				{
					case "var":
						ReadOpcodeVariant(opcodeSpec);
						break;
					case "[":
						ReadAnnotations();
						break;
					default:
						throw new ScriptException(String.Format("Unknown keyword {0}, expected one of: var, [", keyword));
				}
			});
		}

		#region Annotations
		/// <summary>
		/// Reads an annotation.
		/// </summary>
		private Annotation ReadAnnotation()
		{
			#region Contract
			Contract.Ensures(Contract.Result<Annotation>() != null);
			#endregion

			string name = ReadIdentifier();

			string equalsSign = this.reader.Read();
			if (!equalsSign.Equals("="))
				throw new ScriptException(String.Format("Expected '=' operator, got {0}.", equalsSign));

			object value = ReadAnyValue();

			return new Annotation(name, value);
		}

		/// <summary>
		/// Reads annotations, if they are specified.
		/// </summary>
		private void ReadAnnotations()
		{
			if (reader.Peek() == "[")
			{
				this.reader.ReadListInRegion(ScriptReader.RegionType.SquareBrackets, ",", () =>
				{
					this.readAnnotations.Add(ReadAnnotation());
				});
			}
		}

		/// <summary>
		/// Adds the annotations to the specified object.
		/// </summary>
		/// <param name="target">The target object.</param>
		private void ApplyAnnotations(object target)
		{
			var annotations = this.readAnnotations;
			this.readAnnotations = new List<Annotation>();

			IList<Annotation> existingAnnotations;
			if (!annotationAssociations.TryGetValue(target, out existingAnnotations))
				annotationAssociations[target] = annotations;
			else
			{
				foreach (var annotation in annotations)
					existingAnnotations.Add(annotation);
			}
		}

		/// <summary>
		/// Applies all annotations.
		/// </summary>
		private void ApplyAllAnnotations()
		{
			foreach (var annotationAssociation in this.annotationAssociations)
			{
				foreach (var annotation in annotationAssociation.Value)
				{
					annotation.SetOn(annotationAssociation.Key);
				}
			}
		}
		#endregion

		/// <summary>
		/// Reads an opcode variant.
		/// </summary>
		private void ReadOpcodeVariant(OpcodeSpec opcodeSpec)
		{
			string keyword = this.reader.Read();
			if (!keyword.Equals("var"))
				throw new ScriptException(String.Format("Unknown keyword {0}, expected one of: var", keyword));

			var opcodeVariantSpec = this.factory.CreateOpcodeVariantSpec();
			opcodeSpec.Variants.Add(opcodeVariantSpec);

			string opcodeBytesStr = ReadIdentifier();
			opcodeVariantSpec.OpcodeBytes = ReadByteArray(opcodeBytesStr);

			ApplyAnnotations(opcodeVariantSpec);

			this.reader.ReadListInRegion(ScriptReader.RegionType.Parentheses, ",", () =>
			{
				ReadAnnotations();

				string operandType = ReadIdentifier();
				string operandName = ReadIdentifier();
				object defaultValue = null;

				if (this.reader.Peek().Equals("="))
				{
					this.reader.Read();
					defaultValue = ReadAnyValue();
				}

				var operandSpec = this.factory.CreateOperandSpec(operandType, defaultValue);
				opcodeVariantSpec.Operands.Add(operandSpec);
				operandSpec.Name = operandName;

				ApplyAnnotations(operandSpec);
			});

			string terminator = this.reader.Read();
			if (!terminator.Equals(";"))
				throw new ScriptException(String.Format("Expected ';' terminator, got {0}.", terminator));
		}

		/// <summary>
		/// Reads an array of bytes.
		/// </summary>
		/// <param name="str">The input string.</param>
		/// <returns>The array of bytes.</returns>
		private static byte[] ReadByteArray(string str)
		{
			var byteStrings = str.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
			byte[] bytes = new byte[byteStrings.Length];
			int index = 0;
			foreach (var s in byteStrings)
			{
				int value;
				if (!Int32.TryParse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
				{
					throw new ScriptException("Malformed opcode variant identifier. Expected a space-separated list " +
						"of hexadecimal byte values.");
				}
				bytes[index++] = (byte)value;
			}
			return bytes;
		}

		/// <summary>
		/// Reads a value of any type.
		/// </summary>
		/// <returns>The read value.</returns>
		private object ReadAnyValue()
		{
			string keyword = this.reader.Peek();
			switch(keyword)
			{
				case "true": this.reader.Read(); return true;
				case "false": this.reader.Read(); return false;
			}

			if (this.reader.PeekString() != null)
				return this.reader.ReadString();

			if (this.reader.PeekInteger().HasValue)
				return this.reader.ReadInteger();

			if (this.reader.PeekIdentifier() != null)
				return (Identifier)ReadIdentifier();

			throw new ScriptException("Could not determine type of value.");
		}
	}
}
