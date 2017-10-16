#region Copyright & License Information
/*
 * Copyright 2007-2017 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenRA.Traits;

namespace OpenRA.Mods.Common.Traits
{
	[Desc("The actor can produce units with modified AutoTarget stance.")]
	public class AutoTargetProducerInfo : ConditionalTraitInfo, Requires<ProductionInfo>
	{
		[Desc("It will try to modify AutoTarget stance for units that have different than default initial value.")]
		public readonly bool ChangeSpecialStance = false;
		public override object Create(ActorInitializer init) { return new AutoTargetProducer(this); }
	}	

	public class AutoTargetProducer : ConditionalTrait<AutoTargetProducerInfo>, IResolveOrder
	{
		public static uint CancelStanceSetting = (uint)Enum.GetValues(typeof(UnitStance)).Cast<UnitStance>().Max() + 1;

		public UnitStance? Stance { get; set; }

		public AutoTargetProducer(AutoTargetProducerInfo info) : base(info) { }

		void IResolveOrder.ResolveOrder(Actor self, Order order)
		{
			// Decision to cancel production stance should come from UI and not decided by trait
			if (order.OrderString == "SetBuildingProductionStance")
				if (order.ExtraData == CancelStanceSetting)
					Stance = null;
				else
					Stance = (UnitStance)order.ExtraData;
		}
	}
}
