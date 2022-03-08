using System;
using PrepTime.Util;

namespace PrepTime.Common.Models
{
    /// <summary>
    /// Interface that represents a task to be done to a dish.
    /// </summary>
    public interface ITask : IEntity
    {
        /// <summary>
        /// Gets the description of the task.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets the interval of the task.
        /// </summary>
        TimeSpan Interval { get; set; }

        /// <summary>
        /// Gets the time to begin the task.
        /// </summary>
        DateTime BeginTime { get; }

        /// <summary>
        /// Gets the ID of the dish that owns the task.
        /// </summary>
        int DishID { get; }

        TimeSpan BeginOffset { get; set; }
    }

    /// <summary>
    /// Represents a task to be done to a dish.
    /// </summary>
    public class Task : Entity, ITask
    {
        /// <summary>
        /// Gets the description of the task.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets the interval of the task.
        /// </summary>
        public TimeSpan Interval { get; set; }

        /// <summary>
        /// Gets the time to begin the task.
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// Gets the ID of the dish that owns the task.
        /// </summary>
        public int DishID { get; }

        /// <summary>
        /// Gets or sets how soon the next task can begin after this one.
        /// </summary>
        public TimeSpan BeginOffset { get; set; }

        ///// <summary>
        ///// Gets or sets how soon the next task can end after this one.
        ///// </summary>
        //public TimeSpan EndOffset
        //{
        //   get;
        //   set;
        //}

        /// <summary>
        /// Creates an object of type Task.
        /// </summary>
        /// <param name="id">ID of the task.</param>
        /// <param name="description">The description of the task.</param>
        /// <param name="interval">The interval of the task.</param>
        /// <param name="dishID">ID of the dish that owns the task.</param>
        public Task(int id, string description, TimeSpan interval, int dishID)
         : base(id)
        {
            Description = description;
            Interval = interval;
            DishID = dishID;
            //BeginOffset = Interval;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("{0} [{1}] {2}", BeginTime.ToString("hh:mm tt MMM dd, yyyy"), Interval, Description);
        }
    }
}
