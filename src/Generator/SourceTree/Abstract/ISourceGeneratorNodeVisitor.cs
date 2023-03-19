using Generator.SourceTree.Model;

namespace Generator.SourceTree.Abstract
{
    internal interface ISourceGeneratorNodeVisitor
    {
        void VisitClass(ClassGeneratorNode classGeneratorNode);

        void VisitConstructor(ConstructorGeneratorNode constructorGeneratorNode);

        void VisitEnum(EnumGeneratorNode enumGeneratorNode);

        void VisitField(FieldGeneratorNodeBase fieldGeneratorNode);

        void VisitNamespace(NamespaceGeneratorNode namespaceGeneratorNode);

        void VisitProperty(PropertyGeneratorNode propertyGeneratorNode);

        void VisitType(TypeGeneratorNode typeGeneratorNode);
    }
}
