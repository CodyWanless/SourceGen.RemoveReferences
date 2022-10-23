using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using VerifyCS = Generator.Tests.CSharpSourceGeneratorVerifier<Generator.ContractModelGenerator>;

namespace Generator.Tests
{
    public class ContractModelGeneratorTest
    {
        private static readonly MetadataReference OriginalMetadataReference = MetadataReference.CreateFromFile("Original.dll");

        [Fact]
        public async Task Test1()
        {
            const string sourceDirectory = @"..\..\..\..\..\tests\Expected";
            var expectedFileNames = Directory.GetFiles(sourceDirectory, "*", SearchOption.AllDirectories)
                .Where(filePath => !filePath.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}"))
                .Where(file => file.EndsWith(".cs"));
            var expectedFiles = expectedFileNames
                .Select(file => (Contents: File.ReadAllText(file), RelativePath: file));

            var test = new VerifyCS.Test
            {
                TestState =
                {
                    AdditionalReferences =
                    {
                        OriginalMetadataReference
                    },
                },
            };
            foreach (var (Contents, RelativePath) in expectedFiles)
            {
                var fileName = Path.GetFileName(RelativePath);
                test.TestState.GeneratedSources.Add(
                    (typeof(ContractModelGenerator), fileName, SourceText.From(Contents, Encoding.UTF8, SourceHashAlgorithm.Sha256)));
            }

            var globalConfig = $"is_global = true{Environment.NewLine}build_property.GenerateDtoFromAssembly = Original";
            test.TestState.AnalyzerConfigFiles.Add(("/.globalconfig", globalConfig));

            await test.RunAsync();
        }
    }
}