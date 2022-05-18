// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using Kicker.ImageRecognition.Benchmarks.Imaging;

//BenchmarkRunner.Run<HomographyCalculationBenchmarks>();
//BenchmarkRunner.Run<HomographyTransformBenchmarks>();
//BenchmarkRunner.Run<ImageAnalyzerFindBarPositionBenchmarks>();
BenchmarkRunner.Run<ColorFromArgbBenchmarks>();