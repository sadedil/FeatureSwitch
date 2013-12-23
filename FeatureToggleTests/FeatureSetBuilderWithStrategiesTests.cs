﻿using FeatureToggle.Strategies;
using FeatureToggle.Tests.Features;
using FeatureToggle.Tests.Strategies;
using StructureMap;
using Xunit;

namespace FeatureToggle.Tests
{
    public class FeatureSetBuilderWithStrategiesTests
    {
        [Fact]
        public void BuilderTest_AddSameStrategyMultipleTimes_NoFailures()
        {
            var builder = new FeatureSetBuilder(new Container());
            builder.Build(ctx =>
                          {
                              ctx.AddFeature<MySampleFeature>();
                              ctx.ForStrategy<UnitTestsAlwaysTrueStrategy>().Use<AlwaysTrueStrategyReader>();
                              ctx.ForStrategy<UnitTestsAlwaysTrueStrategy>().Use<AlwaysTrueStrategyReader>();
                          },
                          expression => expression.AddRegistry<UnitTestDependencyRegistry>());

            var isEnabled = FeatureContext.IsEnabled<MySampleFeature>();

            Assert.True(isEnabled);
        }

        [Fact]
        public void  BuilderTest_CustomStrategyWithConstructorParameters_NoFailure()
        {
            var builder = new FeatureSetBuilder(new Container());
            builder.Build(ctx =>
                          {
                              ctx.AddFeature<MySampleFeatureWithConstructorParameterStrategy>();
                              ctx.ForStrategy<StrategyWithConstructorParameter>().Use<StrategyWithConstructorParameterReader>();
                          },
                          expression => expression.For<ISampleInjectedInterface>().Use<SampleInjectedInterface>());

            var feature = FeatureContext.GetFeature<MySampleFeatureWithConstructorParameterStrategy>();

            Assert.True(feature.IsEnabled);
            Assert.NotNull(((StrategyWithConstructorParameterReader)builder.GetStrategyImplementation<StrategyWithConstructorParameter>()).Dependency);
        }

        [Fact]
        public void BuilderTest_CustomStrategy_FeatureIsEnabled()
        {
            var builder = new FeatureSetBuilder(new Container());
            builder.Build(ctx =>
                          {
                              ctx.AddFeature<MyFancyStrategySampleFeature>();
                              ctx.ForStrategy<UnitTestsAlwaysTrueStrategy>().Use<AlwaysTrueStrategyReader>();
                          });

            var isEnabled = FeatureContext.IsEnabled<MyFancyStrategySampleFeature>();

            Assert.True(isEnabled, "MyFancyStrategySampleFeature is not enabled");
        }

        [Fact]
        public void BuilderTest_SwapBuiltInStrategy_ContainerHasSwappedStrategy()
        {
            var builder = new FeatureSetBuilder(new Container());
            builder.Build(ctx =>
                          {
                              ctx.AddFeature<MySampleFeature>();
                              ctx.ForStrategy<AppSettingsStrategy>().Use<AlwaysTrueStrategyReader>();
                          });

            var strategy = builder.GetStrategyImplementation<AppSettingsStrategy>();
            Assert.IsAssignableFrom(typeof(AlwaysTrueStrategyReader), strategy);
        }

        [Fact]
        public void BuilderTest_SwapBuiltInStrategy_NoFailure()
        {
            var builder = new FeatureSetBuilder(new Container());
            builder.Build(ctx =>
                          {
                              ctx.AddFeature<MySampleFeature>();
                              ctx.ForStrategy<AppSettingsStrategy>().Use<AlwaysTrueStrategyReader>();
                          });

            var isEnabled = FeatureContext.IsEnabled<MySampleFeature>();

            Assert.True(isEnabled);
        }
    }
}
