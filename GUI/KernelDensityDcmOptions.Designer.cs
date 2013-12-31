﻿#region copyright
// Copyright 2013 Matthew S. Gerber (gerber.matthew@gmail.com)
// 
// This file is part of the Asymmetric Threat Tracker (ATT).
// 
// The ATT is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// The ATT is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with the ATT.  If not, see <http://www.gnu.org/licenses/>.
#endregion
 
namespace PTL.ATT.GUI
{
    partial class KernelDensityDcmOptions
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.normalize = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // normalize
            // 
            this.normalize.AutoSize = true;
            this.normalize.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.normalize.Location = new System.Drawing.Point(3, 3);
            this.normalize.Name = "normalize";
            this.normalize.Size = new System.Drawing.Size(75, 17);
            this.normalize.TabIndex = 0;
            this.normalize.Text = "Normalize:";
            this.normalize.UseVisualStyleBackColor = true;
            // 
            // KernelDensityDcmOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.normalize);
            this.Name = "KernelDensityDcmOptions";
            this.Size = new System.Drawing.Size(207, 151);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox normalize;
    }
}
