﻿#region copyright
// Copyright 2013-2014 The Rector & Visitors of the University of Virginia
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace PTL.ATT.GUI.Visualization
{
    public partial class MultiVisualizer : UserControl
    {
        private IEnumerable<Prediction> _displayedPredictions;
        private IEnumerable<Overlay> _overlays;

        internal IEnumerable<Prediction> DisplayedPredictions
        {
            get { return _displayedPredictions; }
        }

        internal IEnumerable<Overlay> Overlays
        {
            get { return _overlays; }
        }

        public MultiVisualizer()
        {
            InitializeComponent();
        }

        public virtual void Display(IEnumerable<Prediction> predictions, IEnumerable<Overlay> overlays)
        {
            Invoke(new Action(Clear));

            _displayedPredictions = predictions;
            _overlays = overlays;
        }

        public virtual void Clear()
        {
            _displayedPredictions = null;
            _overlays = null;
        }
    }
}
