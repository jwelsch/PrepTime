using AutoFixture;
using FluentAssertions;
using PrepTime.Common.Models;
using System;
using Xunit;

namespace PrepTime.Common.Tests.Models
{
    public class TaskCollectionTests
    {
        private readonly static Fixture AutoFixture = new();

        [Fact]
        public void When_gettotaltimelength_called_with_no_items_then_zero_length_interval_is_returned()
        {
            var sut = new TaskCollection();

            var result = sut.GetTotalTimeLength();

            var timeSpan = new TimeSpan();

            result.Should().Be(timeSpan);
        }

        [Fact]
        public void When_gettotaltimelength_called_with_items_then_non_zero_length_interval_is_returned()
        {
            var dishId = AutoFixture.Create<int>();

            var id1 = AutoFixture.Create<int>();
            var id2 = AutoFixture.Create<int>();
            var id3 = AutoFixture.Create<int>();

            var description1 = AutoFixture.Create<string>();
            var description2 = AutoFixture.Create<string>();
            var description3 = AutoFixture.Create<string>();

            var timeSpan1 = AutoFixture.Create<TimeSpan>();
            var timeSpan2 = AutoFixture.Create<TimeSpan>();
            var timeSpan3 = AutoFixture.Create<TimeSpan>();

            var sut = new TaskCollection();
            sut.Add(new Task(id1, description1, timeSpan1, dishId));
            sut.Add(new Task(id2, description2, timeSpan2, dishId));
            sut.Add(new Task(id3, description3, timeSpan3, dishId));

            var result = sut.GetTotalTimeLength();

            result.Should().Be(timeSpan1 + timeSpan2 + timeSpan3);
        }
    }
}
