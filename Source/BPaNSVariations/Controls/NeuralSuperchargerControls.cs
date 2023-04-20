﻿using BPaNSVariations.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BPaNSVariations.Controls
{
	internal class NeuralSuperchargerControls : BaseControls
	{
		#region PROPERTIES
		public NeuralSuperchargerSettings NeuralSuperchargerSettings => (NeuralSuperchargerSettings)Settings;
		#endregion

		#region CONSTRUCTORS
		public NeuralSuperchargerControls(NeuralSuperchargerSettings settings) : base(settings)
		{
		}
		#endregion

		#region OVERRIDES
		public override void CreateSettings(ref float offsetY, float viewWidth)
		{
			// Neural Supercharger
			CreateTitle(
				ref offsetY,
				viewWidth,
				Label);


			// Margin
			offsetY += SettingsRowHeight / 2;
		}
		#endregion
	}
}