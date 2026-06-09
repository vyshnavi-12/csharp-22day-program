using System;
using System.Collections.Generic;

namespace CareBridge.PerformanceLab.Models;

public partial class VwBilling
{
    public int PatientId { get; set; }

    public string FullName { get; set; } = null!;

    public string Payer { get; set; } = null!;

    public string PolicyNumber { get; set; } = null!;

    public int ClaimId { get; set; }

    public decimal BilledAmount { get; set; }

    public decimal? ReimbursedAmt { get; set; }

    public string Status { get; set; } = null!;
}
