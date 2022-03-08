using System;
using System.Collections.Generic;
using PrepTime.Common.Models;
using PrepTime.Common.Models.Factories;

namespace PrepTime.Common
{
    public interface IController
    {
        TimeSpan CalculateTotalTaskTime();
        IDish DishAdd(string name);
        void DishChangeName(int id, string newName);
        bool DishDelete(int id);
        int[] DishGetSortedTasks(int id);
        bool DishNameExists(string name);
        IEntity[] FindEntities(int[] ids);
        IEntity? FindEntity(int id);
        DateTime GetBeginTime();
        void SetEndTime(DateTime time);
        void SwapTasks(ITask task1, ITask task2);
        ITask TaskAdd(int dishID, string description, TimeSpan interval);
        void TaskChangeDescription(int id, string newDescription);
        void TaskChangeInterval(int id, TimeSpan newInterval);
        void TaskUpdateDependencies(int dependentID, IEnumerable<int> dependencyIDs);
    }

    /// <summary>
    /// Provides the interaction between the UI and the data model.
    /// </summary>
    public class Controller : IController
    {
        private readonly IDishFactory _dishFactory;
        private readonly ITaskFactory _taskFactory;
        private readonly IDataModel _dataModel;

        ///<summary>
        /// Creates an object of type Controller.
        /// </summary>
        public Controller(IDishFactory dishFactory, ITaskFactory taskFactory, IDataModel dataModel)
        {
            _dishFactory = dishFactory;
            _taskFactory = taskFactory;
            _dataModel = dataModel;
        }

        /// <summary>
        /// Gets the begin time.
        /// </summary>
        /// <returns>The begin time.</returns>
        public DateTime GetBeginTime()
        {
            return _dataModel.EndTime - CalculateTotalTaskTime();
        }

        /// <summary>
        /// Sets the end time.
        /// </summary>
        /// <param name="time">The end time.</param>
        public void SetEndTime(DateTime time)
        {
            _dataModel.EndTime = time;
            CalculateTaskTime();
        }

        /// <summary>
        /// Adds a dish to prepare.
        /// </summary>
        /// <param name="name">Name of the dish.</param>
        /// <returns>The unique ID of the dish.</returns>
        public IDish DishAdd(string name)
        {
            var dish = _dishFactory.Create(name);
            _dataModel.Dishes.Add(dish);

            return dish;
        }

        /// <summary>
        /// Deletes the dish.
        /// </summary>
        /// <param name="id">The unique ID of the dish to remove.</param>
        /// <returns>True if the dish can be deleted, false otherwise.</returns>
        public bool DishDelete(int id)
        {
            var dish = DishFind(id);

            if (dish == null)
            {
                return false;
            }

            if (_dataModel.Dishes.Count == 1)
            {
                return false;
            }

            foreach (var task in dish.Tasks)
            {
                _dataModel.DependencyGraph.RemoveNode(task);
            }

            _dataModel.Dishes.Remove(dish);

            return true;
        }

        /// <summary>
        /// Changes the name of the dish with the specified ID.
        /// </summary>
        /// <param name="id">ID of the dish to change.</param>
        /// <param name="newName">New name of the dish.</param>
        public void DishChangeName(int id, string newName)
        {
            var dish = DishFind(id) ?? throw new ArgumentException(Resources.ErrorNoMatchingDishID, nameof(id));

            dish.Name = newName;
        }

