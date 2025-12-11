using ApiContracts;
using Sep3.WayOfMilk.Grpc;

namespace WoM_WebApi.Mapping;

public class CowGrpcMapper
{
    //=========== COW ===========

    public static CowDto ToDto(CowData data)
    {
        return new CowDto
        {
            Id = data.Id,
            RegNo = data.RegNo ?? string.Empty, // Handle nulls safely
            BirthDate = DateGrpcMapping.FromGrpcDate(data.BirthDate), // Safe Date Parse
            Healthy = data.IsHealthy,
            DepartmentId = data.DepartmentId == 0 ? null : data.DepartmentId,
            DepartmentName = null // Placeholder for UI enrichment
        };
    }

    //not sure if needed, but better have than not have
    public static CowData ToGrpc(CowDto dto)
    {
        return new CowData
        {
            Id = dto.Id,
            RegNo = dto.RegNo,
            BirthDate = DateGrpcMapping.ToGrpcDate(dto.BirthDate),
            IsHealthy = dto.Healthy,
            DepartmentId = dto.DepartmentId ?? 0
        };
    }

   //creation function

    public static CowCreationDto ToCreationDto(CowCreationRequest data)
    //not really needed
    {
        return new CowCreationDto
        {
            RegNo = data.RegNo,
            BirthDate = DateGrpcMapping.FromGrpcDate(data.BirthDate),
            DepartmentId = data.QuarantineDepartmentId == 0 ? null : data.QuarantineDepartmentId,
            RegisteredByUserId = data.RegisteredByUserId
        };
    }

    public static CowCreationRequest CreationDtoToCow(CowCreationDto dto)
    {
        return new CowCreationRequest
        {
            RegNo = dto.RegNo,
            BirthDate = DateGrpcMapping.ToGrpcDate(dto.BirthDate),
            QuarantineDepartmentId = dto.DepartmentId ?? 0,
            RegisteredByUserId = dto.RegisteredByUserId
        };
    }



}