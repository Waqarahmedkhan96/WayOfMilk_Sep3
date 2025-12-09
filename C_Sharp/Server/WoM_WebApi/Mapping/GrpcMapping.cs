using ApiContracts;
using WoM_Grpc;

namespace WoM_WebApi.Mapping;

public static class GrpcMapping
{
    // -------- UserRole mapping --------

    // API -> gRPC
    public static UserRole ToGrpcUserRole(ApiContracts.UserRole role)
        => role switch
        {
            ApiContracts.UserRole.Owner  => CowFarm_Grpc.UserRole.Owner,
            ApiContracts.UserRole.Worker => CowFarm_Grpc.UserRole.Worker,
            ApiContracts.UserRole.Vet    => CowFarm_Grpc.UserRole.Vet,
            _                            => CowFarm_Grpc.UserRole.Worker
        };

    // gRPC -> API
    public static ApiContracts.UserRole ToApiUserRole(CowFarm_Grpc.UserRole role)
        => role switch
        {
            CowFarm_Grpc.UserRole.Owner  => ApiContracts.UserRole.Owner,
            CowFarm_Grpc.UserRole.Worker => ApiContracts.UserRole.Worker,
            CowFarm_Grpc.UserRole.Vet    => ApiContracts.UserRole.Vet,
            _                             => ApiContracts.UserRole.Worker
        };

    // -------- DepartmentType mapping --------

    // API -> gRPC
    public static CowFarm_Grpc.DepartmentType ToGrpcDepartment(ApiContracts.DepartmentType type)
        => type switch
        {
            ApiContracts.DepartmentType.Resting    => CowFarm_Grpc.DepartmentType.Resting,
            ApiContracts.DepartmentType.Milking    => CowFarm_Grpc.DepartmentType.Milking,
            ApiContracts.DepartmentType.Quarantine => CowFarm_Grpc.DepartmentType.Quarantine,
            _                                      => CowFarm_Grpc.DepartmentType.Resting
        };

    // gRPC -> API
    public static ApiContracts.DepartmentType ToApiDepartment(CowFarm_Grpc.DepartmentType type)
        => type switch
        {
            CowFarm_Grpc.DepartmentType.Resting    => ApiContracts.DepartmentType.Resting,
            CowFarm_Grpc.DepartmentType.Milking    => ApiContracts.DepartmentType.Milking,
            CowFarm_Grpc.DepartmentType.Quarantine => ApiContracts.DepartmentType.Quarantine,
            _                                      => ApiContracts.DepartmentType.Resting
        };

    // -------- MilkTestResult mapping --------

    // API -> gRPC
    public static CowFarm_Grpc.MilkTestResult ToGrpcMilkTest(ApiContracts.MilkTestResult result)
        => result switch
        {
            ApiContracts.MilkTestResult.Unknown => CowFarm_Grpc.MilkTestResult.Unspecified,
            ApiContracts.MilkTestResult.Pass    => CowFarm_Grpc.MilkTestResult.Pass,
            ApiContracts.MilkTestResult.Fail    => CowFarm_Grpc.MilkTestResult.Fail,
            _                                   => CowFarm_Grpc.MilkTestResult.Unspecified
        };

    // gRPC -> API
    public static ApiContracts.MilkTestResult ToApiMilkTest(CowFarm_Grpc.MilkTestResult result)
        => result switch
        {
            CowFarm_Grpc.MilkTestResult.Unspecified => ApiContracts.MilkTestResult.Unknown,
            CowFarm_Grpc.MilkTestResult.Pass                         => ApiContracts.MilkTestResult.Pass,
            CowFarm_Grpc.MilkTestResult.Fail                          => ApiContracts.MilkTestResult.Fail,
            _                                                        => ApiContracts.MilkTestResult.Unknown
        };
}
