using System;
using BenchmarkDotNet.Running;

Console.WriteLine("This is my benchmark.");

var summary = BenchmarkRunner.Run(typeof(Program).Assembly);

//Console.WriteLine(summary);
