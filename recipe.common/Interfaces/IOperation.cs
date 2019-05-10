using System;

namespace ch.thommenmedia.common.Interfaces
{
    /// <summary>
    ///     Stellt eine nicht typisierte Operation dar.
    ///     Alle Operationen implementieren dieses Interface, somit können von überall mit einem beliebigen Input gestartet
    ///     werden.
    /// </summary>
    public interface IOperation
    {
        Type InputType { get; }
        Type OutputType { get; }

        /// <summary>
        ///     Startet die Operationen mit einem Input.
        /// </summary>
        /// <param name="input">Input-Parameter</param>
        /// <returns>Resultat</returns>
        object StartUntyped(object input);

        /// <summary>
        ///     Start die Operation ohne einen Parameter
        /// </summary>
        void StartUntyped();
    }

    /// <summary>
    ///     Stellt eine typisierte Operation dar. Es kann der Input sowie der Output definiert werden.
    /// </summary>
    /// <typeparam name="TResult">Typ des Outputs (Result)</typeparam>
    /// <typeparam name="TInput">Typ des Inputs</typeparam>
    public interface IOperation<in TInput, out TResult> : IOperation
    {
        /// <summary>
        ///     Startet die Operation mit den definierten Input.
        /// </summary>
        /// <returns>Resultat</returns>
        TResult Start(TInput input);
    }
}