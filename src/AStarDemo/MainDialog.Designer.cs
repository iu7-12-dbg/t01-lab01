using System.Drawing;
using System.Windows.Forms;

namespace AStarDemo
{
    partial class MainDialog
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
            this.scToolPanel = new System.Windows.Forms.SplitContainer();
            this.btnObject = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUndo = new System.Windows.Forms.Button();
            this.pbDrawingSurface = new System.Windows.Forms.PictureBox();
            this.scFooter = new System.Windows.Forms.SplitContainer();
            this.lStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.scToolPanel)).BeginInit();
            this.scToolPanel.Panel1.SuspendLayout();
            this.scToolPanel.Panel2.SuspendLayout();
            this.scToolPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbDrawingSurface)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scFooter)).BeginInit();
            this.scFooter.Panel1.SuspendLayout();
            this.scFooter.Panel2.SuspendLayout();
            this.scFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // scToolPanel
            // 
            this.scToolPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scToolPanel.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scToolPanel.IsSplitterFixed = true;
            this.scToolPanel.Location = new System.Drawing.Point(0, 0);
            this.scToolPanel.Name = "scToolPanel";
            this.scToolPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scToolPanel.Panel1
            // 
            this.scToolPanel.Panel1.Controls.Add(this.btnObject);
            this.scToolPanel.Panel1.Controls.Add(this.btnSelect);
            this.scToolPanel.Panel1.Controls.Add(this.btnAdd);
            this.scToolPanel.Panel1.Controls.Add(this.btnUndo);
            this.scToolPanel.Panel1.Margin = new System.Windows.Forms.Padding(4);
            this.scToolPanel.Panel1.Padding = new System.Windows.Forms.Padding(2);
            this.scToolPanel.Panel1MinSize = 24;
            // 
            // scToolPanel.Panel2
            // 
            this.scToolPanel.Panel2.Controls.Add(this.pbDrawingSurface);
            this.scToolPanel.Panel2.Padding = new System.Windows.Forms.Padding(2);
            this.scToolPanel.Panel2MinSize = 240;
            this.scToolPanel.Size = new System.Drawing.Size(624, 419);
            this.scToolPanel.SplitterDistance = 25;
            this.scToolPanel.SplitterWidth = 1;
            this.scToolPanel.TabIndex = 1;
            // 
            // btnObject
            // 
            this.btnObject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(185)))), ((int)(((byte)(185)))));
            this.btnObject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnObject.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World, ((byte)(204)));
            this.btnObject.Location = new System.Drawing.Point(48, 4);
            this.btnObject.Name = "btnObject";
            this.btnObject.Size = new System.Drawing.Size(21, 21);
            this.btnObject.TabIndex = 31;
            this.btnObject.Text = "O";
            this.btnObject.UseVisualStyleBackColor = false;
            // 
            // btnSelect
            // 
            this.btnSelect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(185)))), ((int)(((byte)(185)))));
            this.btnSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World, ((byte)(204)));
            this.btnSelect.Location = new System.Drawing.Point(25, 4);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(21, 21);
            this.btnSelect.TabIndex = 30;
            this.btnSelect.Text = "S";
            this.btnSelect.UseVisualStyleBackColor = false;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(185)))), ((int)(((byte)(185)))));
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World, ((byte)(204)));
            this.btnAdd.Location = new System.Drawing.Point(2, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(21, 21);
            this.btnAdd.TabIndex = 29;
            this.btnAdd.Text = "A";
            this.btnAdd.UseVisualStyleBackColor = false;
            // 
            // btnUndo
            // 
            this.btnUndo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World, ((byte)(204)));
            this.btnUndo.Location = new System.Drawing.Point(75, 3);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(50, 23);
            this.btnUndo.TabIndex = 28;
            this.btnUndo.Text = "Undo";
            this.btnUndo.UseVisualStyleBackColor = true;
            // 
            // pbDrawingSurface
            // 
            this.pbDrawingSurface.BackColor = System.Drawing.Color.White;
            this.pbDrawingSurface.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbDrawingSurface.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbDrawingSurface.Location = new System.Drawing.Point(2, 2);
            this.pbDrawingSurface.Name = "pbDrawingSurface";
            this.pbDrawingSurface.Size = new System.Drawing.Size(620, 389);
            this.pbDrawingSurface.TabIndex = 0;
            this.pbDrawingSurface.TabStop = false;
            this.pbDrawingSurface.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbDrawingSurface_MouseDown);
            this.pbDrawingSurface.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbDrawingSurface_MouseMove);
            this.pbDrawingSurface.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbDrawingSurface_MouseUp);
            // 
            // scFooter
            // 
            this.scFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scFooter.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.scFooter.IsSplitterFixed = true;
            this.scFooter.Location = new System.Drawing.Point(0, 0);
            this.scFooter.Name = "scFooter";
            this.scFooter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scFooter.Panel1
            // 
            this.scFooter.Panel1.Controls.Add(this.scToolPanel);
            this.scFooter.Panel1MinSize = 160;
            // 
            // scFooter.Panel2
            // 
            this.scFooter.Panel2.Controls.Add(this.lStatus);
            this.scFooter.Panel2MinSize = 24;
            this.scFooter.Size = new System.Drawing.Size(624, 445);
            this.scFooter.SplitterDistance = 419;
            this.scFooter.SplitterWidth = 1;
            this.scFooter.TabIndex = 3;
            // 
            // lStatus
            // 
            this.lStatus.AutoSize = true;
            this.lStatus.Location = new System.Drawing.Point(3, 6);
            this.lStatus.Name = "lStatus";
            this.lStatus.Size = new System.Drawing.Size(47, 13);
            this.lStatus.TabIndex = 4;
            this.lStatus.Text = "<status>";
            this.lStatus.Visible = false;
            // 
            // MainDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 445);
            this.Controls.Add(this.scFooter);
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "MainDialog";
            this.Text = "A* pathfinding";
            this.scToolPanel.Panel1.ResumeLayout(false);
            this.scToolPanel.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scToolPanel)).EndInit();
            this.scToolPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbDrawingSurface)).EndInit();
            this.scFooter.Panel1.ResumeLayout(false);
            this.scFooter.Panel2.ResumeLayout(false);
            this.scFooter.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scFooter)).EndInit();
            this.scFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer scToolPanel;
        private System.Windows.Forms.PictureBox pbDrawingSurface;
        private SplitContainer scFooter;
        private Label lStatus;
        private Button btnUndo;
        private Button btnAdd;
        private Button btnSelect;
        private Button btnObject;
    }
}
