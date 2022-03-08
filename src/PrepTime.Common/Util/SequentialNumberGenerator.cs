using System.Collections.Generic;

namespace PrepTime.Common.Util
{
    public interface ISequentialNumberGenerator
    {
        int Next(object key);
        void SetNextValue(object key, int value);
    }

    public class SequentialNumberGenerator : ISequentialNumberGenerator
    {
        private readonly int _defaultStartNumber = 0;
        private readonly object _lock = new();
        private readonly Dictionary<object, int> _map = new();

        public SequentialNumberGenerator()
        {
        }

        public SequentialNumberGenerator(int defaultStartNumber)
        {
            _defaultStartNumber = defaultStartNumber;
        }

        public int Next(object key)
        {
            lock(_lock)
            {
                if (!_map.TryGetValue(key, out int value))
                {
                    value = _defaultStartNumber;
                }

                _map[key] = value + 1;

                return value;
            }
        }

        public void SetNextValue(object key, int value)
        {
            lock (_lock)
            {
                _map[key] = value;
            }
        }
    }
}
