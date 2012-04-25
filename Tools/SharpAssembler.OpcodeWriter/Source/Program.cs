using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpAssembler.OpcodeWriter.X86;
using System.IO;
using System.Diagnostics.Contracts;

namespace SharpAssembler.OpcodeWriter
{
	/// <summary>
	/// The main program entry point.
	/// </summary>
	internal class Program
	{
		/// <summary>
		/// The main entry point.
		/// </summary>
		/// <param name="args">Program arguments.</param>
		private static void Main(string[] args)
		{
			// Input folder				// ..\..\..\..\..\Input
			string codeDirectory;
			string testDirectory;
			string yasmPath;			// ..\..\..\Assembler\yasm
			IList<string> files = ReadArguments(args, out codeDirectory, out testDirectory, out yasmPath);
			if (files == null)
				return;

			var specFactoryDispenser = new SpecFactoryDispenser();
			specFactoryDispenser.Register("x86", new X86SpecFactory());
			interpreter = new ScriptInterpreter(new ScriptTokenizer(), specFactoryDispenser);
			writerDispenser = new SpecWriterDispenser();
			writerDispenser.Register("x86", new X86SpecWriter(yasmPath));

			foreach (string file in files)
			{
				Execute(file, codeDirectory, testDirectory);
			}

			Console.WriteLine("All done!");
			//Console.ReadLine();
		}

		private static IList<string> ReadArguments(string[] args, out string codeDirectory, out string testDirectory, out string yasmPath)
		{
			//Console.WriteLine("Working directory: " + Directory.GetCurrentDirectory());
			List<string> files = new List<string>();
			codeDirectory = null;
			testDirectory = null;
			yasmPath = null;

			bool recursive = false;

			for (int i = 0; i < args.Length; i++)
			{
				switch (args[i].Trim())
				{
					case "-h":
					case "--help":
						PrintHelp();
						return null;
					case "-y":
					case "--yasm":
						yasmPath = args[++i];
						string testYasmPath = yasmPath;
						if (Path.GetExtension(yasmPath).Equals(String.Empty))
							testYasmPath += ".exe";
						if (!File.Exists(testYasmPath))
						{
							Console.WriteLine("Error: YASM not found:");
							Console.WriteLine("       {0}", Path.GetFullPath(testYasmPath));
							Console.WriteLine();
							PrintHelp();
							return null;
						}
						break;
					case "-oc":
					case "--outputcode":
						codeDirectory = args[++i];
						if (!Directory.Exists(codeDirectory))
						{
							Console.WriteLine("Error: Output code directory not found:");
							Console.WriteLine("       {0}", Path.GetFullPath(codeDirectory));
							Console.WriteLine();
							PrintHelp();
							return null;
						}
						break;
					case "-ot":
					case "--outputtest":
						testDirectory = args[++i];
						if (!Directory.Exists(codeDirectory))
						{
							Console.WriteLine("Error: Output test directory not found:");
							Console.WriteLine("       {0}", Path.GetFullPath(testDirectory));
							Console.WriteLine();
							PrintHelp();
							return null;
						}
						break;
					case "-r":
					case "--r":
						recursive = true;
						break;
					default:
						if (args[i].StartsWith("-"))
						{
							Console.WriteLine("Unknown parameter '{0}'", args[i]);
							Console.WriteLine();
							PrintHelp();
							return null;
						}
						else
						{
							if (Directory.Exists(args[i]))
							{
								files.AddRange(Directory.EnumerateFiles(args[i], "*.script", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));
								recursive = false;
							}
							else if (File.Exists(args[i]))
							{
								files.Add(args[i]);
							}
							else
							{
								Console.WriteLine("Error: Neither a file nor a directory named '{0}' was found.", args[i]);
								Console.WriteLine();
								PrintHelp();
								return null;
							}
						}
						break;
				}
			}

			if (codeDirectory == null)
			{
				Console.WriteLine("Error: Code output directory not specified.");
				Console.WriteLine();
				PrintHelp();
				return null;
			}
			if (testDirectory == null)
			{
				Console.WriteLine("Error: Test output directory not specified.");
				Console.WriteLine();
				PrintHelp();
				return null;
			}
			if (yasmPath == null)
			{
				Console.WriteLine("Error: Path to YASM assembler not specified.");
				Console.WriteLine();
				PrintHelp();
				return null;
			}
			if (files.Count == 0)
			{
				Console.WriteLine("Error: No files specified.");
				Console.WriteLine();
				PrintHelp();
				return null;
			}

			return files;
		}

