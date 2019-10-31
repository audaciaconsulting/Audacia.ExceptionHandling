using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Audacia.ExceptionHandling
{
	/// <summary>
	/// Used to keep a cache of types and their matched handlers so the
	/// inheritance hierarchy doesn't need to be inspected for every exception encountered.
	/// </summary>
	internal class TypeMap
	{
		private readonly ConcurrentDictionary<Type, ExceptionHandler> _maps = new ConcurrentDictionary<Type, ExceptionHandler>();

		public void Add(Type source, ExceptionHandler handler) => _maps.TryAdd(source, handler);

		public void Invalidate() => _maps.Clear();

		public ExceptionHandler Find(Type source) => _maps.TryGetValue(source, out var target) ? target : null;
	}
}