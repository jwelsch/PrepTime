using FluentAssertions;
using PrepTime.Common.Graph;
using PrepTime.Common.Tests.Fakes;
using PrepTime.Common.Tests.Testing;
using System.Collections.Generic;
using Xunit;

namespace PrepTime.Common.Tests.Graph
{
    public class TopologicalSorterTests
    {
        [Fact]
        public void When_unsortedvertices_is_empty_then_sortedvertices_is_empty_and_return_false()
        {
            var unsorted = new List<IVertex>();

            var sut = new TopologicalSorter();

            var result = sut.TryDepthFirst(unsorted, out IList<IVertex> sorted);

            result.Should().BeFalse();
            sorted.Should().BeEmpty();
        }

        [Fact]
        public void When_unsortedvertices_has_one_vertex_then_sortedvertices_has_one_vertex_and_return_true()
        {
            var unsorted = new List<IVertex>()
            {
                new Vertex<FakeData>(new FakeData())
            };

            var sut = new TopologicalSorter();

            var result = sut.TryDepthFirst(unsorted, out IList<IVertex> sorted);

            result.Should().BeTrue();
            sorted.Should().HaveCount(unsorted.Count);
            sorted.Should().Contain(unsorted[0]);
        }

        [Fact]
        public void When_unsortedvertices_has_simple_acyclic_vertices_in_sorted_order_then_sortedvertices_is_correct_and_return_true()
        {
            // Graph:
            //
            // 2   3
            //  \ /
            //   1

            var vertex1 = new Vertex<FakeData>(new FakeData(1));
            var vertex2 = new Vertex<FakeData>(new FakeData(2), vertex1);
            var vertex3 = new Vertex<FakeData>(new FakeData(3), vertex1);

            var unsorted = new List<IVertex>()
            {
                vertex2,
                vertex3,
                vertex1
            };

            var sut = new TopologicalSorter();

            var result = sut.TryDepthFirst(unsorted, out IList<IVertex> sorted);

            result.Should().BeTrue();
            sorted.Should().HaveCount(unsorted.Count)
                           .And.OnlyHaveUniqueItems();
            sorted.Should().Satisfy
            (
                i => i.Equals(vertex2) || i.Equals(vertex3),
                i => i.Equals(vertex2) || i.Equals(vertex3),
                i => i.Equals(vertex1)
            );
        }

        [Fact]
        public void When_unsortedvertices_has_simple_acyclic_vertices_in_unsorted_order_then_sortedvertices_is_correct_and_return_true()
        {
            // Graph:
            //
            // 2   3
            //  \ /
            //   1

            var vertex1 = new Vertex<FakeData>(new FakeData(1));
            var vertex2 = new Vertex<FakeData>(new FakeData(2), vertex1);
            var vertex3 = new Vertex<FakeData>(new FakeData(3), vertex1);

            var unsorted = new List<IVertex>()
            {
                vertex1,
                vertex2,
                vertex3
            };

            var sut = new TopologicalSorter();

            var result = sut.TryDepthFirst(unsorted, out IList<IVertex> sorted);

            result.Should().BeTrue();
            sorted.Should().HaveCount(unsorted.Count)
                           .And.OnlyHaveUniqueItems();
            sorted[0].Should().BeOneOf(vertex2, vertex3);
            sorted[1].Should().BeOneOf(vertex2, vertex3);
            sorted[2].Should().Be(vertex1);
        }

