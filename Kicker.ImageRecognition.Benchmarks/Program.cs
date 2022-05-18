// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using Kicker.ImageRecognition.Benchmarks;
using Kicker.ImageRecognition.Benchmarks.ImageTransformation;
using Kicker.ImageRecognition.Benchmarks.Masking;

//BenchmarkRunner.Run<HomographyCalculationBenchmarks>();
//BenchmarkRunner.Run<HomographyTransformBenchmarks>();
//BenchmarkRunner.Run<BitmapBenchmarks>();
BenchmarkRunner.Run<ImageAnalyzerFindBarPositionBenchmarks>();