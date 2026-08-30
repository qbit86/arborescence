[assembly: System.CLSCompliant(true)]

// IBfsHandler<,,> and IDfsHandler<,,> moved to Arborescence.Abstractions so that
// Arborescence.Traversal.Specialized can constrain on them without referencing this assembly.
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(Arborescence.Traversal.IBfsHandler<,,>))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(Arborescence.Traversal.IDfsHandler<,,>))]
