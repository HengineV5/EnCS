using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Text;

namespace EnCS.Generator.Tests
{
    public class SimpleAnalyzerConfigOptionsProvider : AnalyzerConfigOptionsProvider
    {
        private readonly AnalyzerConfigOptions _globalOptions;

        public SimpleAnalyzerConfigOptionsProvider(IDictionary<string, string> options)
        {
            _globalOptions = new SimpleAnalyzerConfigOptions(options);
        }

        public override AnalyzerConfigOptions GlobalOptions => _globalOptions;

        public override AnalyzerConfigOptions GetOptions(Microsoft.CodeAnalysis.SyntaxTree tree)
            => _globalOptions;

        public override AnalyzerConfigOptions GetOptions(Microsoft.CodeAnalysis.AdditionalText textFile)
            => _globalOptions;

        private class SimpleAnalyzerConfigOptions : AnalyzerConfigOptions
        {
            private readonly ImmutableDictionary<string, string> _options;

            public SimpleAnalyzerConfigOptions(IDictionary<string, string> options)
            {
                _options = options.ToImmutableDictionary();
            }

            public override bool TryGetValue(string key, out string value)
                => _options.TryGetValue(key, out value);
        }
    }

    public static class TestHelper
	{
		public static Task Verify(params string[] source)
		{
			List<SyntaxTree> trees = new List<SyntaxTree>();
			foreach (var item in source)
			{
				trees.Add(CSharpSyntaxTree.ParseText(item));
			}

			var r = new MetadataReference[1];
			r[0] = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);

			CSharpCompilation compilation = CSharpCompilation.Create("Tests", trees, references: r);
			var diag = compilation.GetDiagnostics();

            var analyzerProvider = new SimpleAnalyzerConfigOptionsProvider(new Dictionary<string, string>
			{
				{"build_property.TemplateGenerator_LogFilePath", "True"},
				{"build_property.TemplateGenerator_LogLevel", "Trace"}
			});

            GeneratorDriver driver = CSharpGeneratorDriver.Create(new TemplateGenerator());
			driver.WithUpdatedAnalyzerConfigOptions(analyzerProvider);
            driver = driver.RunGenerators(compilation);
			
			return Verifier.Verify(driver);
		}
	}

	[UsesVerify]
	public class EcsTests
	{
		[Fact]
		public Task ComponentTest()
		{
			var fixedArraySource = File.ReadAllText("Files/Common/FixedArraySource.cs");
			var interfaceSource = File.ReadAllText("Files/Common/InterfaceSource.cs");
			var attribSource = File.ReadAllText("Files/Common/AttributeSource.cs");

			var source3 = File.ReadAllText("Files/TestFiles/TestFile.cs");

			return TestHelper.Verify(source3, attribSource, interfaceSource, fixedArraySource);
		}
	}
}