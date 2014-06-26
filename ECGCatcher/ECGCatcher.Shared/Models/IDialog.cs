using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGraphics.Models
{
    /// <summary>
    /// Dialog interface
    /// </summary>
    public interface IDialog
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is open.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is open; otherwise, <c>false</c>.
        /// </value>
        Boolean IsOpen { get; set; }
    }
    /// <summary>
    /// Dialog generic interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDialog<T> : IDialog
    {
        //Boolean IsOpen { get; set; }
        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        T Result { get; }
    }
}
