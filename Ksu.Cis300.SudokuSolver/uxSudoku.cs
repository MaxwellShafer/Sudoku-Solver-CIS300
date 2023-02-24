using System;
using System.CodeDom.Compiler;
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

        private const int _block = 3;

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
            char c = '0';
            string text = box.Text;
            if (box.Text.Length == 1)
            {
               c = box.Text[0];
            }

            if (box.Text == "")
            {
                box.Text = "";
                _puzzle[row, column] = 0;
            }
            else if (box.Text.Length == 1 && (c > '0' && c <= (_puzzle.GetLength(1) + '0')))
            {
                int num = Convert.ToInt32(box.Text);
                _puzzle[row, column] = num;
            }
            else if(box.Text == "0")
            {
                box.Text = "";
                _puzzle[row, column] = 0;
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
            _textBoxes = new TextBox[size*size, size*size];
            _puzzle = new int[size*size, size*size];
            TextBox box;

            for (int i = 0; i < size* size; i++)
            {
                for (int j = 0; j < size* size; j++)
                {
                    box = new TextBox();
                    box.Font = _cellFont;
                    box.Width = box.Height; // check here *********
                    box.Margin = new Padding(0);
                    box.TextAlign = HorizontalAlignment.Center;
                    box.Name = i.ToString() + j.ToString();
                    box.TextChanged += new EventHandler(CellTextChanged);

                    _textBoxes[i, j] = box;
                    uxFlowPanel.Controls[i/size].Controls[j/size].Controls.Add(box);
                }


            }
        }

        private void ResizePannels(int size)
        {
            foreach(Control c in uxFlowPanel.Controls)
            {
                foreach(Control c1 in c.Controls)
                {
                    c1.Size = new Size(size * c1.Controls[0].Height, size * c1.Controls[0].Height);
                   
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
            uxFlowPanel.Enabled = true;
            NewPuzzle(_smallBlock);
        }

        private void ux9x9_Click(object sender, EventArgs e)
        {
            uxFlowPanel.Enabled = true;
            NewPuzzle(_block);
        }

        private void PlaceSolution()
        {
            for (int i = 0; i < _puzzle.GetLength(1); i++)
            {
                for (int j = 0; j < _puzzle.GetLength(1); j++)
                {
                    
                    
                    if(_textBoxes[i, j].Text == "")
                    {
                        _textBoxes[i, j].Font = _valueFont;
                    }
                    _textBoxes[i, j].Text = _puzzle[i, j].ToString();
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
