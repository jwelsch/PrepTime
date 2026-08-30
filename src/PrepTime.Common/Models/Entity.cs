using PrepTime.Util;

namespace PrepTime.Common.Models
{
    /// <summary>
    /// Interface for a data model entity.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets the identifier of the Dish.
        /// </summary>
        int ID { get; }
    }

    /// <summary>
    /// Represents an object in the data model.
    /// </summary>
    public abstract class Entity : IEntity
    {
        /// <summary>
        /// Gets the identifier of the Dish.
        /// </summary>
        public int ID { get; }

        ///<summary>
        /// Creates an object of type Entity.
        /// </summary>
        /// <param name="id">ID of the entity.</param>
        public Entity(int id)
        {
            ID = id;
        }
    }

    public interface IEntityCollection : IBaseCollection<IEntity>
    {
    }

    /// <summary>
    /// A collection of Entity objects.
    /// </summary>
    public class EntityCollection : BaseCollection<IEntity>, IEntityCollection
    {
    }
}
