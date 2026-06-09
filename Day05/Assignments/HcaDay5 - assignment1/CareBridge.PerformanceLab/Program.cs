using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using CareBridge.PerformanceLab.Models;
using CareBridge.PerformanceLab.Models.DTOs;

var context = new CareBridgeContext();

RevenueAtRiskDashboard();

void RevenueAtRiskDashboard()
{
    Console.WriteLine("-----------------------------------------------------------------------");
    Console.WriteLine("REVENUE-AT-RISK DASHBOARD");
    Console.WriteLine("-----------------------------------------------------------------------");

    Stopwatch stopwatch = Stopwatch.StartNew();

    var summary = context.Claims
        .AsNoTracking()
        .GroupBy(c => c.Status)
        .Select(g => new ClaimDto
        {
            Status = g.Key,
            ClaimCount = g.Count(),
            TotalBilled = g.Sum(x => x.BilledAmount),
            TotalReimbursed = g.Sum(x => x.ReimbursedAmt),
            Gap = g.Sum(x => x.BilledAmount - x.ReimbursedAmt)
        })
        .OrderByDescending(x => x.TotalBilled)
        .ToList();

    var revenueAtRisk = new RevenueAtRiskDto
    {
        RevenueAtRisk = context.Claims
         .AsNoTracking()
         .Where(c => c.Status != "Paid")
         .Sum(c => c.BilledAmount)
    };

    stopwatch.Stop();

    Console.WriteLine(
        $"{"Status",-12} {"Claims",-10} {"Billed",-15} {"Reimbursed",-15} {"Gap",-15}");

    foreach (var item in summary)
    {
        Console.WriteLine(
            $"{item.Status,-12} {item.ClaimCount,-10} {item.TotalBilled,-15:F2} {item.TotalReimbursed,-15:F2} {item.Gap,-15:F2}");
    }

    Console.WriteLine("-----------------------------------------------------------------------");
    Console.WriteLine($"REVENUE AT RISK (not Paid) : {revenueAtRisk.RevenueAtRisk:F2}");
    Console.WriteLine($"Tracked Entities : {context.ChangeTracker.Entries().Count()}");
    Console.WriteLine($"Elapsed Time : {stopwatch.ElapsedMilliseconds} ms");
}