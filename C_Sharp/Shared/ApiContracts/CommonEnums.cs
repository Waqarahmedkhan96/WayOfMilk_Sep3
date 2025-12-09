namespace ApiContracts;

// Must match Java/Proto names & meaning
public enum UserRole
{
    Owner = 0,
    Worker = 1,
    Vet = 2
}


public enum DepartmentType
{
    Resting = 0,
    Milking = 1,
    Quarantine = 2
}

public enum MilkTestResult
{
    Unknown = 0,
    Pass = 1,
    Fail = 2
}
