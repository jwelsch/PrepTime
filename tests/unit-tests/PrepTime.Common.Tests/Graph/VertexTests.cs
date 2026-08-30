using AutoFixture;
using FluentAssertions;
using FluentAssertions.Extensions;
using PrepTime.Common.Graph;
using PrepTime.Common.Tests.Fakes;
using Xunit;

namespace PrepTime.Common.Tests.Graph
{
    public class VertexTests
    {
        private readonly static Fixture _autoFixture = new();

        [Fact]
        public void When_one_argument_constructor_called_then_data_is_set_and_edges_are_empty()
        {
            var data = _autoFixture.Create<FakeData>();

            var sut = new Vertex<FakeData>(data);

            sut.Data.Should().Be(data);
            sut.Edges.Should().BeEmpty();
        }

        [Fact]
        public void When_two_argument_constructor_called_then_data_is_set_and_edges_are_set()
        {
            var data1 = _autoFixture.Create<FakeData>();
            var dataA = _autoFixture.Create<FakeData>();
            var dataB = _autoFixture.Create<FakeData>();

            var vertexA = new Vertex<FakeData>(dataA);
            var vertexB = new Vertex<FakeData>(dataB);

            var edges = new[] { vertexA, vertexB };

            var sut = new Vertex<FakeData>(data1, edges);

            sut.Data.Should().Be(data1);
            sut.Edges.Should().HaveCount(2);
            sut.Edges[0].Should().Be(vertexA);
            sut.Edges[1].Should().Be(vertexB);
        }

        [Fact]
        public void When_equals_called_with_null_then_return_false()
        {
            var data = _autoFixture.Create<FakeData>();

            var sut = new Vertex<FakeData>(data);

            var result = sut.Equals(null);

            result.Should().BeFalse();
        }

        [Fact]
        public void When_equals_called_with_wrong_type_then_return_false()
        {
            var data = _autoFixture.Create<FakeData>();

            var sut = new Vertex<FakeData>(data);

            var result = sut.Equals(new object());

            result.Should().BeFalse();
        }

        [Fact]
        public void When_has_no_edges_and_equals_called_with_object_that_has_edges_then_return_false()
        {
            var data = _autoFixture.Create<FakeData>();

            var vertexA = new Vertex<FakeData>(data);
            var vertexB = new Vertex<FakeData>(data);

            var edges = new[] { vertexA, vertexB };

            var sut1 = new Vertex<FakeData>(data);
            var sut2 = new Vertex<FakeData>(data, edges);

            var result = sut1.Equals(sut2);

            result.Should().BeFalse();
        }

        [Fact]
        public void When_has_edges_and_equals_called_with_object_that_has_no_edges_then_return_false()
        {
            var data = _autoFixture.Create<FakeData>();

            var vertexA = new Vertex<FakeData>(data);
            var vertexB = new Vertex<FakeData>(data);

            var edges = new[] { vertexA, vertexB };

            var sut1 = new Vertex<FakeData>(data, edges);
            var sut2 = new Vertex<FakeData>(data);

            var result = sut1.Equals(sut2);

            result.Should().BeFalse();
        }

        [Fact]
        public void When_has_edges_and_equals_called_with_object_that_has_different_edge_count_then_return_false()
        {
            var data = _autoFixture.Create<FakeData>();

            var vertex2 = new Vertex<FakeData>(data);
            var vertexB = new Vertex<FakeData>(data);
            var vertexC = new Vertex<FakeData>(data);

            var edges1 = new[] { vertex2 };
            var edgesA = new[] { vertexB, vertexC };

            var sut1 = new Vertex<FakeData>(data, edges1);
            var sut2 = new Vertex<FakeData>(data, edgesA);

            var result = sut1.Equals(sut2);

            result.Should().BeFalse();
        }

        [Fact]
        public void When_equals_called_with_object_that_has_different_data_then_return_false()
        {
            var data1 = _autoFixture.Create<FakeData>();
            var dataA = _autoFixture.Create<FakeData>();

            var sut1 = new Vertex<FakeData>(data1);
            var sut2 = new Vertex<FakeData>(dataA);

            var result = sut1.Equals(sut2);

            result.Should().BeFalse();
        }

        [Fact]
        public void When_equals_called_with_object_that_has_edges_that_are_circular_then_do_not_infinite_loop()
        {
            var data1 = new FakeData { Id = 1, Name = "A" };
            var data2 = new FakeData { Id = 2, Name = "B" };
            var data3 = new FakeData { Id = 3, Name = "C" };

            var vertex1 = new Vertex<FakeData>(data1);
            var vertex2 = new Vertex<FakeData>(data2);
            var vertex3 = new Vertex<FakeData>(data3);

            vertex1.Edges.Add(vertex2);
            vertex2.Edges.Add(vertex3);
            vertex3.Edges.Add(vertex1);

            vertex1.ExecutionTimeOf(x => x.Equals(vertex2))
                   .Should().BeLessThanOrEqualTo(500.Milliseconds());
        }
    }
}
