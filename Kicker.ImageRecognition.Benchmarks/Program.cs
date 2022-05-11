// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using Kicker.ImageRecognition.Benchmarks;
using Kicker.ImageRecognition.Benchmarks.ImageTransformation;

//BenchmarkRunner.Run<HomographyCalculationBenchmarks>();
//BenchmarkRunner.Run<HomographyTransformBenchmarks>();
//BenchmarkRunner.Run<BitmapBenchmarks>();
BenchmarkRunner.Run<MemorizationBenchmarks>();
