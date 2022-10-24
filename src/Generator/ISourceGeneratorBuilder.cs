namespace Generator
{
    internal interface ICodeGeneratorBuilder
    {
        int CurrentIndentation { get; }

        void AddNewLine();

        void AddLineOfSource(string sourceText);
    }
}
