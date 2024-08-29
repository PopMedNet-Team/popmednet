package net.shrine.config;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Iterator;
import java.util.List;
import java.util.ListIterator;

import javax.xml.bind.annotation.XmlElement;

public class LocalKeys implements List<String>
{
	@XmlElement(name = "local_key", required = true)
	private ArrayList<String> keys = new ArrayList<String>();

	private LocalKeys(){ /*for JAXB*/ }
	
	public LocalKeys(String key)
	{
		keys.add(key);
	}
	
	public boolean add(String key)
	{
		return keys.add(key);
	}

	public void add(int index, String element)
	{
		keys.add(index, element);
	}

	public boolean addAll(Collection<? extends String> c)
	{
		return keys.addAll(c);
	}

	public boolean addAll(int index, Collection<? extends String> c)
	{
		return keys.addAll(index, c);
	}

	public void clear()
	{
		keys.clear();
	}

	public boolean contains(Object o)
	{
		return keys.contains(o);
	}

	public boolean containsAll(Collection<?> c)
	{
		return keys.containsAll(c);
	}

	public String get(int index)
	{
		return keys.get(index);
	}

	public int indexOf(Object o)
	{
		return keys.indexOf(o);
	}

	public boolean isEmpty()
	{
		return keys.isEmpty();
	}

	public Iterator<String> iterator()
	{
		return keys.iterator();
	}

	public int lastIndexOf(Object o)
	{
		return keys.lastIndexOf(o);
	}

	public ListIterator<String> listIterator()
	{
		return keys.listIterator();
	}

	public ListIterator<String> listIterator(int index)
	{
		return keys.listIterator(index);
	}

	public boolean remove(Object o)
	{
		return keys.remove(o);
	}

	public String remove(int index)
	{
		return keys.remove(index);
	}

	public boolean removeAll(Collection<?> c)
	{
		return keys.removeAll(c);
	}

	public boolean retainAll(Collection<?> c)
	{
		return keys.retainAll(c);
	}

	public String set(int index, String element)
	{
		return keys.set(index, element);
	}

	public int size()
	{
		return keys.size();
	}

	public List<String> subList(int fromIndex, int toIndex)
	{
		return keys.subList(fromIndex, toIndex);
	}

	public Object[] toArray()
	{
		return keys.toArray();
	}

	public <T> T[] toArray(T[] a)
	{
		return keys.toArray(a);
	}

}
	