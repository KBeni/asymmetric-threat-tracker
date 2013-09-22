﻿#region copyright
// Copyright 2013 
// Predictive Technology Laboratory 
// predictivetech@virginia.edu
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
 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PTL.ATT.Smoothers;
using PTL.ATT.Models;

namespace PTL.ATT.GUI
{
    public partial class KernelDensityDcmForm : Form
    {
        public string ModelName
        {
            get { return modelName.Text; }
        }

        public int PointSpacing
        {
            get { return (int)pointSpacing.Value; }
        }

        public int TrainingSampleSize
        {
            get { return (int)trainingSampleSize.Value; }
        }

        public int PredictionSampleSize
        {
            get { return (int)predictionSampleSize.Value; }
        }

        public bool Normalize
        {
            get { return normalize.Checked; }
        }

        public IEnumerable<Smoother> Smoothers
        {
            get { return smoothers.SelectedItems.Cast<Smoother>().ToArray(); }
        }

        public KernelDensityDcmForm()
        {
            InitializeComponent();

            smoothers.Populate(null);
        }

        public KernelDensityDcmForm(KernelDensityDCM current)
            : this()
        {
            modelName.Text = current.Name;
            pointSpacing.Value = current.PointSpacing;
            trainingSampleSize.Value = current.TrainingSampleSize;
            predictionSampleSize.Value = current.PredictionSampleSize;
            normalize.Checked = current.Normalize;
            smoothers.Populate(current);
        }

        private void ok_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
    }
}
