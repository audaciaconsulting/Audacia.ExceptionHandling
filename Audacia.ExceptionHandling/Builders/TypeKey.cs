using System;

namespace Audacia.ExceptionHandling.Builders
{
    internal class TypeKey : IEquatable<TypeKey>
    {
        private Type ExceptionType { get; }
        private Type InheritedType { get; }

        public TypeKey(Type exceptionType, Type inheritedType = null)
        {
            ExceptionType = exceptionType;
            InheritedType = inheritedType;
        }

        public bool Equals(TypeKey? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ExceptionType.Equals(other.ExceptionType) && InheritedType.Equals(other.InheritedType);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TypeKey) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (ExceptionType.GetHashCode() * 397) ^ InheritedType.GetHashCode();
            }
        }
    }
}