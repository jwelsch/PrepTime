using PrepTime.Util;
using System;

namespace PrepTime.Common.Models
{
    public interface ITaskCollection : IBaseCollection<ITask>
    {
    }

    /// <summary>
    /// A collection of Task objects.
    /// </summary>
    public class TaskCollection : BaseCollection<ITask>, ITaskCollection
    {
        /// <summary>
        /// Gets the total time span for all the tasks in the collection.
        /// </summary>
        /// <returns>Total time.</returns>
        public TimeSpan GetTotalTimeLength()
        {
            var interval = new TimeSpan();

            foreach (var task in this)
            {
                interval += task.Interval;
            }

            return interval;
        }
    }
}