        /// <summary>
        /// Checks if a dish with the same name exists.
        /// </summary>
        /// <param name="name">Name of dish to check.</param>
        /// <returns>True if there is a dish with the specified name, false otherwise.</returns>
        public bool DishNameExists(string name)
        {
            foreach (var dish in _dataModel.Dishes)
            {
                if (dish.Name == name)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Finds the dish with the specified ID.
        /// </summary>
        /// <param name="id">ID of the dish to find.</param>
        /// <returns>The Dish object with that has a matching ID or null if no match was found.</returns>
        private Dish? DishFind(int id)
        {
            foreach (var dish in _dataModel.Dishes)
            {
                if (dish.ID == id)
                {
                    return (Dish)dish;
                }
            }

            return null;
        }

        /// <summary>
        /// Adds a task to a dish.
        /// </summary>
        /// <param name="dishID">Unique ID of the dish that is getting the task added to it.</param>
        /// <param name="description">Description of the task.</param>
        /// <param name="interval">Interval of the task.</param>
        /// <returns>The unique ID of the task.</returns>
        public ITask TaskAdd(int dishID, string description, TimeSpan interval)
        {
            var dish = DishFind(dishID) ?? throw new ArgumentException(Resources.ErrorNoMatchingDishID, nameof(dishID));

            var task = _taskFactory.Create(description, interval, dish.ID);
            task.BeginOffset = task.Interval;

            if (dish.Tasks.Count > 0)
            {
                var sortedTaskIDs = DishGetSortedTasks(dish.ID);
                _dataModel.DependencyGraph.AddNode(task.ID, new[] { sortedTaskIDs[sortedTaskIDs.Length - 1] });
            }
            else
            {
                _dataModel.DependencyGraph.AddNode(task.ID);
            }

            dish.Tasks.Add(task);

            CalculateTaskTime();

            return task;
        }

        /// <summary>
        /// Changes the description of the task with the specified ID.
        /// </summary>
        /// <param name="id">ID of the task to change.</param>
        /// <param name="newName">New description of the task.</param>
        public void TaskChangeDescription(int id, string newDescription)
        {
            var task = TaskFind(id) ?? throw new ArgumentException(Resources.ErrorNoMatchingTaskID, nameof(id));

            task.Description = newDescription;
        }

        /// <summary>
        /// Changes the interval of the task with the specified ID.
        /// </summary>
        /// <param name="id">ID of the task to change.</param>
        /// <param name="newInterval">New interval of the task.</param>
        public void TaskChangeInterval(int id, TimeSpan newInterval)
        {
            var task = TaskFind(id) ?? throw new ArgumentException(Resources.ErrorNoMatchingTaskID, nameof(id));

            task.Interval = newInterval;
            task.BeginOffset = newInterval;

            CalculateTaskTime();
        }

        /// <summary>
        /// Updates the given task with the given dependencies.
        /// </summary>
        /// <param name="dependentID">ID of the dependent task.</param>
        /// <param name="dependencyIDs">IDs of the task's dependencies.</param>
        public void TaskUpdateDependencies(int dependentID, IEnumerable<int> dependencyIDs)
        {
            _ = TaskFind(dependentID) ?? throw new ArgumentException(Resources.ErrorNoMatchingTaskID, nameof(dependentID));

            var dependencies = _dataModel.DependencyGraph.GetDependencies(dependentID);
            var idsToAdd = new List<int>();
            var idsToRemove = new List<int>();

            bool found;
            foreach (var entityID in dependencies)
            {
                found = false;

                foreach (var dependencyID in dependencyIDs)
                {
                    if (entityID == dependencyID)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    idsToRemove.Add(entityID);
                }
            }

            foreach (var dependencyID in dependencyIDs)
            {
                found = false;

                foreach (var entityID in dependencies)
                {
                    if (dependencyID == entityID)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    idsToAdd.Add(dependencyID);
                }
            }

            _dataModel.DependencyGraph.AddDependencies(dependentID, idsToAdd);
            _dataModel.DependencyGraph.RemoveDependencies(dependentID, idsToRemove);

            CalculateTaskTime();
        }

        /// <summary>
        /// Finds the task with the specified ID.
        /// </summary>
        /// <param name="id">ID of the task to find.</param>
        /// <returns>The Task object with that has a matching ID or null if no match was found.</returns>
        private Task? TaskFind(int id)
        {
            foreach (var dish in _dataModel.Dishes)
            {
                foreach (var task in dish.Tasks)
                {
                    if (task.ID == id)
                    {
                        return (Task)task;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Calculates the total amount of time needed by all tasks.
        /// </summary>
        /// <returns>Total amount of time needed by all tasks.</returns>
        public TimeSpan CalculateTotalTaskTime()
        {
            var longest = TimeSpan.Zero;

            foreach (var dish in _dataModel.Dishes)
            {
                var total = TimeSpan.Zero;

                foreach (var task in dish.Tasks)
                {
                    total += task.Interval;
                }

                if (total > longest)
                {
                    longest = total;
                }
            }

            return longest;
        }

        /// <summary>
        /// Calculates the task begin time for all dishes.
        /// </summary>
        private void CalculateTaskTime()
        {
            var groups = _dataModel.DependencyGraph.GroupedTopologicalSort();
            var tasks = new List<Task>();

            foreach (var group in groups)
            {
                var total = TimeSpan.Zero;
                var entities = FindEntities(group);

                for (var i = entities.Length - 1; i >= 0; i--)
                {
                    if (entities[i] is Task task)
                    {
                        total += task.Interval;
                        task.BeginTime = _dataModel.EndTime - total;
                        tasks.Add(task);
                    }
                }
            }

            //var comparer = new Comparer<Task>();
            //tasks.Sort( comparer );

            //System.Diagnostics.Trace.WriteLine( "Before offset adjustment." );
            //foreach ( var task in tasks )
            //{
            //   System.Diagnostics.Trace.WriteLine( string.Format( "Begin: {0}; Description: {1}", task.BeginTime, task.Description ) );
            //}

            //for ( var i = tasks.Count - 1; i >= 0; i-- )
            //{
            //   for ( var j = i - 1; j >= 0; j-- )
            //   {
            //      if ( tasks[i].DishID == tasks[j].DishID )
            //      {
            //         continue;
            //      }

            //      var beginOffsetTime = tasks[j].BeginTime + tasks[j].BeginOffset;

            //      if ( beginOffsetTime > tasks[i].BeginTime )
            //      {
            //         tasks[j].BeginTime -= beginOffsetTime - tasks[i].BeginTime;
            //      }
            //   }
            //}

            //tasks.Sort( comparer );
            //System.Diagnostics.Trace.WriteLine( "\nAfter offset adjustment." );
            //foreach ( var task in tasks )
            //{
            //   System.Diagnostics.Trace.WriteLine( string.Format( "Begin: {0}; Description: {1}", task.BeginTime, task.Description ) );
            //}
        }

        /// <summary>
        /// Deletes the task.
        /// </summary>
        /// <param name="taskID">ID of the task to delete.</param>
        public void TaskDelete(int taskID)
        {
            var task = TaskFind(taskID) ?? throw new ArgumentException(Resources.ErrorNoMatchingTaskID, nameof(taskID));

            var dish = DishFind(task.DishID) ?? throw new InvalidOperationException(Resources.ErrorNoMatchingTaskID);
            dish.Tasks.Remove(task);
            _dataModel.DependencyGraph.RemoveNode(task);
        }

        /// <summary>
        /// Determines if a task can depend on the given independent ID.
        /// </summary>
        /// <param name="dependentTaskID">ID of the task.</param>
        /// <param name="independentID">ID of the independent entity.</param>
        /// <returns>True if the dependency can exist, false otherwise.</returns>
        public bool TaskCanHaveDependency(int dependentTaskID, int independentID)
        {
            var dependentTask = TaskFind(dependentTaskID) ?? throw new ArgumentException(Resources.ErrorNoMatchingTaskID, nameof(dependentTaskID));

            var independentTask = TaskFind(independentID);

            if (independentTask == null)
            {
                var independentDish = DishFind(independentID) ?? throw new ArgumentException(Resources.ErrorNoMatchingEntityID, nameof(independentID));

                if (independentDish.ID == dependentTask.DishID)
                {
                    return false;
                }
            }
            else
            {
                if (independentTask.DishID == dependentTask.DishID)
                {
                    return false;
                }

                var tempArray = new IEntity[] { independentTask };

                try
                {
                    if (!_dataModel.DependencyGraph.TryAddDependencies(dependentTaskID, tempArray))
                    {
                        return false;
                    }
                }
                finally
                {
                    _dataModel.DependencyGraph.RemoveDependencies(dependentTaskID, tempArray);
                }
            }

            return true;
        }

        /// <summary>
        /// Finds the entity that has the given ID.
        /// </summary>
        /// <param name="id">ID to find.</param>
        /// <returns>Interface to the entity that has the given ID, or null if no match was found.</returns>
        public IEntity? FindEntity(int id)
        {
            foreach (var dish in _dataModel.Dishes)
            {
                if (dish.ID == id)
                {
                    return dish;
                }

                foreach (var task in dish.Tasks)
                {
                    if (task.ID == id)
                    {
                        return task;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the entities that have the given IDs.
        /// </summary>
        /// <param name="ids">IDs to find.</param>
        /// <returns>Entities that has the given IDs.</returns>
        public IEntity[] FindEntities(int[] ids)
        {
            var entities = new List<IEntity>();

            foreach (var id in ids)
            {
                bool found = false;
                foreach (var dish in _dataModel.Dishes)
                {
                    if (dish.ID == id)
                    {
                        entities.Add(dish);
                        found = true;
                        break;
                    }

                    if (!found)
                    {
                        foreach (var task in dish.Tasks)
                        {
                            if (task.ID == id)
                            {
                                entities.Add(task);
                                found = true;
                                break;
                            }
                        }

                        if (found)
                        {
                            break;
                        }
                    }
                }
            }

            //foreach ( var dish in _dataModel.Dishes )
            //{
            //   foreach ( var id in ids )
            //   {
            //      if ( dish.ID == id )
            //      {
            //         entities.Add( dish );
            //         break;
            //      }
            //   }

            //   foreach ( var task in dish.Tasks )
            //   {
            //      foreach ( var id in ids )
            //      {
            //         if ( task.ID == id )
            //         {
            //            entities.Add( task );
            //            break;
            //         }
            //      }
            //   }
            //}

            return entities.ToArray();
        }

        /// <summary>
        /// Swaps the positions of two tasks.
        /// </summary>
        /// <param name="task1">First task to swap.</param>
        /// <param name="task2">Second task to swap.</param>
        public void SwapTasks(ITask task1, ITask task2)
        {
            if (task1.DishID != task2.DishID)
            {
                throw new Exception(Resources.ErrorTasksNotFromSameDish);
            }

            var task1Dependencies = _dataModel.DependencyGraph.GetDependencies(task1.ID);
            var task2Dependencies = _dataModel.DependencyGraph.GetDependencies(task2.ID);

            var task1Dependents = _dataModel.DependencyGraph.GetDependents(task1.ID);
            var task2Dependents = _dataModel.DependencyGraph.GetDependents(task2.ID);

            _dataModel.DependencyGraph.RemoveDependencies(task1.ID, task1Dependencies);
            _dataModel.DependencyGraph.RemoveDependencies(task2.ID, task2Dependencies);

            for (var i = 0; i < task1Dependencies.Length; i++)
            {
                if (task1Dependencies[i] == task2.ID)
                {
                    task1Dependencies[i] = task1.ID;
                }
            }

            for (var i = 0; i < task2Dependencies.Length; i++)
            {
                if (task2Dependencies[i] == task1.ID)
                {
                    task2Dependencies[i] = task2.ID;
                }
            }

            var task1DishDependent = -1;
            var task2DishDependent = -1;

            foreach (var dependent in task1Dependents)
            {
                var task = TaskFind(dependent) ?? throw new InvalidOperationException(Resources.ErrorNoMatchingTaskID);

                if (task.DishID == task1.DishID)
                {
                    _dataModel.DependencyGraph.RemoveDependencies(dependent, new[] { task1.ID });
                    task1DishDependent = dependent;
                    break;
                }
            }

            foreach (var dependent in task2Dependents)
            {
                var task = TaskFind(dependent) ?? throw new InvalidOperationException(Resources.ErrorNoMatchingTaskID);

                if (task.DishID == task2.DishID)
                {
                    _dataModel.DependencyGraph.RemoveDependencies(dependent, new[] { task2.ID });
                    task2DishDependent = dependent;
                    break;
                }
            }

            TaskUpdateDependencies(task1.ID, task2Dependencies);
            TaskUpdateDependencies(task2.ID, task1Dependencies);

            if ((task1DishDependent != -1) && (task1DishDependent != task2.ID))
            {
                _dataModel.DependencyGraph.AddDependencies(task1DishDependent, new[] { task2.ID });
            }

            if ((task2DishDependent != -1) && (task2DishDependent != task1.ID))
            {
                _dataModel.DependencyGraph.AddDependencies(task2DishDependent, new[] { task1.ID });
            }

            CalculateTaskTime();
        }

        /// <summary>
        /// Gets the sorted tasks of the specified dish.
        /// </summary>
        /// <param name="id">ID of the dish.</param>
        /// <returns>Topologically sorted tasks.</returns>
        public int[] DishGetSortedTasks(int id)
        {
            var groups = _dataModel.DependencyGraph.GroupedTopologicalSort();

            foreach (var group in groups)
            {
                foreach (var taskID in group)
                {
                    var task = TaskFind(taskID) ?? throw new InvalidOperationException(Resources.ErrorNoMatchingTaskID);

                    if (task.DishID == id)
                    {
                        return group;
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            return Array.Empty<int>();
        }
    }
}