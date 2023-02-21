namespace Ksu.Cis300.SudokuSolver
{
    partial class uxSudoku
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.uxFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.puzzleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ux4x4 = new System.Windows.Forms.ToolStripMenuItem();
            this.ux9x9 = new System.Windows.Forms.ToolStripMenuItem();
            this.uxSolve = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uxFlowPanel
            // 
            this.uxFlowPanel.AutoSize = true;
            this.uxFlowPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.uxFlowPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.uxFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.uxFlowPanel.Location = new System.Drawing.Point(0, 36);
            this.uxFlowPanel.MinimumSize = new System.Drawing.Size(50, 50);
            this.uxFlowPanel.Name = "uxFlowPanel";
            this.uxFlowPanel.Size = new System.Drawing.Size(50, 50);
            this.uxFlowPanel.TabIndex = 0;
            this.uxFlowPanel.WrapContents = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.puzzleToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 33);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // puzzleToolStripMenuItem
            // 
            this.puzzleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ux4x4,
            this.ux9x9,
            this.uxSolve});
            this.puzzleToolStripMenuItem.Name = "puzzleToolStripMenuItem";
            this.puzzleToolStripMenuItem.Size = new System.Drawing.Size(77, 29);
            this.puzzleToolStripMenuItem.Text = "Puzzle";
            // 
            // ux4x4
            // 
            this.ux4x4.Name = "ux4x4";
            this.ux4x4.Size = new System.Drawing.Size(270, 34);
            this.ux4x4.Text = "4x4";
            this.ux4x4.Click += new System.EventHandler(this.ux4x4_Click);
            // 
            // ux9x9
            // 
            this.ux9x9.Name = "ux9x9";
            this.ux9x9.Size = new System.Drawing.Size(270, 34);
            this.ux9x9.Text = "9x9";
            this.ux9x9.Click += new System.EventHandler(this.ux9x9_Click);
            // 
            // uxSolve
            // 
            this.uxSolve.Name = "uxSolve";
            this.uxSolve.Size = new System.Drawing.Size(270, 34);
            this.uxSolve.Text = "Solve";
            this.uxSolve.Click += new System.EventHandler(this.uxSolve_Click);
            // 
            // uxSudoku
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.uxFlowPanel);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "uxSudoku";
            this.Text = "Sudoku";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel uxFlowPanel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem puzzleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ux4x4;
        private System.Windows.Forms.ToolStripMenuItem ux9x9;
        private System.Windows.Forms.ToolStripMenuItem uxSolve;
    }
}

