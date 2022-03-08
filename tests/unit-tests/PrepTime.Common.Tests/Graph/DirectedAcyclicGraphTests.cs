using Autofac.Extras.NSubstitute;
using FluentAssertions;
using NSubstitute;
using PrepTime.Common.Graph;
using PrepTime.Common.Tests.Fakes;
using System;
using System.Collections.Generic;
using Xunit;

namespace PrepTime.Common.Tests.Graph
{
    public class DirectedAcyclicGraphTests
    {
        [Fact]
        public void When_ctor_called_then_dag_is_set_to_defaults()
        {
            using var autoSub = new AutoSubstitute();

            var sut = autoSub.Resolve<DirectedAcyclicGraph>();

            sut.SortedVertices.Should().BeEmpty();
        }

        [Fact]
        public void When_single_vertex_is_added_with_tryaddvertex_then_vertex_is_in_sortedvertices_and_return_true()
        {
            using var autoSub = new AutoSubstitute();

            var vertex = new Vertex<FakeData>(new FakeData(1));

            var sorter = autoSub.Resolve<ITopologicalSorter>();
            sorter.TryDepthFirst(ref Arg.Any<IList<IVertex>>()).Returns(true);

            var sut = autoSub.Resolve<DirectedAcyclicGraph>();

            var result = sut.TryAddVertex(vertex);

            result.Should().BeTrue();
            sut.SortedVertices.Should().HaveCount(1);
            sut.SortedVertices.Should().Contain(vertex);
        }

        [Fact]
        public void When_multiple_acyclic_vertices_are_added_with_tryaddvertex_then_vertices_are_in_sortedvertices_and_return_true()
        {
            using var autoSub = new AutoSubstitute();

            var vertex1 = new Vertex<FakeData>(new FakeData(1));
            var vertex2 = new Vertex<FakeData>(new FakeData(2));
            var vertex3 = new Vertex<FakeData>(new FakeData(3));

            var sorter = autoSub.Resolve<ITopologicalSorter>();
            sorter.TryDepthFirst(ref Arg.Any<IList<IVertex>>()).Returns(true);

            var sut = autoSub.Resolve<DirectedAcyclicGraph>();

            var result = sut.TryAddVertex(vertex1);
            result.Should().BeTrue();

            result = sut.TryAddVertex(vertex2);
            result.Should().BeTrue();

            result = sut.TryAddVertex(vertex3);
            result.Should().BeTrue();

            sut.SortedVertices.Should().HaveCount(3);
            sut.SortedVertices.Should().Contain(vertex1);
            sut.SortedVertices.Should().Contain(vertex2);
            sut.SortedVertices.Should().Contain(vertex3);
        }

        [Fact]
        public void When_cyclic_vertex_is_added_with_tryaddvertex_then_added_vertex_is_not_in_sortedvertices_and_return_false()
        {
            using var autoSub = new AutoSubstitute();

            var vertex1 = new Vertex<FakeData>(new FakeData(1));
            var vertex2 = new Vertex<FakeData>(new FakeData(2));

            var sorter = autoSub.Resolve<ITopologicalSorter>();
            sorter.TryDepthFirst(ref Arg.Any<IList<IVertex>>()).Returns(true, false);

            var sut = autoSub.Resolve<DirectedAcyclicGraph>();

            var result = sut.TryAddVertex(vertex1);
            result.Should().BeTrue();

            result = sut.TryAddVertex(vertex2);
            result.Should().BeFalse();

            sut.SortedVertices.Should().HaveCount(1);
            sut.SortedVertices.Should().Contain(vertex1);
        }

        [Fact]
        public void When_single_vertex_is_added_with_addvertex_then_vertex_is_in_sortedvertices_and_return()
        {
            using var autoSub = new AutoSubstitute();

            var vertex = new Vertex<FakeData>(new FakeData(1));

            var sorter = autoSub.Resolve<ITopologicalSorter>();
            sorter.TryDepthFirst(ref Arg.Any<IList<IVertex>>()).Returns(true);

            var sut = autoSub.Resolve<DirectedAcyclicGraph>();

            sut.AddVertex(vertex);

            sut.SortedVertices.Should().HaveCount(1);
            sut.SortedVertices.Should().Contain(vertex);
        }

        [Fact]
        public void When_multiple_acyclic_vertices_are_added_with_addvertex_then_vertices_are_in_sortedvertices_and_return()
        {
            using var autoSub = new AutoSubstitute();

            var vertex1 = new Vertex<FakeData>(new FakeData(1));
            var vertex2 = new Vertex<FakeData>(new FakeData(2));
            var vertex3 = new Vertex<FakeData>(new FakeData(3));

            var sorter = autoSub.Resolve<ITopologicalSorter>();
            sorter.TryDepthFirst(ref Arg.Any<IList<IVertex>>()).Returns(true);

            var sut = autoSub.Resolve<DirectedAcyclicGraph>();

            sut.AddVertex(vertex1);
            sut.AddVertex(vertex2);
            sut.AddVertex(vertex3);

            sut.SortedVertices.Should().HaveCount(3);
            sut.SortedVertices.Should().Contain(vertex1);
            sut.SortedVertices.Should().Contain(vertex2);
            sut.SortedVertices.Should().Contain(vertex3);
        }

