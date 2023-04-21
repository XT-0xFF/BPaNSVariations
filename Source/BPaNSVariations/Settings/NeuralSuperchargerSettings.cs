﻿using BPaNSVariations.Utility;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace BPaNSVariations.Settings
{
	internal class NeuralSuperchargerSettings : BaseSettings
	{
		//Misc
		//- Charge time
		//- Anyone can build toggle

		//Buff
		//- Duration
		//- Consciousness
		//- Global Learning Factor
		//- Hunger Rate Factor
		#region PROPERTIES
		public int DefaultTicksToRecharge { get; }
		public int TicksToRecharge
		{
			get => Def.GetSingleCompPropertiesOfType<CompProperties_NeuralSupercharger>().ticksToRecharge;
			set => Def.GetSingleCompPropertiesOfType<CompProperties_NeuralSupercharger>().ticksToRecharge = value;
		}

		public static bool DefaultAnyoneCanBuild { get; private set; }
		public static bool _anyoneCanBuild = false;
		public static bool AnyoneCanBuild
		{
			get => _anyoneCanBuild;
			set => SetAnyoneCanBuild(value);
		}

		public static int DefaultHediffDisappearsAfterTicks { get; private set; }
		public static int HediffDisappearsAfterTicks
		{
			get => HediffDefOf.NeuralSupercharge.GetSingleCompPropertiesOfType<HediffCompProperties_Disappears>().disappearsAfterTicks.max;
			set => HediffDefOf.NeuralSupercharge.GetSingleCompPropertiesOfType<HediffCompProperties_Disappears>().disappearsAfterTicks.max = value;
		}
		public static float DefaultHediffConsciousness { get; private set; }
		public static float HediffConsciousness
		{
			get => HediffDefOf.NeuralSupercharge.stages.First().capMods.First(v => v.capacity == PawnCapacityDefOf.Consciousness).offset;
			set => HediffDefOf.NeuralSupercharge.stages.First().capMods.First(v => v.capacity == PawnCapacityDefOf.Consciousness).offset = value;
		}
		public static float DefaultHediffLearningFactor { get; private set; }
		public static float HediffLearningFactor
		{
			get => HediffDefOf.NeuralSupercharge.stages.First().statOffsets.First(v => v.stat == StatDefOf.GlobalLearningFactor).value;
			set => HediffDefOf.NeuralSupercharge.stages.First().statOffsets.First(v => v.stat == StatDefOf.GlobalLearningFactor).value = value;
		}
		public static float DefaultHediffHungerRateFactor { get; private set; }
		public static float HediffHungerRateFactor
		{
			get => HediffDefOf.NeuralSupercharge.stages.First().hungerRateFactorOffset;
			set => HediffDefOf.NeuralSupercharge.stages.First().hungerRateFactorOffset = value;
		}
		#endregion

		#region FIELDS
		private static readonly List<MemeDef> _memeDefsWithDesignator = new List<MemeDef>();
		#endregion

		#region CONSTRUCTORS
		public NeuralSuperchargerSettings(ThingDef neuralSupercharger) : base(neuralSupercharger)
		{
			DefaultTicksToRecharge = TicksToRecharge;
		}
		#endregion

		#region PUBLIC METHODS
		public static void InitializeStatics()
		{
			DefaultAnyoneCanBuild = AnyoneCanBuild;

			DefaultHediffDisappearsAfterTicks = HediffDisappearsAfterTicks;
			DefaultHediffConsciousness = HediffConsciousness;
			DefaultHediffLearningFactor = HediffLearningFactor;
			DefaultHediffHungerRateFactor = HediffHungerRateFactor;
		}

		public static void ExposeStatics()
		{
			if (Scribe.EnterNode(nameof(NeuralSuperchargerSettings)))
			{
				try
				{
					bool boolValue = AnyoneCanBuild;
					Scribe_Values.Look(ref boolValue, nameof(AnyoneCanBuild), DefaultAnyoneCanBuild);
					AnyoneCanBuild = boolValue;

					int intValue = HediffDisappearsAfterTicks;
					Scribe_Values.Look(ref intValue, nameof(HediffDisappearsAfterTicks), DefaultHediffDisappearsAfterTicks);
					HediffDisappearsAfterTicks = intValue;

					float floatValue = HediffConsciousness;
					Scribe_Values.Look(ref floatValue, nameof(HediffConsciousness), DefaultHediffConsciousness);
					HediffConsciousness = floatValue;

					floatValue = HediffLearningFactor;
					Scribe_Values.Look(ref floatValue, nameof(HediffLearningFactor), DefaultHediffLearningFactor);
					HediffLearningFactor = floatValue;

					floatValue = HediffHungerRateFactor;
					Scribe_Values.Look(ref floatValue, nameof(HediffHungerRateFactor), DefaultHediffHungerRateFactor);
					HediffHungerRateFactor = floatValue;
				}
				catch (Exception exc)
				{
					Log.Error(exc.ToString());
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
		}
		#endregion

		#region OVERRIDES
		public override bool IsModified() =>
			base.IsModified()
			|| DefaultTicksToRecharge != TicksToRecharge;

		public override void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving && !IsModified())
				return;

			if (Scribe.EnterNode(Def.defName))
			{
				try
				{
					base.ExposeData();

					int intValue = TicksToRecharge;
					Scribe_Values.Look(ref intValue, nameof(TicksToRecharge), DefaultTicksToRecharge);
					TicksToRecharge = intValue;
				}
				catch (Exception exc)
				{
					Log.Error(exc.ToString());
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
		}

		public override void CopyTo(BaseSettings to)
		{
			if (to != this && to is NeuralSuperchargerSettings copy)
			{
				base.CopyTo(to);

				copy.TicksToRecharge = TicksToRecharge;
			}
		}
		#endregion

		#region PRIVATE METHODS
		private static void SetAnyoneCanBuild(bool value)
		{
			if (_anyoneCanBuild != value)
			{
				_anyoneCanBuild = value;

				if (value)
				{
					foreach (var memeDef in DefDatabase<MemeDef>.AllDefs)
					{
						if (memeDef.addDesignatorGroups?.Contains(BPaNSDefOf.SY_BNV_NeuralSuperchargers) == true)
						{
							if (!_memeDefsWithDesignator.Contains(memeDef))
								_memeDefsWithDesignator.AddDistinct(memeDef);
							memeDef.addDesignatorGroups.Remove(BPaNSDefOf.SY_BNV_NeuralSuperchargers);
						}
					}
				}
				else
				{
					foreach (var memeDef in _memeDefsWithDesignator)
						if (!memeDef.addDesignatorGroups.Contains(BPaNSDefOf.SY_BNV_NeuralSuperchargers))
							memeDef.addDesignatorGroups.Add(BPaNSDefOf.SY_BNV_NeuralSuperchargers);
				}

				foreach (var def in BPaNSUtility.GetNeuralSuperchargerDefs())
					def.canGenerateDefaultDesignator = value;
			}
		}
		#endregion
	}
}