		/// <summary>
		/// Prints the help.
		/// </summary>
		private static void PrintHelp()
		{
			Console.WriteLine(@"
OpcodeWriter
============

Syntax: -oc OCPATH -ot OTPATH -y YASMPATH [-h] [FILEPATH | [-r] DIRPATH]+

Filepath: path to a script file to process.
Dirpath: path to a folder with script files to process.
Options:
  -r Recursively go through the directory specified in the next argument
  -h Displays this help information.
  -y Specifies the path to the YASM assembler.
  -oc Specifies the folder where the code files are written
  -ot Specifies the folder where the test files are written
");
			Console.WriteLine("Syntax: [OPTIONS] ");
		}

		/// <summary>
		/// The script interpreter to use.
		/// </summary>
		private static ScriptInterpreter interpreter;
		/// <summary>
		/// Dispenses <see cref="SpecWriter"/> objects.
		/// </summary>
		private static SpecWriterDispenser writerDispenser;

		/// <summary>
		/// Reads the specified script and executes the program.
		/// </summary>
		/// <param name="filepath">The path to the script file.</param>
		/// <param name="opcodeOutputFolder">The path to the directory in which to create the opcode code file.</param>
		/// <param name="codeOutputDirectory">The path to the directory in which to create the test code file.</param>
		/// <returns><see langword="true"/> when execution went well;
		/// otherwise, <see langword="false"/> when an error occurred.</returns>
		private static bool Execute(string filepath, string codeOutputDirectory, string testOutputDirectory)
		{
			#region Contract
			Contract.Requires<ArgumentNullException>(filepath != null);
			Contract.Requires<ArgumentNullException>(codeOutputDirectory != null);
			Contract.Requires<ArgumentNullException>(testOutputDirectory != null);
			#endregion

			Console.Write("{0,17}: ", Path.GetFileName(filepath));

			try
			{
				var opcodes = interpreter.ReadFrom(filepath);

				bool first = true;
				foreach (var opcodeSpec in opcodes)
				{
					if (!first)
						Console.Write(", ");
					Console.Write(opcodeSpec.Mnemonic.ToUpperInvariant());

					var writer = writerDispenser.Get(opcodeSpec.Platform);
					writer.Write(opcodeSpec,
						Path.Combine(codeOutputDirectory, GetCodeFilename(opcodeSpec)),
						Path.Combine(testOutputDirectory, GetTestFilename(opcodeSpec)));
					
					first = false;
				}

				Console.WriteLine(" [Done]");
			}
			catch (ScriptException se)
			{
				Console.Write(se.Message);
				return false;
			}

			return true;
		}

		/// <summary>
		/// Returns the file name of the code file for the specified opcode.
		/// </summary>
		/// <param name="opcodeSpec">The opcode specification.</param>
		/// <returns>A filename.</returns>
		private static string GetCodeFilename(OpcodeSpec opcodeSpec)
		{
			return SpecWriter.AsValidIdentifier(opcodeSpec.Name + "Opcode") + ".generated.cs";
		}

		/// <summary>
		/// Returns the file name of the test file for the specified opcode.
		/// </summary>
		/// <param name="opcodeSpec">The opcode specification.</param>
		/// <returns>A filename.</returns>
		private static string GetTestFilename(OpcodeSpec opcodeSpec)
		{
			return SpecWriter.AsValidIdentifier(opcodeSpec.Name + "Tests") + ".generated.cs";
		}
	}
}
