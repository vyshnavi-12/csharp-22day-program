using System;
using System.Collections.Generic;

namespace CareBridge.PerformanceLab.Models;

public partial class VwBillingClaim
{
    public int ClaimId { get; set; }

    public int EncounterId { get; set; }

    public int InsuranceId { get; set; }

    public decimal BilledAmount { get; set; }

    public decimal? ReimbursedAmt { get; set; }

    public string Status { get; set; } = null!;
}
