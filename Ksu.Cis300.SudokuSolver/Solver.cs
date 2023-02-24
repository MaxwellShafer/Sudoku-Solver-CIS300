/* Solver.cs
 * Author: Max Shafer
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ksu.Cis300.SudokuSolver
{
    /// <summary>
    /// a class to solve sudoku puzzles
    /// </summary>
    internal static class Solver
    {
        /// <summary>
        /// a 2D array to store the puzzle
        /// </summary>
        private static int[,] _puzzle;

        /// <summary>
        /// an int storing the number of rows either 2 or 3
        /// </summary>
        private static int _numRows;

        /// <summary>
        /// an int storing the number of columns either 4 or 9
        /// </summary>
        private static int _numColumns;

        /// <summary>
        /// an array storing the used values in each row
        /// </summary>
        private static bool[,] _usedRowValues;

        /// <summary>
        /// an array storing the used values in a block
        /// </summary>
        private static bool[,,] _usedBlockValues;

        /// <summary>
        /// A dbld linked list storing the empty cells in a (each?) row
        /// The data property of each cell should corrispond with its column
        /// </summary>
        private static DbldLinkedListCell[] _emptyCellsInRow;

        /// <summary>
        /// a dbld linked list storing the unused values in a row
        /// </summary>
        private static DbldLinkedListCell[] _unusedValuesInRow;

        /// <summary>
        /// an int showing the current row being filled
        /// </summary>
        private static int _currentRow;

        /// <summary>
        /// A stack containg the cells removed from list of unused values
        /// </summary>
        private static Stack<DbldLinkedListCell> _valueStack;

        /// <summary>
        /// reffering to a dbld linked list cell with the next Loc to be filled from empty cells
        /// </summary>
        private static DbldLinkedListCell _nextLocToFill;

        /// <summary>
        ///  refers to a dbld linked list cell with the next value to try
        /// </summary>
        private static DbldLinkedListCell _nextValue;




        /// <summary>
        /// Removes a cell from a list
        /// </summary>
        /// <param name="cell"> The cell to be removed </param>
        private static void RemoveCell(DbldLinkedListCell cell)
        {
            cell.Next.Prev = cell.Prev; /// sets the Next cell's Prev to match 
            cell.Prev.Next = cell.Next; /// sets the cell before's Next property to match

        }

        /// <summary>
        /// Re-refrences current cell to the prev and next cells
        /// </summary>
        /// <param name="cell"></param>
        private static void RestoreCell(DbldLinkedListCell cell)
        {
            cell.Next.Prev = cell;
            cell.Prev.Next = cell;
        }

        /// <summary>
        /// A method to insert a cell into a liked list
        /// </summary>
        /// <param name="newCell">The cell being inserted</param>
        /// <param name="cellAfter">The cell following the inserted cell</param>
        private static void InsertCell(DbldLinkedListCell newCell, DbldLinkedListCell cellAfter)
        {
            cellAfter.Prev.Next = newCell;

            newCell.Prev = cellAfter.Prev;
            newCell.Next = cellAfter;

            cellAfter.Prev = newCell;
        }

        /// <summary>
        ///  loops through and generates a linked list that includes numbers 1-9
        /// </summary>
        /// <returns>returns a header cell of a linked list that includes numbers 1-9</returns>
        private static DbldLinkedListCell getPuzzleValues()
        {
            DbldLinkedListCell headerCell = new DbldLinkedListCell();
            DbldLinkedListCell front = headerCell;


            for (int i = 1; i <= _numColumns; i++)
            {
                DbldLinkedListCell current = new DbldLinkedListCell();
                current.Data = i;
                current.Prev = front;
                front.Next = current;
                front = current;
            }

            front.Next = headerCell;    //loop  back to header
            headerCell.Prev = front;

            return headerCell;
        }

        /// <summary>
        /// loops through a linked list to remove an int
        /// </summary>
        /// <param name="header">the header cell of the linked list </param>
        /// <param name="num"> the number to be removed</param>
        /// <returns> a bool whether it sucessfully removed the int</returns>
        private static bool RemovePuzzleValue(DbldLinkedListCell header, int value)
        {
            DbldLinkedListCell front = header.Next; //start at the first data cell

            while (front != header)
            {
                if (front.Data == value)
                {
                    RemoveCell(front);
                    return true;
                }
                else
                {
                    front = front.Next;
                }
            }
            return false;

        }

        /// <summary>
        /// records if a value is in a block or row and updated the boolean arrays 
        /// Note: the value is corrisponeded with is position in the array 
        /// so the 8th cell being true means 8 is present in the cell or block
        /// ****************************
        /// NOTE: To get the indices identifying the
        /// block, divide and round up row and column by the 
        /// number of rows/values in a block
        /// ****************************
        /// </summary>
        /// <param name="row">the row</param>
        /// <param name="column">the column</param>
        /// <param name="value">the value</param>
        /// <param name="isThere">if it is there or not</param>
        private static void RecordValue(int row, int column, int value, bool isThere)
        {
            _usedBlockValues[row / _numRows, column / _numRows, value] = isThere; // devide to find indicies
            _usedRowValues[row, value] = isThere;
        }


        /// <summary>
        /// checks to see if you can place a value at a location- checks column and block
        /// </summary>
        /// <param name="row">the row</param>
        /// <param name="column">the column</param>
        /// <param name="value">the value</param>
        /// <returns>returns if you can place it or not</returns>
        private static bool CanPlace(int row, int column, int value)
        {
            if (value == 0)
            {
                return true;
            }
            if (_usedBlockValues[row / _numRows, column / _numRows, value])     // is it in same block?
            {
                return false;
            }
            for (int i = 0; i < _numColumns; i++)              //look at each row, chek
            {
                if (_puzzle[i, column] == value && i != row)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// supposed to initilize a the array of empty spaces by adding
        /// them to a liknked list, the cells that are nonempty get added 
        /// to the linked list of used numbers for that row
        /// </summary>
        /// <param name="row">the row initilized</param>
        /// <returns>return a bool if it worked/ no conflicr</returns>
        private static bool initRow(int row)
        {
            DbldLinkedListCell header = new DbldLinkedListCell();
            header.Data = -1;                                           //**************
            DbldLinkedListCell front = header;

            DbldLinkedListCell unusedHeader = getPuzzleValues();



            for (int i = 0; i < _numColumns; i++)
            {
                if (_puzzle[row, i] == 0)    // if there is notihing stored there creat new empty cell
                {
                    DbldLinkedListCell current = new DbldLinkedListCell();
                    current.Data = i;   // check if needs to be 0
                    current.Prev = front;
                    front.Next = current;
                    front = current;
                }   // ****check logic here later V ********
                else if (CanPlace(row, i, _puzzle[row, i]))   // if you cant remove the element return false
                {
                    if (RemovePuzzleValue(unusedHeader, _puzzle[row, i]))
                    {
                        RecordValue(row, i, _puzzle[row, i], true);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;

                }

            }

            front.Next = header;
            header.Prev = front;

            _emptyCellsInRow[row] = header;

            _unusedValuesInRow[row] = unusedHeader;
            return true;
        }

        /// <summary>
        /// initilizes all but the last 2 fields, 
        /// </summary>
        /// <param name="puzzle">the puzzle</param>
        /// <returns>returns if it worked or not depeding if there were init issues</returns>
        private static bool initFields(int[,] puzzle)
        {

            _puzzle = puzzle;
            _numRows = (int)Math.Sqrt(puzzle.GetLength(1)); // check 3 and 2 vs 4 and 9
            _numColumns = puzzle.GetLength(1);         // check later
            _usedRowValues = new bool[_numColumns, _numColumns + 1];          // suppose to be one bigger?
            _usedBlockValues = new bool[_numRows, _numRows, _numColumns + 1]; // check size here 3x3x10 for a 9x9 sect. 5.2
            _emptyCellsInRow = new DbldLinkedListCell[_numColumns];
            _unusedValuesInRow = new DbldLinkedListCell[_numColumns];
            _currentRow = 0;
            _valueStack = new Stack<DbldLinkedListCell>();

            for (int i = 0; i < _numColumns; i++)
            {
                if (!initRow(i))
                {
                    return false;
                }
            }

            return true;


        }

        /// <summary>
        /// moves current row to next if there is one
        /// also responsable for setting nextLocToFill
        /// and nextValue
        /// </summary>
        /// <returns>return</returns>
        private static bool NextRow()
        {
            if (_currentRow + 1 < _numColumns)                       // check if there is a next row??
            {
                _currentRow++;
                _nextLocToFill = _emptyCellsInRow[_currentRow].Next; //.Next so its not on the header cell?
                _nextValue = _unusedValuesInRow[_currentRow].Next;   //same^
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// a method to backtrack throug hthe numbers
        /// </summary>
        /// <returns></returns>
        private static bool Backtrack() 
        {


            while (_nextLocToFill != _emptyCellsInRow[_currentRow])
            {
                // restore the nuber to unused list
                _nextLocToFill = _nextLocToFill.Prev;

                if (_nextLocToFill.Data == -1) //check later  //if its a header cell 
                {
                    if (_currentRow == 0)    // make sure it is not last row
                    {
                        return false;
                    }

                    _currentRow--;                                       // go back a row
                    _nextLocToFill = _emptyCellsInRow[_currentRow].Prev; // sets next loc to fill to last cell of prev row
                                                                         // sets next value to biggest 
                }

                DbldLinkedListCell numRemoved = (_valueStack.Pop());

                RestoreCell(numRemoved);

                RecordValue(_currentRow, _nextLocToFill.Data, _puzzle[_currentRow, _nextLocToFill.Data], false); //update puzzle in bool tables
                _puzzle[_currentRow, _nextLocToFill.Data] = 0; // update table

                if (numRemoved.Data < _unusedValuesInRow[_currentRow].Prev.Data)
                {
                    _nextValue = numRemoved.Next; // set to value after restored value, ie in row 2 ("index 1") we just restored 3, and now it should be set to 7
                    return true;
                }


            }
            return false;

        }

        /// <summary>
        /// Tries to use the next value
        /// </summary>
        private static void UseNextValue()
        {
            _valueStack.Push(_nextValue);
            RemoveCell(_nextValue);

            _puzzle[_currentRow, _nextLocToFill.Data] = _nextValue.Data;
            RecordValue(_currentRow, _nextLocToFill.Data, _nextValue.Data, true);
            _nextValue = _unusedValuesInRow[_currentRow].Next;
            _nextLocToFill = _nextLocToFill.Next;

        }

        /// <summary>
        /// tries to solve the puzzle
        /// </summary>
        /// <param name="puzzle">the puzzle</param>
        /// <returns>if it worked or not</returns>
        public static bool Solve(int[,] puzzle)
        {
            if (!initFields(puzzle))
            {
                return false;
            }
            else
            {
                _currentRow = -1;
                NextRow();
            }

            while (true)
            {

                if (CanPlace(_currentRow, _nextLocToFill.Data, _nextValue.Data))
                {
                    if (_nextLocToFill.Data == -1) // if the location is the header
                    {
                        if (!NextRow())
                        {
                            return true;
                            /*program has been solved*/
                        }

                    }
                    else if (_nextValue.Data == 0 && _nextValue.Prev.Data != 0) //if no values left?
                    {
                        if (!Backtrack())
                        {
                            return false;
                            /*there is no solution*/
                        }
                    }
                    else
                    {
                        UseNextValue();
                    }
                }
                else
                {
                    _nextValue = _nextValue.Next;
                }


            }




        }






    }
}
