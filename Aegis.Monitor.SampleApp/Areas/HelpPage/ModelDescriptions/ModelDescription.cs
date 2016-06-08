using System;

namespace Aegis.Monitor.SampleApp.Areas.HelpPage.ModelDescriptions
{
    /// <summary>Describes a type model.</summary>
    public abstract class ModelDescription
    {
        public string Documentation { get; set; }

        public Type ModelType { get; set; }

        public string Name { get; set; }
    }
}