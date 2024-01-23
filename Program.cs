/* Project Name:  Assignment 1
   Group Members: Shahzan, Dinesh, Junaid
   Start Date:    5th October
   End Date:      14th October 
   Abstract:      Program demonstrates to use graphs and related operations and methods
*/


using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;


// Graph class represents a graph data structure.
public class Graph
{
    private Dictionary<int, List<(int, int)>> adjacencyList;

    // Constructor for Graph class.
    public Graph()
    {
        adjacencyList = new Dictionary<int, List<(int, int)>>();
    }

    // AddingVertex Method
    // AddVertex method adds a vertex to the graph.
    // Time complexity: O(1)
    public void AddVertex(int vertex)
    {
        if (!adjacencyList.ContainsKey(vertex))
        {
            adjacencyList[vertex] = new List<(int, int)>();
        }
    }

    // AddEdge Method
    // AddEdge method adds an edge between two vertices with a given weight.
    // Time complexity: O(1)
    public void AddEdge(int source, int destination, int weight)
    {
        if (adjacencyList.ContainsKey(source) && adjacencyList.ContainsKey(destination))
        {
            adjacencyList[source].Add((destination, weight));
            adjacencyList[destination].Add((source, weight));
        }
        else
        {
            throw new InvalidOperationException("Vertices not found.");
        }
    }

    // RemoveVertex Method
    // RemoveVertex method removes a vertex from the graph.
    // Time complexity: O(N)
    public void RemoveVertex(int vertex)
    {
        if (adjacencyList.ContainsKey(vertex))
        {
            adjacencyList.Remove(vertex);

            foreach (var vertices in adjacencyList.Values)
            {
                vertices.RemoveAll(v => v.Item1 == vertex);
            }
        }
        else
        {
            throw new InvalidOperationException("Vertex not found.");
        }
    }

    // RemoveEdge Method
    // RemoveEdge method removes an edge between two vertices.
    // Time complexity: O(N)
    public void RemoveEdge(int source, int destination)
    {
        if (adjacencyList.ContainsKey(source) && adjacencyList.ContainsKey(destination))
        {
            adjacencyList[source].RemoveAll(v => v.Item1 == destination);
            adjacencyList[destination].RemoveAll(v => v.Item1 == source);
        }
        else
        {
            throw new InvalidOperationException("Edge not found.");
        }
    }

    // DFS Method
    // DFS method performs depth-first search traversal on the graph.
    // Time complexity: O(N)
    public void DFS(int startVertex)
    {
        Console.WriteLine("Detailed DFS Visualization:");
        HashSet<int> visited = new HashSet<int>();
        DFSHelper(startVertex, visited);
    }

    // DFSHelper Method
    // Recursive function for DFS traversal from a given vertex
    // Time complexity: O(V + E), where V is the number of vertices, and E is the number of edges.
    private void DFSHelper(int vertex, HashSet<int> visited)
    {
        Console.Write(vertex + " ");
        visited.Add(vertex);

        foreach (var neighbor in adjacencyList[vertex])
        {
            if (!visited.Contains(neighbor.Item1))
            {
                Console.WriteLine($"Edge: {vertex} -> {neighbor.Item1}");
                DFSHelper(neighbor.Item1, visited);
            }
        }
    }

    // BFS Method
    // BFS method performs breadth-first search traversal on the graph.
    // Time complexity: O(N)
    public void BFS(int startVertex)
    {
        Console.WriteLine("Detailed BFS Visualization:");
        HashSet<int> visited = new HashSet<int>();
        Queue<int> queue = new Queue<int>();

        visited.Add(startVertex);
        queue.Enqueue(startVertex);

        while (queue.Count != 0)
        {
            int vertex = queue.Dequeue();
            Console.Write(vertex + " ");

            foreach (var neighbor in adjacencyList[vertex])
            {
                if (!visited.Contains(neighbor.Item1))
                {
                    Console.WriteLine($"Edge: {vertex} -> {neighbor.Item1}");
                    visited.Add(neighbor.Item1);
                    queue.Enqueue(neighbor.Item1);
                }
            }
        }
    }

