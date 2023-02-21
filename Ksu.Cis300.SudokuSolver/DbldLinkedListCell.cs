using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksu.Cis300.SudokuSolver
{
    /// <summary>
    /// A class that functions as a doubled linked list
    /// </summary>
    internal class DbldLinkedListCell
    {
        /// <summary>
        /// The Data Property of a Doubled Linked list
        /// </summary>
        public int Data { get; set; }

        /// <summary>
        /// The refrence to the cell preceding this one
        /// </summary>
        public DbldLinkedListCell Prev { get; set; }

        /// <summary>
        /// the refrence to the cell after this one
        /// </summary>
        public DbldLinkedListCell Next { get; set; }


        /// <summary>
        /// initiilzes the next and previous cells to its self
        /// to be modified later with user code
        /// </summary>
        public DbldLinkedListCell()
        {
            this.Next = this;
            this.Prev = this;
        }
    }
}
