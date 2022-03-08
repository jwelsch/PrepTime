using PrepTime.Util;

namespace PrepTime.Common.Models
{
    public interface IDishCollection : IBaseCollection<IDish>
    {
    }

    /// <summary>
    /// A collection of Dish objects.
    /// </summary>
    public class DishCollection : BaseCollection<IDish>, IDishCollection
    {
    }
}