    // IsConnected Method
    // IsConnected method checks if the graph is connected.
    // Time complexity: O(N)
    public bool IsConnected()
    {
        HashSet<int> visited = new HashSet<int>();
        DFSHelper(adjacencyList.Keys.First(), visited);
        return visited.Count == adjacencyList.Count;
    }

    // Dijkstra's Shortest Path Algorithm
    // Dijkstra method finds the shortest path using Dijkstra's algorithm.
    // Time complexity: O(E * log(V))
    public Dictionary<int, int> Dijkstra(int startVertex)
    {
        Dictionary<int, int> distances = new Dictionary<int, int>();
        foreach (var vertex in adjacencyList.Keys)
        {
            distances[vertex] = int.MaxValue;
        }
        distances[startVertex] = 0;

        PriorityQueue<(int, int)> priorityQueue = new PriorityQueue<(int, int)>();
        priorityQueue.Enqueue((startVertex, 0));

        while (priorityQueue.Count != 0)
        {
            var (currentVertex, currentDistance) = priorityQueue.Dequeue();

            if (currentDistance > distances[currentVertex])
            {
                continue;
            }

            foreach (var (neighbor, weight) in adjacencyList[currentVertex])
            {
                int newDistance = currentDistance + weight;

                if (newDistance < distances[neighbor])
                {
                    distances[neighbor] = newDistance;
                    priorityQueue.Enqueue((neighbor, newDistance));
                }
            }
        }

        return distances;
    }

    // GetEdgeWeight Method
    // GetEdgeWeight method returns the weight of an edge between two vertices.
    // Time complexity: O(N)
    public int GetEdgeWeight(int source, int destination)
    {
        foreach (var (neighbor, weight) in adjacencyList[source])
        {
            if (neighbor == destination)
            {
                return weight;
            }
        }
        return -1; // Return -1 if the edge doesn't exist
    }

    // HasCycle Method
    // HasCycle method checks if the graph contains cycles using DFS.
    // Time complexity: O(N)
    public bool HasCycle()
    {
        HashSet<int> visited = new HashSet<int>();
        foreach (var vertex in adjacencyList.Keys)
        {
            if (!visited.Contains(vertex) && HasCycleHelper(vertex, -1, visited))
            {
                return true;
            }
        }
        return false;
    }


    private bool HasCycleHelper(int vertex, int parent, HashSet<int> visited)
    {
        visited.Add(vertex);

        foreach (var neighbor in adjacencyList[vertex])
        {
            if (!visited.Contains(neighbor.Item1))
            {
                if (HasCycleHelper(neighbor.Item1, vertex, visited))
                {
                    return true;
                }
            }
            else if (neighbor.Item1 != parent)
            {
                return true;
            }
        }
        return false;
    }

    // Minimum Spanning Tree Method (Kruskal's Algorithm)
    // Kruskal method finds the minimum spanning tree using Kruskal's algorithm.
    // Time complexity: O(E * log(E))
    public List<(int, int, int)> Kruskal()
    {
        List<(int, int, int)> result = new List<(int, int, int)>();
        PriorityQueue<(int, int, int)> edges = new PriorityQueue<(int, int, int)>();

        foreach (var vertex in adjacencyList.Keys)
        {
            foreach (var (neighbor, weight) in adjacencyList[vertex])
            {
                edges.Enqueue((vertex, neighbor, weight));
            }
        }

        DisjointSet disjointSet = new DisjointSet(adjacencyList.Keys);

        while (edges.Count != 0)
        {
            var (source, destination, weight) = edges.Dequeue();

            if (disjointSet.Union(source, destination))
            {
                result.Add((source, destination, weight));
            }
        }

        return result;
    }
}

