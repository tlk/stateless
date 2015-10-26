namespace Stateless
{
    /// <summary>
    /// Transform a StateMachine configuration into a string representation.
    /// </summary>
    public interface IStringFormatter
    {
        /// <summary>
        /// Add a transition.
        /// </summary>
        /// <param name="source">The source state.</param>
        /// <param name="trigger">The trigger.</param>
        /// <param name="guardMethodName">The method name of the guard function.</param>
        /// <param name="guardMethodNamespace">The name space of the guard function.</param>
        /// <param name="destination">The destination state.</param>
        /// <returns>A configuration object through which the state can be configured.</returns>
        void Add(string source, string trigger, string guardMethodName, string guardMethodNamespace, string destination);

        /// <summary>
        /// String representation.
        /// </summary>
        string ToString();
    }
}
