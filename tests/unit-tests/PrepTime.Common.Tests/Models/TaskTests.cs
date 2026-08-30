using AutoFixture;
using FluentAssertions;
using PrepTime.Common.Models;
using System;
using Xunit;

namespace PrepTime.Common.Tests.Models
{
    public class TaskTests
    {
        private readonly static Fixture AutoFixture = new();

        [Fact]
        public void When_constructor_called_then_properties_are_set()
        {
            var id = AutoFixture.Create<int>();
            var description = AutoFixture.Create<string>();
            var interval = AutoFixture.Create<TimeSpan>();
            var dishId = AutoFixture.Create<int>();

            var sut = new Task(id, description, interval, dishId);

            sut.ID.Should().Be(id);
            sut.Description.Should().Be(description);
            sut.Interval.Should().Be(interval);
            sut.DishID.Should().Be(dishId);
            sut.BeginTime.Should().Be(DateTime.MinValue);
            sut.BeginOffset.Should().Be(TimeSpan.Zero);
        }

        [Fact]
        public void When_tostring_called_then_formatted_string_is_returned()
        {
            var id = AutoFixture.Create<int>();
            var description = AutoFixture.Create<string>();
            var interval = AutoFixture.Create<TimeSpan>();
            var dishId = AutoFixture.Create<int>();

            var sut = new Task(id, description, interval, dishId);

            var result = sut.ToString();

            result.Should().Be($"{sut.BeginTime:hh:mm tt MMM dd, yyyy} [{sut.Interval}] {sut.Description}");
        }
    }
}