// PriorityQueue class represents a priority queue used in Dijkstra's and Kruskal's algorithms.
// is designed to maintain elements in order of their priority (key)
// and efficiently support operations like adding elements with priorities
// and removing the element with the highest priority
public class PriorityQueue<T>
{
    private List<T> elements = new List<T>();

    public int Count { get { return elements.Count; } }

    public void Enqueue(T item)
    {
        elements.Add(item);
        int i = elements.Count - 1;

        while (i > 0)
        {
            int parent = (i - 1) / 2;

            if (Comparer<T>.Default.Compare(elements[i], elements[parent]) >= 0)
                break;

            T temp = elements[i];
            elements[i] = elements[parent];
            elements[parent] = temp;

            i = parent;
        }
    }
    //The Dequeue method removes and returns the element with the highest priority
    // (the element with the smallest key) from the priority queue.
    // It first checks if the queue is empty and throws an exception if it is.
    public T Dequeue()
    {
        if (elements.Count == 0)
            throw new InvalidOperationException("Queue is empty.");

        T top = elements[0];
        elements[0] = elements[elements.Count - 1];
        elements.RemoveAt(elements.Count - 1);

        int i = 0;
        while (true)
        {
            int left = 2 * i + 1;
            int right = 2 * i + 2;
            int smallest = i;

            if (left < elements.Count && Comparer<T>.Default.Compare(elements[left], elements[smallest]) < 0)
                smallest = left;

            if (right < elements.Count && Comparer<T>.Default.Compare(elements[right], elements[smallest]) < 0)
                smallest = right;

            if (smallest == i)
                break;

            T temp = elements[i];
            elements[i] = elements[smallest];
            elements[smallest] = temp;

            i = smallest;
        }

        return top;
    }
}

// DisjointSet class represents a disjoint-set data structure used in Kruskal's algorithm.
// The constructor public DisjointSet(IEnumerable<int> elements) initializes the disjoint-set data structure with a collection of elements.
// It creates a dictionary called parent to keep track of the parent (representative) of each element
public class DisjointSet
{
    private Dictionary<int, int> parent;

    public DisjointSet(IEnumerable<int> elements)
    {
        parent = new Dictionary<int, int>();
        foreach (var element in elements)
        {
            parent[element] = element;
        }
    }
    // The Find method used to find the representative (root) of a set.
   // It takes an element as input and recursively follows the chain of parent pointers 
   //To optimize future Find operations, it also performs path compression.
    public int Find(int element)
    {
        if (parent[element] == element)
            return element;

        return parent[element] = Find(parent[element]);
    }

    // The Union method merges (union) two sets that contain elements a and b.
    // It takes two elements a and b as input.
    public bool Union(int a, int b)
    {
        int rootA = Find(a);
        int rootB = Find(b);

        if (rootA == rootB)
            return false;

        parent[rootA] = rootB;
        return true;
    }
}

