using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
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
                .Where(file => file.EndsWith(".cs") && !file.EndsWith("Routes.cs"));
            var expectedFiles = expectedFileNames
                .Select(file => (Contents: File.ReadAllText(file), RelativePath: file));

            var test = new VerifyCS.Test
            {
                TestState =
                {
                    AdditionalReferences =
                    {
                        OriginalMetadataReference,
                    },
                },
            };
            foreach (var (contents, relativePath) in expectedFiles)
            {
                var fileName = Path.GetFileName(relativePath);
                test.TestState.GeneratedSources.Add(
                    (typeof(ContractModelGenerator), fileName, SourceText.From(contents, Encoding.UTF8, SourceHashAlgorithm.Sha256)));
            }

            var globalConfig = $"is_global = true{Environment.NewLine}build_property.GenerateDtoFromAssembly = Original{Environment.NewLine}build_property.ExcludeNamespaceFromOutputAssembly=ServiceStack";
            test.TestState.AnalyzerConfigFiles.Add(("/.globalconfig", globalConfig));

            await test.RunAsync();
        }
    }
}