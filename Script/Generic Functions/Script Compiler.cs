/*
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.Emit;

using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using UnityEngine;

namespace NagaisoraFamework
{
	public static class ScriptCompiler
	{
		public static Assembly Comp(string path)
		{
			FileStream fileStream;

			try
			{
				fileStream = new(path, FileMode.Open, FileAccess.Read);
			}
			catch
			{
				throw new FileNotFoundException(string.Format("找不到文件 {0}", path));
			}

			StreamReader sr = new StreamReader(fileStream);

			return Compile(sr.ReadToEnd());
		}

		public static Assembly Comp(Stream stream)
		{
			StreamReader sr = new StreamReader(stream);
			return Compile(sr.ReadToEnd());
		}

		public static Assembly Compile(string source)
		{
			SourceText sourceText = SourceText.From(source, Encoding.UTF8);

			//--装载所有的程序集---------------			
			List<MetadataReference> metadataReference = new List<MetadataReference>()
			{
				MetadataReference.CreateFromFile(typeof(int).Assembly.Location),
			};

			List<FileInfo> lstFiles = new List<FileInfo>();

			FileHelper.getFile("./Assemblys/", ".dll", ref lstFiles);

			foreach (FileInfo fileInfo in lstFiles)
			{
				metadataReference.Add(MetadataReference.CreateFromFile(fileInfo.FullName));
			}
			//---------------------------------

			CSharpParseOptions option = new CSharpParseOptions(LanguageVersion.CSharp9, preprocessorSymbols: new List<string>() { "Debug" });
			var tree = CSharpSyntaxTree.ParseText(sourceText, option);
			var compileOption = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
			var compilation = CSharpCompilation.Create("class1", new List<SyntaxTree> { tree }, metadataReference, compileOption);

			MemoryStream ms = new();

			EmitResult result = compilation.Emit(ms);

			IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
			diagnostic.Severity == DiagnosticSeverity.Error ||
			diagnostic.Severity == DiagnosticSeverity.Warning ||
			diagnostic.Severity == DiagnosticSeverity.Info);
			foreach (Diagnostic diagnostic in failures)
			{
				Debug.LogError($"ID:{diagnostic.Id} " +
					$"类型:{diagnostic.Severity} 等级:{diagnostic.WarningLevel} " +
					$"第{diagnostic.Location.GetLineSpan().StartLinePosition.Line}行 " +
					$"第{diagnostic.Location.GetLineSpan().StartLinePosition.Character}列 " +
					$"=> {diagnostic.GetMessage()}");
			}

			if (!result.Success)
			{
				return null;
			}

			ms.Seek(0x00, SeekOrigin.Begin);

			return Assembly.Load(ms.ToArray());
		}
	}
}
*/