        [Fact]
        public void When_unsortedvertices_has_multiple_acyclic_vertices_in_unsorted_order_then_sortedvertices_is_correct_and_return_true()
        {
            // Graph:
            //
            // 8   7   6   5
            //  \   \ / \ /
            //   4   3   2
            //    \ /   /
            //     1___/

            var vertex1 = new Vertex<FakeData>(new FakeData(1));
            var vertex2 = new Vertex<FakeData>(new FakeData(2), vertex1);
            var vertex3 = new Vertex<FakeData>(new FakeData(3), vertex1);
            var vertex4 = new Vertex<FakeData>(new FakeData(4), vertex1);
            var vertex5 = new Vertex<FakeData>(new FakeData(5), vertex2);
            var vertex6 = new Vertex<FakeData>(new FakeData(6), vertex2, vertex3);
            var vertex7 = new Vertex<FakeData>(new FakeData(7), vertex3);
            var vertex8 = new Vertex<FakeData>(new FakeData(8), vertex4);

            var unsorted = new List<IVertex>()
            {
                vertex1,
                vertex2,
                vertex3,
                vertex4,
                vertex5,
                vertex6,
                vertex7,
                vertex8
            };

            var sut = new TopologicalSorter();

            var result = sut.TryDepthFirst(unsorted, out IList<IVertex> sorted);

            result.Should().BeTrue();
            sorted.Should().HaveCount(unsorted.Count)
                           .And.OnlyHaveUniqueItems();
            sorted[0].Should().BeOneOf(vertex8, vertex7, vertex6, vertex5);
            sorted[1].Should().BeOneOf(vertex8, vertex7, vertex6, vertex5, vertex4);
            sorted[2].Should().BeOneOf(vertex8, vertex7, vertex6, vertex5, vertex4, vertex3, vertex2);
            sorted[3].Should().BeOneOf(vertex8, vertex7, vertex6, vertex5, vertex4, vertex3, vertex2);
            sorted[4].Should().BeOneOf(vertex8, vertex7, vertex6, vertex5, vertex4, vertex3, vertex2);
            sorted[5].Should().BeOneOf(vertex8, vertex7, vertex6, vertex5, vertex4, vertex3, vertex2);
            sorted[6].Should().BeOneOf(vertex7, vertex6, vertex5, vertex4, vertex3, vertex2);
            sorted[7].Should().Be(vertex1);
        }

        [Fact]
        public void When_unsortedvertices_has_multiple_acyclic_vertices_with_multiple_edges_in_unsorted_order_then_sortedvertices_is_correct_and_return_true()
        {
            // Graph:
            //
            // 8   7   6   5
            //  \   \ / \ /
            //   4-->3-->2
            //    \ /   /
            //     1---+

            var vertex1 = new Vertex<FakeData>(new FakeData(1));
            var vertex2 = new Vertex<FakeData>(new FakeData(2), vertex1);
            var vertex3 = new Vertex<FakeData>(new FakeData(3), vertex1, vertex2);
            var vertex4 = new Vertex<FakeData>(new FakeData(4), vertex3, vertex1);
            var vertex5 = new Vertex<FakeData>(new FakeData(5), vertex2);
            var vertex6 = new Vertex<FakeData>(new FakeData(6), vertex2, vertex3);
            var vertex7 = new Vertex<FakeData>(new FakeData(7), vertex3);
            var vertex8 = new Vertex<FakeData>(new FakeData(8), vertex4);

            var unsorted = new List<IVertex>()
            {
                vertex7,
                vertex1,
                vertex4,
                vertex8,
                vertex5,
                vertex3,
                vertex6,
                vertex2,
            };

            var sut = new TopologicalSorter();

            var result = sut.TryDepthFirst(unsorted, out IList<IVertex> sorted);

            result.Should().BeTrue();
            sorted.Should().HaveCount(unsorted.Count)
                           .And.OnlyHaveUniqueItems();
            sorted[0].Should().BeOneOf(vertex8, vertex7, vertex6, vertex5);
            sorted[1].Should().BeOneOf(vertex8, vertex7, vertex6, vertex5, vertex4);
            sorted[2].Should().BeOneOf(vertex8, vertex7, vertex6, vertex5, vertex4, vertex3);
            sorted[3].Should().BeOneOf(vertex8, vertex7, vertex6, vertex5, vertex4, vertex3);
            sorted[4].Should().BeOneOf(vertex8, vertex7, vertex6, vertex5, vertex4, vertex3);
            sorted[5].Should().BeOneOf(vertex8, vertex7, vertex6, vertex5, vertex4, vertex3);
            sorted[6].Should().BeOneOf(vertex7, vertex6, vertex5, vertex4, vertex3, vertex2);
            sorted[7].Should().Be(vertex1);

            //foreach (Vertex<FakeData> s in sorted)
            //{
            //    System.Diagnostics.Trace.WriteLine(s.Data.Id);
            //}
        }