// Program class contains the main method for running test cases.
class Program
{
    public static void Main(string[] args)
    {
        int choice;

        // Do loop containing choices for test iterations
        do
        {
            Console.WriteLine("Graphinator 9000");
            Console.WriteLine("\n==============================================");
            Console.WriteLine("Select a test case:");
            Console.WriteLine("1. Graph Creation and Connectivity");
            Console.WriteLine("2. Dijkstra's Shortest Path");
            Console.WriteLine("3. Cycle Detection");
            Console.WriteLine("4. Kruskal's Minimum Spanning Tree");
            Console.WriteLine("5. Get Edge Weight");
            Console.WriteLine("6. Depth-First Search (DFS)");
            Console.WriteLine("7. Breadth-First Search (BFS)");
            Console.WriteLine("8. Exit");
            Console.WriteLine("Enter choice (1-8):");

            // If statement containing switch cases for each case
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                switch (choice)
                {
                    case 1:
                        TestCase1();
                        break;
                    case 2:
                        TestCase2();
                        break;
                    case 3:
                        TestCase3();
                        break;
                    case 4:
                        TestCase4();
                        break;
                    case 5:
                        TestCase5();
                        break;
                    case 6:
                        TestCase6();
                        break;
                    case 7:
                        TestCase7();
                        break;
                    case 8:
                        Console.WriteLine("\nExiting...");
                        break;
                    default:
                        Console.WriteLine("\nInvalid choice!");
                        break;
                }
            }
            else
            {
                Console.WriteLine("\nPlease enter a valid number!");
            }

            if (choice != 8)
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }

        } while (choice != 8);
    }

    private static void TestCase1()
    {
        // Testcase1 which tests graph connectivity
        Console.WriteLine("Test Case 1: Graph Creation and Connectivity");
        Console.WriteLine();

        // Creates a graph and add vertices
        Graph graph1 = new Graph();
        graph1.AddVertex(1);
        graph1.AddVertex(2);
        graph1.AddVertex(3);
        graph1.AddVertex(4);
        graph1.AddVertex(5); 
        graph1.AddVertex(6); 
        graph1.AddEdge(1, 2, 3);
        graph1.AddEdge(2, 3, 2);
        graph1.AddEdge(3, 4, 5);
        graph1.AddEdge(4, 5, 2); 
       


        // Checks if graph is connected
        bool isConnected1 = graph1.IsConnected();
        Console.WriteLine("Is graph1 connected? " + isConnected1); // Should be false

        // Adds another edge and checks connectivity again
        graph1.AddEdge(5, 6, 1);
        isConnected1 = graph1.IsConnected();
        Console.WriteLine("Is graph1 connected after adding another edge? " + isConnected1); // Should be true
    }

    private static void TestCase2()
    {
        Console.WriteLine(" Test Case 2: Dijkstra's Shortest Path");
        Console.WriteLine();
        // Create a graph and add vertices
        Graph graph2 = new Graph();
        graph2.AddVertex(1);
        graph2.AddVertex(2);
        graph2.AddVertex(3);
        graph2.AddVertex(4);
        // and then add edges
        graph2.AddEdge(1, 2, 3);
        graph2.AddEdge(2, 3, 2);
        graph2.AddEdge(3, 4, 5);
        graph2.AddEdge(4, 1, 1);
        
        // Find and display shortest path distances from vertex 1
        Dictionary<int, int> distances = graph2.Dijkstra(1);
        Console.WriteLine("Shortest Path Distances from vertex 1:");
        foreach (var kvp in distances)
        {
            Console.WriteLine("To vertex " + kvp.Key + ": " + kvp.Value);
        }
    }

    private static void TestCase3()
    {
        Console.WriteLine("Test Case 3: Cycle Detection");
        Console.WriteLine();
        // Create a graph and add vertices
        Graph graph3 = new Graph();
        graph3.AddVertex(1);
        graph3.AddVertex(2);
        graph3.AddVertex(3);
        graph3.AddVertex(4);
        // and then add edges
        graph3.AddEdge(1, 2, 1);
        graph3.AddEdge(2, 3, 2);
        graph3.AddEdge(3, 4, 4);

        // checks graphs for cycles
        bool hasCycle1 = graph3.HasCycle();
        Console.WriteLine("Does graph3 contain cycles? " + hasCycle1); // Should be false
        // adds edge and then checks again for cycles
        graph3.AddEdge(4, 1, 4);
        bool hasCycle2 = graph3.HasCycle();
        Console.WriteLine("Does graph3 contain cycles after adding an edge? " + hasCycle2); // Should be true
    }

    private static void TestCase4()
    {
        Console.WriteLine("Test Case 4: Kruskal's Minimum Spanning Tree");
        Console.WriteLine();
        // Create a graph and add vertices
        Graph graph4 = new Graph();
        graph4.AddVertex(1);
        graph4.AddVertex(2);
        graph4.AddVertex(3);
        graph4.AddVertex(4);
        // and then add edges
        graph4.AddEdge(1, 2, 3);
        graph4.AddEdge(2, 3, 2);
        graph4.AddEdge(3, 4, 5);
        graph4.AddEdge(4, 1, 1);

        // Creates a Minimum spanning tree
        List<(int, int, int)> minSpanningTree = graph4.Kruskal();
        Console.WriteLine("Minimum Spanning Tree:");
        foreach (var edge in minSpanningTree)
        {
            Console.WriteLine($"Edge: ({edge.Item1}, {edge.Item2}) Weight: {edge.Item3}");
        }
    }

    private static void TestCase5()
    {
        Graph graph5 = new Graph();
        // Create a graph and add vertices 
        graph5.AddVertex(1);
        graph5.AddVertex(2);
        graph5.AddVertex(3);
        graph5.AddVertex(4);
        // and then add edges
        graph5.AddEdge(1, 2, 3);
        graph5.AddEdge(2, 3, 2);
        graph5.AddEdge(3, 4, 5);
        graph5.AddEdge(4, 1, 1);
        Console.WriteLine("Test Case 5: Get Edge Weight");
        Console.WriteLine();
        int weight12 = graph5.GetEdgeWeight(1, 2);
        Console.WriteLine("Edge weight between 1 and 2: " + weight12); // Should be 3

        int weight21 = graph5.GetEdgeWeight(2, 1);
        Console.WriteLine("Edge weight between 2 and 1: " + weight21); // Should be 3

        int weight14 = graph5.GetEdgeWeight(1, 4);
        Console.WriteLine("Edge weight between 1 and 4: " + weight14); // Should be 1

        int weight23 = graph5.GetEdgeWeight(2, 3);
        Console.WriteLine("Edge weight between 2 and 3: " + weight23); // Should be 2

        int weight34 = graph5.GetEdgeWeight(3, 4);
        Console.WriteLine("Edge weight between 3 and 4: " + weight34); // Should be 5
    }
    // Test Case 6: Depth-First Search (DFS)
    private static void TestCase6()
    {
        Console.WriteLine("Test Case 6: Depth-First Search (DFS)");
        Console.WriteLine();
        // Create a graph and add vertices and edges
        Graph graph6 = new Graph();
        graph6.AddVertex(1);
        graph6.AddVertex(2);
        graph6.AddVertex(3);
        graph6.AddVertex(4);
        graph6.AddVertex(5);
        graph6.AddVertex(6);
        graph6.AddEdge(1, 2, 1);
        graph6.AddEdge(2, 3, 1);
        graph6.AddEdge(3, 4, 1);
        graph6.AddEdge(4, 5, 1);
        graph6.AddEdge(5, 6, 1);
        graph6.AddEdge(6, 1, 1);
        graph6.AddEdge(1, 3, 1);

        // Perform DFS starting from vertex 1
        Console.WriteLine("DFS starting from vertex 1:");
        graph6.DFS(1);
    }

    // Test Case 7: Breadth-First Search (BFS)
    private static void TestCase7()
    {
        Console.WriteLine("Test Case 7: Breadth-First Search (BFS)");
        Console.WriteLine();
        // Create a graph and add vertices and edges
        Graph graph7 = new Graph();
        graph7.AddVertex(1);
        graph7.AddVertex(2);
        graph7.AddVertex(3);
        graph7.AddVertex(4);
        graph7.AddVertex(5);
        graph7.AddVertex(6);
        graph7.AddEdge(1, 2, 1);
        graph7.AddEdge(2, 3, 1);
        graph7.AddEdge(3, 4, 1);
        graph7.AddEdge(4, 5, 1);
        graph7.AddEdge(5, 6, 1);
        graph7.AddEdge(6, 1, 1);
        graph7.AddEdge(1, 3, 1);

        // Perform BFS starting from vertex 1
        Console.WriteLine("BFS starting from vertex 1:");
        graph7.BFS(1);
    }
}
