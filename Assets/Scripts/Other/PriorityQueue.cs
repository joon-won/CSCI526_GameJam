using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T> {

    private List<Tuple<T, int>> items = new List<Tuple<T, int>>();

    public int Count => items.Count;

    public void Enqueue(T item, int priority) {
        items.Add(new Tuple<T, int>(item, priority));
        items.Sort((x, y) => x.Item2.CompareTo(y.Item2));
    }

    public T Dequeue() {
        if (items.Count == 0) {
            throw new InvalidOperationException("Queue is empty.");
        }

        T item = items[0].Item1;
        items.RemoveAt(0);
        return item;
    }

    public void Remove(T item) {
        for (int i = 0; i < Count; i++) {
            var tuple = items[i];
            if (EqualityComparer<T>.Default.Equals(tuple.Item1, item)) {
                items.RemoveAt(i);
                return;
            }
        }
    }

    public bool Contains(T item) {
        foreach (var tuple in items) {
            if (EqualityComparer<T>.Default.Equals(tuple.Item1, item))
                return true;
        }
        return false;
    }
}

