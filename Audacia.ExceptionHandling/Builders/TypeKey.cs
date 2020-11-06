using System;

namespace Audacia.ExceptionHandling.Builders
{
    internal class TypeKey : IEquatable<TypeKey>
    {
        private Type ExceptionType { get; }
        private Type InheritedType { get; }

        public TypeKey(Type exceptionType, Type inheritedType = null!)
        {
            ExceptionType = exceptionType;
            InheritedType = inheritedType;
        }

        public bool Equals(TypeKey? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            var exceptionTypeEquals = ExceptionType.Equals(other.ExceptionType);

            if (InheritedType == null)
            {
                return exceptionTypeEquals;
            }

            return exceptionTypeEquals && InheritedType.Equals(other.InheritedType);
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
                var exceptionTypeValue = ExceptionType.GetHashCode() * 397;
                if (InheritedType == null)
                {
                    return exceptionTypeValue;
                }

                return exceptionTypeValue ^ InheritedType.GetHashCode();
            }
        }
    }
}