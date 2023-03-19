namespace TestProject.Types
{
    using System;
    using System.Collections.Generic;
    using TestProject.Enums;

    public class Widget
    {
        public Guid Id { get; set; }
        public WidgetType Type { get; set; }
        public String Name { get; set; }
        public IReadOnlyCollection<WidgetDetails> Details { get; set; }
    }
}
