using PrepTime.Common.Graph;
using System;

namespace PrepTime.Common.Models
{
    public interface IDataModel
    {
        Version Version { get; }
        DateTime BeginTime { get; set; }
        DateTime EndTime { get; set; }
        IPrepGraph DependencyGraph { get; }
        IDishCollection Dishes { get; }
    }

    /// <summary>
    /// Data model.
    /// </summary>
    public class DataModel
    {
        /// <summary>
        /// Gets the version of the data model.
        /// </summary>
        public Version Version
        {
            get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version; }
        }

        /// <summary>
        /// Gets the begin time.
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// Gets the end time.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets the dependency graph.
        /// </summary>
        public IPrepGraph DependencyGraph { get; } = new PrepGraph();

        /// <summary>
        /// Gets the collection dishes.
        /// </summary>
        public IDishCollection Dishes { get; } = new DishCollection();

        /// <summary>
        /// Creates an object of type DataModel.
        /// </summary>
        public DataModel()
        {
            EndTime = DateTime.Now;
            BeginTime = EndTime;
        }

        public void DebugDump()
        {
            System.Diagnostics.Trace.WriteLine(string.Format("BeginTime: {0}", BeginTime.ToString("hh:mm tt MMM dd, yyyy")));
            System.Diagnostics.Trace.WriteLine(string.Format("EndTime: {0}", EndTime.ToString("hh:mm tt MMM dd, yyyy")));

            System.Diagnostics.Trace.WriteLine(string.Format("Dishes"));
            foreach (var dish in Dishes)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("  ID: {0}", dish.ID));
                System.Diagnostics.Trace.WriteLine(string.Format("  Name: {0}", dish.Name));
                System.Diagnostics.Trace.WriteLine(string.Format("  Tasks"));

                foreach (var task in dish.Tasks)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("    ID: {0}", task.ID));
                    System.Diagnostics.Trace.WriteLine(string.Format("    Description: {0}", task.Description));
                    System.Diagnostics.Trace.WriteLine(string.Format("    Interval: {0}", task.Interval));
                    System.Diagnostics.Trace.WriteLine(string.Format("    BeginTime: {0}", task.BeginTime));
                }
            }

            System.Diagnostics.Trace.WriteLine(string.Format("Dependencies"));
            foreach (var node in DependencyGraph.Nodes)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("  ID: {0}", node.Value.ID));

                foreach (var dependency in node.Value.Dependencies)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("    ID: {0}", dependency.Value.ID));
                }
            }
        }
    }
}