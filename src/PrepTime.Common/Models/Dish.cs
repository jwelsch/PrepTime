namespace PrepTime.Common.Models
{
    /// <summary>
    /// Interface that represents a dish to prepare.
    /// </summary>
    public interface IDish : IEntity
    {
        /// <summary>
        /// Gets or sets the name of the dish.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets the collection of tasks.
        /// </summary>
        ITaskCollection Tasks { get; }
    }

    /// <summary>
    /// Represents a dish to prepare.
    /// </summary>
    public class Dish : Entity, IDish
    {
        /// <summary>
        /// Gets the name of the dish.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the collection of tasks.
        /// </summary>
        public ITaskCollection Tasks { get; } = new TaskCollection();

        ///<summary>
        /// Creates an object of type Dish.
        /// </summary>
        /// <param name="id">ID of the dish.</param>
        /// <param name="name">The name of the dish.</param>
        public Dish(int id, string name)
           : base(id)
        {
            Name = name;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
