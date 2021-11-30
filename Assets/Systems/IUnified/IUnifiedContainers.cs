using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.IUnified;

public class IUnifiedContainers<TContainer, TInterface> : IList<TInterface>
    where TInterface : class
    where TContainer : IUnifiedContainer<TInterface>, new()
{
    private readonly Func<IList<TContainer>> _getList;

    public IUnifiedContainers(Func<IList<TContainer>> getList)
    {
        _getList = getList;
    }

    public int Count { get { return _getList().Count; } }

    public bool IsReadOnly { get { return _getList().IsReadOnly; } }

    public IEnumerator<TInterface> GetEnumerator()
    {
        return _getList().Select(c => c == null ? null : c.Result).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(TInterface item)
    {
        _getList().Add(new TContainer { Result = item });
    }

    public void Clear()
    {
        _getList().Clear();
    }

    public bool Contains(TInterface item)
    {
        return IndexOf(_getList(), item) >= 0;
    }

    public void CopyTo(TInterface[] array, int arrayIndex)
    {
        var list = _getList().Select(c => c == null ? null : c.Result).ToList();
        Array.Copy(list.ToArray(), 0, array, arrayIndex, list.Count);
    }

    public bool Remove(TInterface item)
    {
        var list = _getList();
        var indexToRemove = IndexOf(list, item);
        if(indexToRemove < 0)
        {
            return false;
        }

        list.RemoveAt(indexToRemove);
        return true;
    }

    public int IndexOf(TInterface item)
    {
        return IndexOf(_getList(), item);
    }

    public void Insert(int index, TInterface item)
    {
        _getList().Insert(index, new TContainer { Result = item });
    }

    public void RemoveAt(int index)
    {
        _getList().RemoveAt(index);
    }

    public TInterface this[int index]
    {
        get
        {
            var container = _getList()[index];
            return container == null ? null : container.Result;
        }
        set { _getList()[index] = new TContainer { Result = value }; }
    }

    private static int IndexOf(IList<TContainer> list, TInterface item)
    {
        return list.FirstIndexWhere(c =>
        {
            if(item == null)
            {
                return c == null || c.Result == null;
            }
            return c != null && c.Result == item;
        });
    }
}