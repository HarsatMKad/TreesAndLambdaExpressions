using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTrees
{
  public class BinaryTree<T> : IEnumerable<T>
      where T : IComparable
  {
    private Node root;
    public int Count { get; private set; }

    public class Node
    {
      public Node left, right;
      public T value;
    }

    public BinaryTree()
    {
      root = new Node();
    }

    public T this[int i]
    {
      get
      {
        var list = this.ToList();
        return list[i];
      }
    }

    public void Add(T key)
    {
      var current = root;

      while (true)
      {
        if (Count == 0)
        {
          current.value = key;
          Count++;
          break;
        }

        if (key.CompareTo(current.value) <= 0)
        {
          if (current.left == null)
          {
            current.left = new Node { value = key };
            Count++;
            break;
          }
          else
            current = current.left;
        }

        if (key.CompareTo(current.value) > 0)
        {
          if (current.right == null)
          {
            current.right = new Node { value = key };
            Count++;
            break;
          }
          else
            current = current.right;
        }
      }
    }

    public bool Contains(T key)
    {
      var current = root;
      while (true)
      {
        if (Count == 0)
          return false;

        if (current == null)
          return false;

        var result = key.CompareTo(current.value);

        if (result == 0)
          return true;

        if (result < 0)
        {
          current = current.left;
        }
        else if (result > 0)
        {
          current = current.right;
        }
      }
    }

    /*
    public Node Next(Node startingNode)
    {
      Node currentNode = startingNode;
      while (currentNode != null)
      {
        Node nextNode = currentNode.Next;
        if (nextNode != null)
        {
          List<Node> siblingList = currentNode.Siblings;
          siblingList.Add(nextNode);
          currentNode = nextNode;
        }
        else
        {
          break;
        }
      }
      return currentNode;
    }
    */

    public IEnumerator<T> GetEnumerator()
    {
      var stack = new Stack<Node>();

      var current = root;

      var done = false;

      while (!done)
      {
        if (current != null)
        {
          stack.Push(current);
          current = current.left;
        }

        else if (stack.Count != 0)
        {
          current = stack.Pop();

          yield return current.value;

          current = current.right;
        }

        else done = true;
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }

  class Program
  {
    static void Main(string[] args)
    {
      BinaryTree<int> tree = new BinaryTree<int>();

      tree.Add(3);
      tree.Add(10);
      tree.Add(8);
      tree.Add(1);
      tree.Add(22);
      tree.Add(5);

      for(int i = 0; i < 6; ++i)
      {
        Console.WriteLine(tree[i]);
      }

      Console.WriteLine(" ");

      foreach (int item in tree)
      {
        Console.WriteLine(item);
      }
      

      Console.ReadKey();
    }
  }
}
