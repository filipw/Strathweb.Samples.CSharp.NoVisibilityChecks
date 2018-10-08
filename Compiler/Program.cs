using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Compiler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var metadataReferences = new[]
            {
                typeof(object).GetTypeInfo().Assembly, // System.Private.CoreLib.dll
                typeof(Enumerable).GetTypeInfo().Assembly, // System.Linq.dll
                typeof(Console).GetTypeInfo().Assembly, // System.Console.dll
                Assembly.Load(new AssemblyName("System.Runtime")), // System.Runtime.dll
                Assembly.Load(new AssemblyName("Calculator")) // Calculator.dll
            }.Select(x => MetadataReference.CreateFromFile(x.Location)).ToList();

            var compilationOptions = new CSharpCompilationOptions(OutputKind.ConsoleApplication).
                WithMetadataImportOptions(MetadataImportOptions.All);

            var topLevelBinderFlagsProperty = typeof(CSharpCompilationOptions).GetProperty("TopLevelBinderFlags", BindingFlags.Instance | BindingFlags.NonPublic);
            topLevelBinderFlagsProperty.SetValue(compilationOptions, (uint)1 << 22);

            var code = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "CrazyProgram", "Program.cs"));
            var compilation = CSharpCompilation.Create("DynamicCrazyProgram", new[] {
                CSharpSyntaxTree.ParseText(code) }, metadataReferences, compilationOptions);

            using (var ms = new MemoryStream())
            {
                var cr = compilation.Emit(ms);
                ms.Seek(0, SeekOrigin.Begin);
                var assembly = Assembly.Load(ms.ToArray());
                assembly.EntryPoint.Invoke(null, new object[] { new string[0] });
            }

            Console.ReadKey();
        }
    }
}
