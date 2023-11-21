

打算;让液培砖消耗按2.5kg一次运行,而不是每时每吸.

默认它是5kg的配置.  速率是1f.  实际是实时消耗,实时补充.

要修改这种代码. 

方案1:改速度
方案2:改回调.
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.consumptionRate = 1f;
		conduitConsumer.capacityKG = 5f;
		conduitConsumer.capacityTag = GameTags.Liquid;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;