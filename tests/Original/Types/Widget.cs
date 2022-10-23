using Original.Enums;

namespace Original.Types
{
    public class Widget
    {
        public Guid Id { get; set; }

        public WidgetType Type { get; set; }

        public string Name { get; set; } = string.Empty;

        public IReadOnlyCollection<WidgetDetails> Details { get; set; } = Array.Empty<WidgetDetails>();
    }
}
