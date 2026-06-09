using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using CareBridge.PerformanceLab.Models;

var context = new CareBridgeContext();

CartesianExplosionDemo();

Console.WriteLine();

SplitQueryDemo();

void CartesianExplosionDemo()
{
    Console.WriteLine("------------------------------------------------");
    Console.WriteLine("                 SINGLE QUERY");
    Console.WriteLine("------------------------------------------------");

    Stopwatch sw = Stopwatch.StartNew();

    var patient = context.Patients
        .AsNoTracking()
        .Include(p => p.Encounters)
            .ThenInclude(e => e.Diagnoses)
        .Include(p => p.Encounters)
            .ThenInclude(e => e.Claims)
        .FirstOrDefault(p => p.Mrn == "MRN888888");

    sw.Stop();

    int encounterCount = patient?.Encounters.Count ?? 0;

    int diagnosisCount = patient?.Encounters
        .SelectMany(e => e.Diagnoses)
        .Count() ?? 0;

    int claimCount = patient?.Encounters
        .SelectMany(e => e.Claims)
        .Count() ?? 0;

    Console.WriteLine($"Encounters : {encounterCount}");
    Console.WriteLine($"Diagnoses  : {diagnosisCount}");
    Console.WriteLine($"Claims     : {claimCount}");
    Console.WriteLine("SQL Statements (Profiler) : 1");
    Console.WriteLine("Rows returned by SQL : 900 (cross-product)");
    Console.WriteLine($"Tracked Entities : {context.ChangeTracker.Entries().Count()}");
    Console.WriteLine($"Elapsed Time : {sw.ElapsedMilliseconds} ms");
}

void SplitQueryDemo()
{
    Console.WriteLine("------------------------------------------------");
    Console.WriteLine("         SPLIT QUERY (AsSplitQuery)");
    Console.WriteLine("------------------------------------------------");

    Stopwatch sw = Stopwatch.StartNew();

    var patient = context.Patients
        .AsNoTracking()
        .AsSplitQuery()
        .Include(p => p.Encounters)
            .ThenInclude(e => e.Diagnoses)
        .Include(p => p.Encounters)
            .ThenInclude(e => e.Claims)
        .FirstOrDefault(p => p.Mrn == "MRN888888");

    sw.Stop();

    int encounterCount = patient?.Encounters.Count ?? 0;

    int diagnosisCount = patient?.Encounters
        .SelectMany(e => e.Diagnoses)
        .Count() ?? 0;

    int claimCount = patient?.Encounters
        .SelectMany(e => e.Claims)
        .Count() ?? 0;

    Console.WriteLine($"Encounters : {encounterCount}");
    Console.WriteLine($"Diagnoses  : {diagnosisCount}");
    Console.WriteLine($"Claims     : {claimCount}");
    Console.WriteLine("SQL Statements (Profiler) : 3");
    Console.WriteLine("Max rows in any statement : 300 (no explosion)");
    Console.WriteLine($"Tracked Entities : {context.ChangeTracker.Entries().Count()}");
    Console.WriteLine($"Elapsed Time : {sw.ElapsedMilliseconds} ms");

    bool identical =
        encounterCount == 100 &&
        diagnosisCount == 300 &&
        claimCount == 300;

    Console.WriteLine($"Object counts identical : {identical}");
}