using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTrees
{
  public class BinaryTree<T> : IEnumerable<T> where T : IComparable<T>
  {
    public class Node<T>
    {
      public T Value { get; set; }
      public Node<T> Left { get; set; }
      public Node<T> Right { get; set; }

      public Node(T value)
      {
        Value = value;
      }
    }

    public Node<T> root;

    public BinaryTree() { }

    public int Count { get; private set; }

    public void Add(T value)
    {
      var node = new Node<T>(value);

      if (root == null)
        root = node;
      else
      {
        Node<T> current = root, parent = null;

        while (current != null)
        {
          parent = current;
          if (value.CompareTo(current.Value) < 0) current = current.Left;
          else current = current.Right;
        }

        if (value.CompareTo(parent.Value) < 0) parent.Left = node;
        else parent.Right = node;
      }
      ++Count;
    }
    public void Remove(T value)
    {
      if (value == null) throw new ArgumentNullException("value");
      if (root == null) return;

      Node<T> current = root, parent = null;

      int result;
      do
      {
        result = value.CompareTo(current.Value);
        if (result < 0) { parent = current; current = current.Left; }
        else if (result > 0) { parent = current; current = current.Right; }
        if (current == null) return;
      }
      while (result != 0);

      if (current.Right == null)
      {
        if (current == root) root = current.Left;
        else
        {
          result = current.Value.CompareTo(parent.Value);
          if (result < 0) parent.Left = current.Left;
          else parent.Right = current.Left;
        }
      }
      else if (current.Right.Left == null)
      {
        current.Right.Left = current.Left;
        if (current == root) root = current.Right;
        else
        {
          result = current.Value.CompareTo(parent.Value);
          if (result < 0) parent.Left = current.Right;
          else parent.Right = current.Right;
        }
      }
      else
      {
        Node<T> min = current.Right.Left, prev = current.Right;
        while (min.Left != null)
        {
          prev = min;
          min = min.Left;
        }
        prev.Left = min.Right;
        min.Left = current.Left;
        min.Right = current.Right;

        if (current == root) root = min;
        else
        {
          result = current.Value.CompareTo(parent.Value);
          if (result < 0) parent.Left = min;
          else parent.Right = min;
        }
      }
      --Count;
    }
    public void Clear()
    {
      root = null;
      Count = 0;
    }
    public bool Contains(T value)
    {
      var current = root;
      while (current != null)
      {
        var result = value.CompareTo(current.Value);
        if (result == 0) return true;
        if (result < 0) current = current.Left;
        else current = current.Right;
      }
      return false;
    }

    public IEnumerable<T> Inorder()
    {
      if (root == null) yield break;

      var stack = new Stack<Node<T>>();
      var node = root;

      while (stack.Count > 0 || node != null)
      {
        if (node == null)
        {
          node = stack.Pop();
          yield return node.Value;
          node = node.Right;
        }
        else
        {
          stack.Push(node);
          node = node.Left;
        }
      }
    }
    public IEnumerable<T> Preorder()
    {
      if (root == null) yield break;

      var stack = new Stack<Node<T>>();
      stack.Push(root);

      while (stack.Count > 0)
      {
        var node = stack.Pop();
        yield return node.Value;
        if (node.Right != null) stack.Push(node.Right);
        if (node.Left != null) stack.Push(node.Left);
      }
    }
    public IEnumerable<T> Postorder()
    {
      if (root == null) yield break;

      var stack = new Stack<Node<T>>();
      var node = root;

      while (stack.Count > 0 || node != null)
      {
        if (node == null)
        {
          node = stack.Pop();
          if (stack.Count > 0 && node.Right == stack.Peek())
          {
            stack.Pop();
            stack.Push(node);
            node = node.Right;
          }
          else { yield return node.Value; node = null; }
        }
        else
        {
          if (node.Right != null) stack.Push(node.Right);
          stack.Push(node);
          node = node.Left;
        }
      }
    }
    public IEnumerable<T> Levelorder()
    {
      if (root == null) yield break;

      var queue = new Queue<Node<T>>();
      queue.Enqueue(root);

      while (queue.Count > 0)
      {
        var node = queue.Dequeue();
        yield return node.Value;
        if (node.Left != null) queue.Enqueue(node.Left);
        if (node.Right != null) queue.Enqueue(node.Right);
      }
    }

    public IEnumerator<T> GetEnumerator()
    {
      return Inorder().GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public static int Print(Node<T> node, int x, int y)
    {
      Console.SetCursorPosition(x, y);
      Console.Write(node.Value);

      var loc = y;

      if (node.Right != null)
      {
        Console.SetCursorPosition(x + 2, y);
        Console.Write("--");
        y = Print(node.Right, x + 4, y);
      }

      if (node.Left != null)
      {
        while (loc <= y)
        {
          Console.SetCursorPosition(x, loc + 1);
          Console.Write(" |");
          loc++;
        }
        y = Print(node.Left, x, y + 2);
      }

      return y;
    }

    public void Next(Node<T> RootNode)
    {

    }
  }

  class Program
  {
    static void Main(string[] args)
    {
      BinaryTree<int> integerTree = new BinaryTree<int>();

      integerTree.Add(30);
      integerTree.Add(15);
      integerTree.Add(6);
      integerTree.Add(45);
      integerTree.Add(41);
      integerTree.Add(54);
      integerTree.Add(16);
      integerTree.Add(84);


      Console.WriteLine("Number of nodes is {0}", integerTree.Count);
      Console.WriteLine("Preorder traversal:");
      Console.WriteLine(string.Join(" ", integerTree.Preorder()));
      Console.WriteLine("Inorder traversal:");
      Console.WriteLine(string.Join(" ", integerTree.Inorder()));
      Console.WriteLine("Postorder traversal:");
      Console.WriteLine(string.Join(" ", integerTree.Postorder()));
      Console.WriteLine("Levelorder traversal:");
      Console.WriteLine(string.Join(" ", integerTree.Levelorder()));
      Console.WriteLine("Default traversal (inorder):");


      BinaryTree<int>.Print(integerTree.root, 10, 10);

      Console.WriteLine("");

      foreach(int item in integerTree)
      {
        Console.WriteLine(item);
      }


      Console.ReadKey();
    }
  }
}
