using System;

namespace Generator.SourceTree.Abstract
{
    internal interface ICodeGeneratorBuilder
    {
        int CurrentIndentation { get; }

        void AddNewLine();

        IDisposable StartNewLine();

        void AddSource(string sourceText);

        void AddLineOfSource(string sourceText);
    }
}
