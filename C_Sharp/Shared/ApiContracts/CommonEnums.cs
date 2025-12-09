using System;

namespace ApiContracts;

// Enum: user role (JWT)
public enum UserRole
{
    Owner = 0,   // Java: OWNER
    Worker = 1,  // Java: WORKER
    Vet = 2      // Java: VET
}

// Enum: department type
public enum DepartmentType
{
    Resting = 0,    // Java: RESTING
    Milking = 1,    // Java: MILKING
    Quarantine = 2  // Java: QUARANTINE
}

// Enum: milk test result
public enum MilkTestResult
{
    Unknown = 0, // Java: UNKNOWN
    Pass = 1,    // Java: PASS
    Fail = 2     // Java: FAIL
}
