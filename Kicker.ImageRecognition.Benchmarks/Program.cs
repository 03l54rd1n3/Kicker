// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using Kicker.ImageRecognition.Benchmarks;

BenchmarkRunner.Run<HomographyBenchmarks>();