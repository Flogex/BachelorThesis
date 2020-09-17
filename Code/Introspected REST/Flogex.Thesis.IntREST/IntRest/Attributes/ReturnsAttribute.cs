using System;

namespace Flogex.Thesis.IntRest.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ReturnsAttribute : Attribute
    {
        public ReturnsAttribute(Type returnType)
        {
            this.ReturnType = returnType;
        }

        public Type ReturnType { get; }
    }
}
