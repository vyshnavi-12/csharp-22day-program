CREATE DATABASE CareBridgeDB;

USE CareBridgeDB;

CREATE TABLE Department (
    DepartmentId   INT IDENTITY(1,1) PRIMARY KEY,
    Name           NVARCHAR(100) NOT NULL,
    Location       NVARCHAR(100) NULL
);

CREATE TABLE Provider (
    ProviderId     INT IDENTITY(1,1) PRIMARY KEY,
    FullName       NVARCHAR(150) NOT NULL,
    Specialty      NVARCHAR(100) NOT NULL,
    DepartmentId   INT NOT NULL
        REFERENCES Department(DepartmentId)
);

CREATE TABLE Patient (
    PatientId      INT IDENTITY(1,1) PRIMARY KEY,
    MRN            NVARCHAR(20) NOT NULL UNIQUE,
    FullName       NVARCHAR(150) NOT NULL,
    DateOfBirth    DATE NOT NULL,
    Gender         CHAR(1) NOT NULL
        CHECK (Gender IN ('M','F','O')),
    City           NVARCHAR(100) NULL,
    IsActive       BIT NOT NULL DEFAULT 1
);

CREATE TABLE Insurance (
    InsuranceId    INT IDENTITY(1,1) PRIMARY KEY,
    PatientId      INT NOT NULL REFERENCES Patient(PatientId),
    Payer          NVARCHAR(120) NOT NULL,
    PolicyNumber   NVARCHAR(50) NOT NULL,
    EffectiveDate  DATE NOT NULL,
    ExpiryDate     DATE NULL
);

CREATE TABLE Encounter (
    EncounterId    INT IDENTITY(1,1) PRIMARY KEY,
    PatientId      INT NOT NULL REFERENCES Patient(PatientId),
    ProviderId     INT NOT NULL REFERENCES Provider(ProviderId),
    DepartmentId   INT NOT NULL REFERENCES Department(DepartmentId),
    AdmitDate      DATETIME2 NOT NULL,
    DischargeDate  DATETIME2 NULL,
    EncounterType  NVARCHAR(30) NOT NULL
        CHECK (EncounterType IN ('Inpatient','Outpatient','ED'))
);

CREATE TABLE Diagnosis (
    DiagnosisId    INT IDENTITY(1,1) PRIMARY KEY,
    EncounterId    INT NOT NULL REFERENCES Encounter(EncounterId),
    IcdCode        NVARCHAR(10) NOT NULL,
    Description    NVARCHAR(200) NOT NULL,
    DiagnosedOn    DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

CREATE TABLE [Procedure] (
    ProcedureId    INT IDENTITY(1,1) PRIMARY KEY,
    EncounterId    INT NOT NULL REFERENCES Encounter(EncounterId),
    CptCode        NVARCHAR(10) NOT NULL,
    Description    NVARCHAR(200) NOT NULL,
    PerformedOn    DATETIME2 NOT NULL
);

CREATE TABLE Claim (
    ClaimId        INT IDENTITY(1,1) PRIMARY KEY,
    EncounterId    INT NOT NULL REFERENCES Encounter(EncounterId),
    InsuranceId    INT NOT NULL REFERENCES Insurance(InsuranceId),
    BilledAmount   DECIMAL(12,2) NOT NULL,
    ReimbursedAmt  DECIMAL(12,2) NULL,
    Status         NVARCHAR(20) NOT NULL
        CHECK (Status IN ('Submitted','Paid','Denied'))
);

/* Assignment 1 - The Overloaded Emergency Department */
SELECT p.FullName, d.Name, Count(e.EncounterID) AS TotalEncounters,
	RANK()
	OVER (
		ORDER BY Count(e.EncounterID) DESC
	) AS ProviderRank
FROM Provider p
LEFT JOIN Encounter e ON e.ProviderId = p.ProviderId
JOIN Department d ON d.DepartmentId = p.DepartmentId

GROUP BY p.ProviderId, p.FullName, d.Name;

/* Assignment 2 - The Insurance Dispute Investigation */
ALTER TABLE Insurance
ADD
	ValidFrom DATETIME2
	GENERATED ALWAYS AS ROW START HIDDEN
	CONSTRAINT DF_Insurance_From
	DEFAULT SYSUTCDATETIME(),

	ValidTo DATETIME2
	GENERATED ALWAYS AS ROW END HIDDEN
	CONSTRAINT DF_Insurance_To
	DEFAULT '9999-12-31 23:59:59.9999999',

	PERIOD FOR SYSTEM_TIME(ValidFrom, ValidTo);

ALTER TABLE Insurance
SET
(
    SYSTEM_VERSIONING = ON
    (
        HISTORY_TABLE = dbo.InsuranceHistory
    )
);

UPDATE Insurance
SET Payer = 'HDFC ERGO'
WHERE PatientID = 9;

SELECT InsuranceID, Payer, PolicyNumber, ValidFrom, ValidTo
FROM Insurance
FOR SYSTEM_TIME ALL
WHERE PatientID = 9
ORDER BY ValidFrom;

/* Assignment 3 - The ₹2 Crore Revenue Leakage Investigation */
CREATE VIEW vw_BillingClaims
AS
SELECT
    c.ClaimId,
    c.EncounterId,
    c.InsuranceId,
    c.BilledAmount,
    c.ReimbursedAmt,
    c.Status
FROM Claim c;

CREATE OR ALTER PROCEDURE usp_MonthlyClaimRecoveryReport
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
    Status AS ClaimStatus,
	COUNT(*) AS TotalClaims
    SUM(BilledAmount) AS TotalBilledAmount,
    SUM(ISNULL(ReimbursedAmt,0)) AS TotalReimbursedAmount,
    SUM(BilledAmount - ISNULL(ReimbursedAmt,0)) AS OutstandingAmount,
	RANK() 
	OVER
        (
          ORDER BY SUM(BilledAmount - ISNULL(ReimbursedAmt,0)) DESC
        ) AS LossRank
    FROM vw_BillingClaims
    GROUP BY Status
    ORDER BY LossRank;
END;

EXEC usp_MonthlyClaimRecoveryReport;

/* Assignment 4 - Build Your Own Executive Dashboard */
CREATE OR ALTER PROCEDURE usp_ExecutiveDashboard
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) AS TotalActivePatients
    FROM Patient
    WHERE IsActive = 1;

    SELECT TOP 5 d.Name AS Department, COUNT(e.EncounterId) AS TotalEncounters
    FROM Encounter e
    JOIN Department d ON e.DepartmentId = d.DepartmentId
    GROUP BY d.Name
    ORDER BY TotalEncounters DESC;

    SELECT AVG(DATEDIFF(DAY, AdmitDate, DischargeDate)) AS AverageLengthOfStay
    FROM Encounter
    WHERE DischargeDate IS NOT NULL;