        [Fact]
        public void When_cyclic_vertex_is_added_with_addvertex_then_added_vertex_is_not_in_sortedvertices_and_throw()
        {
            using var autoSub = new AutoSubstitute();

            var vertex1 = new Vertex<FakeData>(new FakeData(1));
            var vertex2 = new Vertex<FakeData>(new FakeData(2));

            var sorter = autoSub.Resolve<ITopologicalSorter>();
            sorter.TryDepthFirst(ref Arg.Any<IList<IVertex>>()).Returns(true, false);

            var sut = autoSub.Resolve<DirectedAcyclicGraph>();

            sut.AddVertex(vertex1);

            Action act = () => sut.AddVertex(vertex2);

            act.Should().Throw<InvalidOperationException>();

            sut.SortedVertices.Should().HaveCount(1);
            sut.SortedVertices.Should().Contain(vertex1);
        }

        [Fact]
        public void When_single_vertex_is_removed_then_sortedvertices_is_empty_and_return_true()
        {
            using var autoSub = new AutoSubstitute();

            var vertex1 = new Vertex<FakeData>(new FakeData(1));

            var sorter = autoSub.Resolve<ITopologicalSorter>();
            sorter.TryDepthFirst(ref Arg.Any<IList<IVertex>>()).Returns(true);

            var sut = autoSub.Resolve<DirectedAcyclicGraph>();

            sut.AddVertex(vertex1);
            var result = sut.RemoveVertex(vertex1);

            result.Should().BeTrue();
            sut.SortedVertices.Should().BeEmpty();
        }

        [Fact]
        public void When_vertex_is_removed_then_vertex_is_not_in_sortedvertices_and_return_true()
        {
            using var autoSub = new AutoSubstitute();

            var vertex1 = new Vertex<FakeData>(new FakeData(1));
            var vertex2 = new Vertex<FakeData>(new FakeData(2));

            var sorter = autoSub.Resolve<ITopologicalSorter>();
            sorter.TryDepthFirst(ref Arg.Any<IList<IVertex>>()).Returns(true);

            var sut = autoSub.Resolve<DirectedAcyclicGraph>();

            sut.AddVertex(vertex1);
            sut.AddVertex(vertex2);
            var result = sut.RemoveVertex(vertex1);

            result.Should().BeTrue();
            sut.SortedVertices.Should().HaveCount(1);
            sut.SortedVertices[0].Should().Be(vertex2);
        }

        [Fact]
        public void When_vertex_is_removed_but_is_not_in_graph_then_return_false()
        {
            using var autoSub = new AutoSubstitute();

            var vertex1 = new Vertex<FakeData>(new FakeData(1));
            var vertex2 = new Vertex<FakeData>(new FakeData(2));

            var sorter = autoSub.Resolve<ITopologicalSorter>();
            sorter.TryDepthFirst(ref Arg.Any<IList<IVertex>>()).Returns(true);

            var sut = autoSub.Resolve<DirectedAcyclicGraph>();

            sut.AddVertex(vertex1);

            var result = sut.RemoveVertex(vertex2);

            result.Should().BeFalse();
            sut.SortedVertices.Should().HaveCount(1);
            sut.SortedVertices[0].Should().Be(vertex1);
        }

        [Fact]
        public void When_the_same_vertex_is_added_twice_then_return_false()
        {
            using var autoSub = new AutoSubstitute();

            var vertex = new Vertex<FakeData>(new FakeData(1));

            var sorter = autoSub.Resolve<ITopologicalSorter>();
            sorter.TryDepthFirst(ref Arg.Any<IList<IVertex>>()).Returns(true);

            var sut = autoSub.Resolve<DirectedAcyclicGraph>();

            sut.TryAddVertex(vertex);

            var result = sut.TryAddVertex(vertex);

            result.Should().BeFalse();
        }

        [Fact]
        public void When_two_different_vertices_that_contain_the_same_data_are_added_then_return_true()
        {
            using var autoSub = new AutoSubstitute();

            var data = new FakeData(0);

            var vertex1 = new Vertex<FakeData>(data);
            var vertex2 = new Vertex<FakeData>(data);

            var sorter = autoSub.Resolve<ITopologicalSorter>();
            sorter.TryDepthFirst(ref Arg.Any<IList<IVertex>>()).Returns(true);

            var sut = autoSub.Resolve<DirectedAcyclicGraph>();

            sut.TryAddVertex(vertex1);

            var result = sut.TryAddVertex(vertex2);

            result.Should().BeTrue();
        }
    }
}
