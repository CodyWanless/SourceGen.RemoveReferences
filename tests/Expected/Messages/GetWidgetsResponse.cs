namespace TestProject.Messages
{
    using TestProject.Types;

    public class GetWidgetsResponse
    {
        public object ResponseStatus { get; set; }
        public Widget Widget { get; set; }
    }
}
