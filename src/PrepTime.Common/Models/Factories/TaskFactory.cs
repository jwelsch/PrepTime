using PrepTime.Common.Util;
using System;

namespace PrepTime.Common.Models.Factories
{
    public interface ITaskFactory
    {
        ITask Create(int id, string description, TimeSpan interval, int dishID);
        ITask Create(string description, TimeSpan interval, int dishID);
    }

    public class TaskFactory : ITaskFactory
    {
        private readonly ISequentialNumberGenerator _sequentialNumberGenerator;

        public TaskFactory(ISequentialNumberGenerator sequentialNumberGenerator)
        {
            _sequentialNumberGenerator = sequentialNumberGenerator;
        }

        public ITask Create(int id, string description, TimeSpan interval, int dishID)
        {
            return new Task(id, description, interval, dishID);
        }

        public ITask Create(string description, TimeSpan interval, int dishID)
        {
            var id = _sequentialNumberGenerator.Next(typeof(Task));

            return new Task(id, description, interval, dishID);
        }
    }
}
