//Enum is a special type of integer, that defines a set of named constant values for better code readability and control.
namespace Entities;

public enum UserRole
{
    Owner = 0,
    Worker = 1,
    Vet = 2
}

public enum Department
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
