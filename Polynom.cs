using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Polynom<T> : IEnumerable, ICloneable, IComparable where T : IComparable, new()
{
	private SortedDictionary<int, T> polynom;
	public Polynom()
	{
		polynom = new SortedDictionary<int, T>();
	}
	public Polynom(T value)
	{
		polynom = new SortedDictionary<int, T>();
		polynom.Add(0, value);
	}

	public Polynom(SortedDictionary<int, T> poly)
	{
		polynom = poly;
	}

	public static Polynom<T> operator +(Polynom<T> pol1, Polynom<T> pol2) {
		var res = new Polynom<T>();
		foreach (var monom in pol1.polynom)
			res.Add(monom);
		foreach (var monom in pol2.polynom)
			res.Add(monom);
		return res;
	}

	public static Polynom<T> operator -(Polynom<T> pol1, Polynom<T> pol2)
	{
		var res = new Polynom<T>();
		foreach (var monom in pol1.polynom)
			res.Add(monom);
		foreach (var monom in pol2.polynom)
			res.Add((dynamic)monom.Value * (-1), monom.Key);
		return res;
	}

	public static Polynom<T> operator *(Polynom<T> pol1, Polynom<T> pol2)
	{
		Polynom<T> res = new Polynom<T>();
		foreach (var pair1 in pol1.polynom)
		{
			foreach (var pair2 in pol2.polynom)
			{
				res.Add((dynamic)pair1.Value * pair2.Value, pair1.Key + pair2.Key);
			}

		}
		return res;
	}
	public static Polynom<T> operator *(Polynom<T> pol1, T num)
	{
		Polynom<T> res = new Polynom<T>();
		foreach (var pair1 in pol1.polynom)
		{
			res.Add((dynamic)pair1.Value * num, pair1.Key);
		}
		return res;
	}
	public static Polynom<T> operator *(T num, Polynom<T> pol1)
	{
		Polynom<T> res = new Polynom<T>();
		foreach (var pair1 in pol1.polynom)
		{
			res.Add((dynamic)pair1.Value * num, pair1.Key);
		}
		return res;
	}

	public static Polynom<T> operator ^(Polynom<T> pol, int deg)
	{
		if (pol.polynom.Count == 0)
			throw new MyException("Bad Polynom");
		if (deg == 0)
			return new Polynom<T>((dynamic) 1);
		var res = (Polynom<T>)pol.Clone();
		for (var i = 1; i < Math.Abs(deg); i++)
			res = res * pol;
		return res;
	}

	//public static bool operator ==(Polynom<T> pol1, Polynom<T> pol2)
	//{
	//	if (pol1.polynom.Count == pol2.polynom.Count)
	//	{
	//		if (pol1.polynom.Keys.Max() == pol2.polynom.Keys.Max() && pol1.polynom.Keys.Min() == pol2.polynom.Keys.Min())
	//		{
	//			foreach (var monom1 in pol1.polynom) 
	//			{
	//				var test = new T();
	//				if (pol2.polynom.TryGetValue(monom1.Key, out test))
	//					if ((dynamic)test != monom1.Value)
	//						return false;
	//					else { }
	//				else
	//					return false;
	//			}
	//			return true;
	//		}
	//		else
	//			return false;
	//	}
	//	else
	//		return false;
	//}

	//public static bool operator !=(Polynom<T> pol1, Polynom<T> pol2)
	//{
	//	return !(pol1 == pol2);
	//}

	public Polynom<T> Composition(Polynom<T> pol) {
		var res = new Polynom<T>();
		foreach (var monom in polynom) {
			if (monom.Key != 0)
				res += monom.Value * ((dynamic)pol ^ monom.Key);
			else
				res += new Polynom<T>(monom.Value);
		}
		return res;
	}

	public T Solve(double x) {
		var res = new T();
		foreach (var pair in polynom)
			res += (dynamic)pair.Value * Math.Pow(x, pair.Key);
		return res;
	}

	public void Add(T coef, int power)
	{
		if (!coef.Equals(0))
		{
			T num;
			if (polynom.TryGetValue(power, out num))
			{
				polynom.Remove(power);
				polynom.Add(power, (dynamic)coef + num);
			}
			else
				polynom.Add(power, coef);
		}
	}
	public void Add(KeyValuePair<int, T> pair)
	{
		if (!pair.Value.Equals(0))
		{
			T num;
			if (polynom.TryGetValue(pair.Key, out num))
			{
				polynom.Remove(pair.Key);
				polynom.Add(pair.Key, (dynamic)pair.Value + num);
			}
			else
				polynom.Add(pair.Key, pair.Value);
		}
	}
	public void Add(KeyValuePair<T, int> pair)
	{
		if (!pair.Key.Equals(0))
		{
			T num;
			if (polynom.TryGetValue(pair.Value, out num))
			{
				polynom.Remove(pair.Value);
				polynom.Add(pair.Value, (dynamic)pair.Key + num);
			}
			else
				polynom.Add(pair.Value, pair.Key);
		}
	}
	public override string ToString()
	{
		if (polynom.Count == 0)
			return String.Empty;
		var sb = new StringBuilder();
		foreach (var monom in polynom)
		{
			if (sb.Length == 0)
				if (monom.Value == (dynamic)new T()){ }
				else
					if (monom.Key == 0)
						sb.Append($"{monom.Value}");
					else if (monom.Key > 0)
						sb.Append($"{monom.Value}x^{monom.Key}");
					else
						sb.Append($"{monom.Value}x^({monom.Key})");
			else
				sb.Append(StringMonom(monom));
		}
		return sb.ToString();
	}
	private string StringMonom(KeyValuePair<int, T> pair) 
	{
		var res = new StringBuilder();
		if (!pair.Value.Equals(0))
			if (pair.Value > (dynamic)new T())
				if (pair.Key == 0)
					res.Append($"+{pair.Value}");
				else if (pair.Key > 0)
					res.Append($"+{pair.Value}x^{pair.Key}");
				else
					res.Append($"+{pair.Value}x^({pair.Key})");
			else if (pair.Value < (dynamic)new T())
				if (pair.Key == 0)
					res.Append($"{pair.Value}");
				else if (pair.Key > 0)
					res.Append($"{pair.Value}x^{pair.Key}");
				else
					res.Append($"{pair.Value}x^({pair.Key})");
		return res.ToString();
	}
	public object Clone()
	{
		var res = new Polynom<T>();
		foreach (var monom in polynom)
		{
			res.Add(monom.Value, monom.Key);
		}
		return new Polynom<T>{ polynom = res.polynom };
	}

	public IEnumerator GetEnumerator()
	{
		return polynom.GetEnumerator();
	}
	public int CompareTo(Polynom<T> pol)
	{
		return polynom.Keys.Max().CompareTo(pol.polynom.Keys.Max());
	}
	int IComparable.CompareTo(object obj)
	{
		if (!(obj is Polynom<T>))
		{
			return 0;
		}
		return CompareTo((Polynom<T>)obj);
	}
}