END;

EXEC usp_ExecutiveDashboard;

/* Assignment 5 – CareBridge Clinical Operations Console */
-- 30 day Readmissions
CREATE OR ALTER PROCEDURE usp_ThirtyDayReadmissions
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT p.PatientId, p.FullName, e1.EncounterId AS PreviousEncounter, e2.EncounterId AS ReadmissionEncounter
    FROM Encounter e1
    JOIN Encounter e2 ON e1.PatientId = e2.PatientId
    JOIN Patient p ON p.PatientId = e1.PatientId
    WHERE e1.DischargeDate IS NOT NULL AND e2.AdmitDate > e1.DischargeDate
        AND DATEDIFF(DAY, e1.DischargeDate, e2.AdmitDate) <= 30;
END;

-- High Risk Patients
CREATE OR ALTER PROCEDURE usp_HighRiskPatients
AS
BEGIN
    SET NOCOUNT ON;

    SELECT p.PatientId, p.FullName, COUNT(e.EncounterId) AS TotalAdmissions
    FROM Patient p
    JOIN Encounter e ON p.PatientId = e.PatientId
    GROUP BY p.PatientId, p.FullName
    HAVING COUNT(e.EncounterId) >= 3
    ORDER BY TotalAdmissions DESC;
END;

-- Provider Workload
CREATE OR ALTER PROCEDURE usp_ProviderWorkload
AS
BEGIN
    SET NOCOUNT ON;

    SELECT pr.ProviderId, pr.FullName, pr.Specialty, COUNT(e.EncounterId) AS TotalEncounters
    FROM Provider pr
    LEFT JOIN Encounter e ON pr.ProviderId = e.ProviderId
    GROUP BY pr.ProviderId, pr.FullName, pr.Specialty
    ORDER BY TotalEncounters DESC;
END;

-- Revenue Analysis
CREATE OR ALTER PROCEDURE usp_RevenueAnalysis
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) AS TotalClaims, SUM(BilledAmount) AS TotalBilledAmount, SUM(ReimbursedAmt) AS TotalReimbursedAmount
    FROM Claim;
END;

EXEC usp_ThirtyDayReadmissions;

EXEC usp_HighRiskPatients;

EXEC usp_ProviderWorkload;

EXEC usp_RevenueAnalysis;

/* Assignment 6 – HIPAA-Compliant Access Portal */
-- Clinical Team
CREATE OR ALTER VIEW vw_Clinical
AS
SELECT p.PatientId, p.MRN, p.FullName, e.EncounterId, e.EncounterType, e.AdmitDate,
    e.DischargeDate, d.IcdCode, d.Description AS Diagnosis
FROM Patient p
JOIN Encounter e ON p.PatientId = e.PatientId
JOIN Diagnosis d ON e.EncounterId = d.EncounterId;

-- Billing Team
CREATE OR ALTER VIEW vw_Billing
AS
SELECT p.PatientId, p.FullName, i.Payer, i.PolicyNumber,
    c.ClaimId, c.BilledAmount, c.ReimbursedAmt, c.Status
FROM Patient p
JOIN Insurance i ON p.PatientId = i.PatientId
JOIN Claim c ON i.InsuranceId = c.InsuranceId;

-- Analytics Team
CREATE OR ALTER VIEW vw_Analytics_DeId
AS
SELECT
    CASE
        WHEN DATEDIFF(YEAR, p.DateOfBirth, GETDATE()) < 18 THEN 'Under 18'
        WHEN DATEDIFF(YEAR, p.DateOfBirth, GETDATE()) BETWEEN 18 AND 40 THEN '18-40'
        WHEN DATEDIFF(YEAR, p.DateOfBirth, GETDATE()) BETWEEN 41 AND 65 THEN '41-65'
        ELSE '65+'
    END AS AgeBand,

    p.Gender,
    e.EncounterType
FROM Patient p
JOIN Encounter e ON p.PatientId = e.PatientId;

