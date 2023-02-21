using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ksu.Cis300.SudokuSolver
{
    public partial class uxSudoku : Form
    {
        private const int _smallBlock = 2;

        private const int _block = 9;

        private const int _margin = 1;

        private const int _fontSize = 18;

        readonly Font _cellFont = new Font(FontFamily.GenericSerif, _fontSize, FontStyle.Bold);

        readonly Font _valueFont = new Font(FontFamily.GenericSerif, _fontSize);

        private TextBox[,] _textBoxes;

        private int[,] _puzzle;

        public uxSudoku()
        {
            InitializeComponent();
            NewPuzzle(_block);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void CellTextChanged(object sender, EventArgs e)
        {
            TextBox box = (TextBox)sender;
            
            int row = box.Name[0] - '0';
            int column = box.Name[1] - '0';

            string text = box.Text;
            if (box.Text == "")
            {
                box.Text = "";
            }
            else if (box.Text.Length == 1 && (text == "1" || text == "2" || text == "3" || text == "4" || text == "5" || text == "6" || text == "7" || text == "8" || text == "9"))
            {
                int num = Convert.ToInt32(box.Text);
                _puzzle[row, column] = num;
            }
            else if(box.Text == "0")
            {
                box.Text = "";
            }
            else
            {
                box.Text = _puzzle[row, column].ToString();
            }
               
            
        }

        private void AddPanels(int size)
        {
            uxFlowPanel.Controls.Clear();
            for( int i = 0; i < size; i++)
            {

                FlowLayoutPanel panel = new FlowLayoutPanel();
                panel.AutoSize = true;
                panel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                panel.WrapContents = false;
                panel.Margin = new Padding(0);

                uxFlowPanel.Controls.Add(panel);

                for (int j = 0; j < size; j++)
                {
                    FlowLayoutPanel innerPanel = new FlowLayoutPanel();
                    innerPanel.Margin = new Padding(_margin);
                    panel.Controls.Add(innerPanel);
                }   

            }
        }

        private void AddTextBoxes(int size)
        {
            _textBoxes = new TextBox[size, size];
            _puzzle = new int[size, size];
            TextBox box;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j< size; j++)
                {
                    box = new TextBox();
                    box.Font = _cellFont;
                    box.Width = _fontSize;
                    box.Margin = new Padding(0);
                    box.TextAlign = HorizontalAlignment.Center;
                    box.Name = (i/size).ToString() + (j/size).ToString();
                    box.TextChanged += new EventHandler(CellTextChanged);

                    _textBoxes[i, j] = box;
                    uxFlowPanel.Controls[i].Controls[j].Controls.Add(box);
                }


            }
        }

        private void ResizePannels(int size)
        {
            foreach(Control c in uxFlowPanel.Controls)
            {
                foreach(Control c1 in c.Controls)
                {
                    c1.Controls[0].Size = new Size(size * size, size * size);
                }
            }
        }

        private void NewPuzzle(int size)
        {
            uxFlowPanel.Visible = false;

            AddPanels(size);
            AddTextBoxes(size);
            ResizePannels(size);

            uxSolve.Enabled = true;
            uxFlowPanel.Visible = true;


        }

        private void ux4x4_Click(object sender, EventArgs e)
        {
            NewPuzzle(_smallBlock);
        }

        private void ux9x9_Click(object sender, EventArgs e)
        {
            NewPuzzle(_block);
        }

        private void PlaceSolution()
        {
            for (int i = 0; i < _puzzle.GetLength(1); i++)
            {
                for (int j = 0; j < _puzzle.GetLength(1); j++)
                {
                    _textBoxes[i, j].Text = _puzzle[i, j].ToString();
                    _textBoxes[i, j].Font = _valueFont;
                }
            }
        }

        private void uxSolve_Click(object sender, EventArgs e)
        {
            if (!Solver.Solve(_puzzle))
            {
                MessageBox.Show("No Solution");
            }
            else
            {
                PlaceSolution();
            }

            uxFlowPanel.Enabled = false;
            uxSolve.Enabled = false;
        }
    }
}