        [Fact]
        public void When_unsortedvertices_has_simple_acyclic_vertices_with_multiple_end_vertices_in_unsorted_order_then_sortedvertices_is_correct_and_return_true()
        {
            // Graph:
            //
            // 3   4   5
            //  \ / \ /
            //   2   1

            var vertex1 = new Vertex<FakeData>(new FakeData(1));
            var vertex2 = new Vertex<FakeData>(new FakeData(2));
            var vertex3 = new Vertex<FakeData>(new FakeData(3), vertex2);
            var vertex4 = new Vertex<FakeData>(new FakeData(4), vertex1, vertex2);
            var vertex5 = new Vertex<FakeData>(new FakeData(5), vertex1);

            var unsorted = new List<IVertex>()
            {
                vertex1,
                vertex5,
                vertex2,
                vertex4,
                vertex3
            };

            var sut = new TopologicalSorter();

            var result = sut.TryDepthFirst(unsorted, out IList<IVertex> sorted);

            result.Should().BeTrue();
            sorted.Should().HaveCount(unsorted.Count)
                           .And.OnlyHaveUniqueItems();
            sorted[0].Should().BeOneOf(vertex3, vertex4, vertex5);
            sorted[1].Should().BeOneOf(vertex3, vertex4, vertex5);
            sorted[2].Should().BeOneOf(vertex1, vertex2, vertex3, vertex4, vertex5);
            sorted[3].Should().BeOneOf(vertex1, vertex2, vertex3, vertex4, vertex5);
            sorted[4].Should().BeOneOf(vertex1, vertex2);
        }

        [Fact]
        public void When_unsortedvertices_has_simple_acyclic_vertices_with_single_start_vertice_and_multiple_end_vertices_in_unsorted_order_then_sortedvertices_is_correct_and_return_true()
        {
            // Graph:
            //
            //     6
            //    /|\
            //   / | \
            //  /  |  \
            // 3   4   5
            //  \ / \ /
            //   2   1

            var vertex1 = new Vertex<FakeData>(new FakeData(1));
            var vertex2 = new Vertex<FakeData>(new FakeData(2));
            var vertex3 = new Vertex<FakeData>(new FakeData(3), vertex2);
            var vertex4 = new Vertex<FakeData>(new FakeData(4), vertex1, vertex2);
            var vertex5 = new Vertex<FakeData>(new FakeData(5), vertex1);
            var vertex6 = new Vertex<FakeData>(new FakeData(6), vertex3, vertex4, vertex5);

            var unsorted = new List<IVertex>()
            {
                vertex1,
                vertex5,
                vertex2,
                vertex4,
                vertex6,
                vertex3
            };

            var sut = new TopologicalSorter();

            var result = sut.TryDepthFirst(unsorted, out IList<IVertex> sorted);

            result.Should().BeTrue();
            sorted.Should().HaveCount(unsorted.Count)
                           .And.OnlyHaveUniqueItems();
            sorted[0].Should().BeOneOf(vertex6);
            sorted[1].Should().BeOneOf(vertex3, vertex4, vertex5);
            sorted[2].Should().BeOneOf(vertex3, vertex4, vertex5);
            sorted[3].Should().BeOneOf(vertex1, vertex2, vertex3, vertex4, vertex5);
            sorted[4].Should().BeOneOf(vertex1, vertex2, vertex3, vertex4, vertex5);
            sorted[5].Should().BeOneOf(vertex1, vertex2);
        }

        [Fact]
        public void When_unsortedvertices_has_simple_cyclic_vertices_then_sortedvertices_contains_cyclic_vertex_and_return_false()
        {
            // Graph:
            //
            // 2<->3
            //  \ /
            //   1

            var vertex1 = new Vertex<FakeData>(new FakeData(1));
            var vertex2 = new Vertex<FakeData>(new FakeData(2));
            var vertex3 = new Vertex<FakeData>(new FakeData(3));

            vertex2.Edges.Add(vertex1);
            vertex2.Edges.Add(vertex3);

            vertex3.Edges.Add(vertex1);
            vertex3.Edges.Add(vertex2);

            var unsorted = new List<IVertex>()
            {
                vertex2,
                vertex3,
                vertex1
            };

            var sut = new TopologicalSorter();

            var result = sut.TryDepthFirst(unsorted, out IList<IVertex> sorted);

            result.Should().BeFalse();
            sorted.Should().HaveCount(1);
            sorted[0].Should().BeOneOf(vertex2, vertex3);
        }

