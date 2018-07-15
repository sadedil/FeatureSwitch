﻿using System;
using FeatureSwitch.Strategies.Implementations;

namespace FeatureSwitch.Strategies
{
    public class AlwaysFalse : FeatureStrategyAttribute
    {
        public AlwaysFalse()
        {
            Key = "341C1B91-12C2-4D13-9BB3-7791979D9CE5";
        }

        public override Type DefaultImplementation
        {
            get
            {
                return typeof(AlwaysFalseStrategyImpl);
            }
        }
    }
}
