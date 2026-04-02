using System;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T> where T : IComparable<T>
{
    private readonly List<T> _items = new List<T>();

    /// <summary>
    /// Gets the number of items currently stored in the queue.
    /// </summary>
    public int Count => _items.Count;

    /// <summary>
    /// Adds a new item to the queue.
    /// The item is placed in the correct position so the smallest item stays at the top.
    /// </summary>
    public void Enqueue(T item)
    {
        _items.Add(item);

        // Bubble the new item up until the heap property is restored.
        int childIndex = _items.Count - 1;

        while (childIndex > 0)
        {
            int parentIndex = (childIndex - 1) / 2;

            // If the child is larger than or equal to the parent, the heap is valid.
            if (_items[childIndex].CompareTo(_items[parentIndex]) >= 0)
                break;

            Swap(childIndex, parentIndex);
            childIndex = parentIndex;
        }
    }

    /// <summary>
    /// Removes and returns the smallest item in the queue.
    /// </summary>
    public T Dequeue()
    {
        if (_items.Count == 0)
            throw new InvalidOperationException("The priority queue is empty.");

        // The front of the heap always contains the smallest item.
        T frontItem = _items[0];

        int lastIndex = _items.Count - 1;

        // Move the last item to the front.
        _items[0] = _items[lastIndex];
        _items.RemoveAt(lastIndex);

        // If the queue is now empty, we are done.
        if (_items.Count == 0)
            return frontItem;

        // Push the new root down until the heap property is restored.
        int parentIndex = 0;

        while (true)
        {
            int leftChildIndex = parentIndex * 2 + 1;
            if (leftChildIndex >= _items.Count)
                break;

            int rightChildIndex = leftChildIndex + 1;
            int smallestChildIndex = leftChildIndex;

            // Use the smaller of the two children.
            if (rightChildIndex < _items.Count &&
                _items[rightChildIndex].CompareTo(_items[leftChildIndex]) < 0)
            {
                smallestChildIndex = rightChildIndex;
            }

            // If the parent is already smaller than both children, the heap is valid.
            if (_items[parentIndex].CompareTo(_items[smallestChildIndex]) <= 0)
                break;

            Swap(parentIndex, smallestChildIndex);
            parentIndex = smallestChildIndex;
        }

        return frontItem;
    }

    /// <summary>
    /// Swaps two items in the underlying list.
    /// </summary>
    private void Swap(int firstIndex, int secondIndex)
    {
        T temp = _items[firstIndex];
        _items[firstIndex] = _items[secondIndex];
        _items[secondIndex] = temp;
    }
}
