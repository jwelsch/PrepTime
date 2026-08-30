using Autofac.Extras.NSubstitute;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using PrepTime.Common.Models;
using PrepTime.Common.Models.Factories;
using PrepTime.Common.Util;
using System;
using Xunit;

namespace PrepTime.Common.Tests.Models.Factories
{
    public class TaskFactoryTests
    {
        private readonly static Fixture AutoFixture = new();

        [Fact]
        public void When_create_with_id_called_then_task_returned_has_expected_properties()
        {
            using var autoSub = new AutoSubstitute();

            var id = AutoFixture.Create<int>();
            var description = AutoFixture.Create<string>();
            var interval = AutoFixture.Create<TimeSpan>();
            var dishId = AutoFixture.Create<int>();

            var sut = autoSub.Resolve<TaskFactory>();

            var result = sut.Create(id, description, interval, dishId);

            result.ID.Should().Be(id);
            result.Description.Should().Be(description);
            result.Interval.Should().Be(interval);
            result.DishID.Should().Be(dishId);
            result.BeginTime.Should().Be(DateTime.MinValue);
            result.BeginOffset.Should().Be(TimeSpan.Zero);
        }

        [Fact]
        public void When_create_without_id_called_then_task_returned_has_expected_properties()
        {
            using var autoSub = new AutoSubstitute();

            var id = AutoFixture.Create<int>();
            var description = AutoFixture.Create<string>();
            var interval = AutoFixture.Create<TimeSpan>();
            var dishId = AutoFixture.Create<int>();

            var numberGenerator = autoSub.Resolve<ISequentialNumberGenerator>();
            numberGenerator.Next(typeof(Task)).Returns(id);

            var sut = autoSub.Resolve<TaskFactory>();

            var result = sut.Create(description, interval, dishId);

            result.ID.Should().Be(id);
            result.Description.Should().Be(description);
            result.Interval.Should().Be(interval);
            result.DishID.Should().Be(dishId);
            result.BeginTime.Should().Be(DateTime.MinValue);
            result.BeginOffset.Should().Be(TimeSpan.Zero);
        }
    }
}
