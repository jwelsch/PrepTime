using PrepTime.Common.Util;

namespace PrepTime.Common.Models.Factories
{
    public interface IDishFactory
    {
        IDish Create(int id, string name);
        IDish Create(string name);
    }

    public class DishFactory : IDishFactory
    {
        private readonly ISequentialNumberGenerator _sequentialNumberGenerator;

        public DishFactory(ISequentialNumberGenerator sequentialNumberGenerator)
        {
            _sequentialNumberGenerator = sequentialNumberGenerator;
        }

        public IDish Create(int id, string name)
        {
            return new Dish(id, name);
        }

        public IDish Create(string name)
        {
            var id = _sequentialNumberGenerator.Next(typeof(Dish));

            return new Dish(id, name);
        }
    }
}