        [Fact]
        public void When_unsortedvertices_has_multiple_ccyclic_vertices_with_multiple_edges_in_unsorted_order_then_sortedvertices_contains_cyclic_vertex_and_return_false()
        {
            // Graph:
            //
            // 8   7   6   5
            //  \   \ / \ /
            //   4<->3<->2
            //    \ /   /
            //     1---+

            var vertex1 = new Vertex<FakeData>(new FakeData(1));
            var vertex2 = new Vertex<FakeData>(new FakeData(2), vertex1);
            var vertex3 = new Vertex<FakeData>(new FakeData(3), vertex1, vertex2);
            var vertex4 = new Vertex<FakeData>(new FakeData(4), vertex3, vertex1);
            var vertex5 = new Vertex<FakeData>(new FakeData(5), vertex2);
            var vertex6 = new Vertex<FakeData>(new FakeData(6), vertex2, vertex3);
            var vertex7 = new Vertex<FakeData>(new FakeData(7), vertex3);
            var vertex8 = new Vertex<FakeData>(new FakeData(8), vertex4);

            vertex2.Edges.Add(vertex3);

            vertex3.Edges.Add(vertex2);
            vertex3.Edges.Add(vertex4);

            var unsorted = new List<IVertex>()
            {
                vertex7,
                vertex1,
                vertex4,
                vertex8,
                vertex5,
                vertex3,
                vertex6,
                vertex2,
            };

            var sut = new TopologicalSorter();

            var result = sut.TryDepthFirst(unsorted, out IList<IVertex> sorted);

            result.Should().BeFalse();
            sorted.Should().HaveCount(1);
            sorted[0].Should().BeOneOf(vertex2, vertex3, vertex4);
        }

        [Fact]
        public void When_unsortedvertices_has_cyclic_vertices_separated_by_multiple_edges_in_unsorted_order_then_sortedvertices_contains_cyclic_vertex_and_return_false()
        {
            // Graph:
            //
            //     9<-----+
            //    / \     |
            //   8   7    |
            //  / \ / \   |
            // 6   5   4  |
            //  \ / \ /   |
            //   3   2    |
            //    \ /     |
            //     1------+

            var vertex1 = new Vertex<FakeData>(new FakeData(1));
            var vertex2 = new Vertex<FakeData>(new FakeData(2), vertex1);
            var vertex3 = new Vertex<FakeData>(new FakeData(3), vertex1);
            var vertex4 = new Vertex<FakeData>(new FakeData(4), vertex2);
            var vertex5 = new Vertex<FakeData>(new FakeData(5), vertex2, vertex3);
            var vertex6 = new Vertex<FakeData>(new FakeData(6), vertex3);
            var vertex7 = new Vertex<FakeData>(new FakeData(7), vertex4, vertex5);
            var vertex8 = new Vertex<FakeData>(new FakeData(8), vertex5, vertex6);
            var vertex9 = new Vertex<FakeData>(new FakeData(9), vertex7, vertex8);

            vertex1.Edges.Add(vertex9);

            var unsorted = new List<IVertex>()
            {
                vertex9,
                vertex5,
                vertex2,
                vertex8,
                vertex4,
                vertex6,
                vertex3,
                vertex7,
                vertex1
            };

            var sut = new TopologicalSorter();

            var result = sut.TryDepthFirst(unsorted, out IList<IVertex> sorted);

            result.Should().BeFalse();
            sorted.Should().HaveCount(1);
            sorted[0].Should().Be(vertex1);
        }

        [Fact]
        public void When_vertices_has_simple_acyclic_vertices_in_unsorted_order_then_ref_vertices_is_correct_and_return_true()
        {
            // Graph:
            //
            // 2   3
            //  \ /
            //   1

            var vertex1 = new Vertex<FakeData>(new FakeData(1));
            var vertex2 = new Vertex<FakeData>(new FakeData(2), vertex1);
            var vertex3 = new Vertex<FakeData>(new FakeData(3), vertex1);

            var vertices = (IList<IVertex>)new List<IVertex>()
            {
                vertex1,
                vertex2,
                vertex3
            };

            var sut = new TopologicalSorter();

            var result = sut.TryDepthFirst(ref vertices);

            result.Should().BeTrue();
            vertices.Should().HaveCount(vertices.Count)
                             .And.OnlyHaveUniqueItems();
            vertices[0].Should().BeOneOf(vertex2, vertex3);
            vertices[1].Should().BeOneOf(vertex2, vertex3);
            vertices[2].Should().Be(vertex1);
        }
    }
}
