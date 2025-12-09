using ApiContracts;
using Sep3.WayOfMilk.Grpc;

namespace WoM_WebApi.Mapping;

public class CowMapping
{
    //=========== COW ===========

    public static CowDto CowGrpcToDto(CowData data)
    {
        return new CowDto
        {
            Id = data.Id,
            RegNo = data.RegNo,
            // Convert String "YYYY-MM-DD" -> DateOnly
            BirthDate = DateOnly.Parse(data.BirthDate),
            Healthy = data.IsHealthy,
            DepartmentId = data.DepartmentId == 0 ? null : data.DepartmentId, // Handle 0 as null if needed
            DepartmentName = null // Proto doesn't send this yet
        };
    }

    //not sure if needed, but better have than not have
    public static CowData CowDtoToGrpc(CowDto dto)
    {
        return new CowData
        {
            Id = dto.Id,
            RegNo = dto.RegNo,
            BirthDate = dto.BirthDate.ToString("yyyy-MM-dd"), // Convert DateOnly -> String "YYYY-MM-DD"
            IsHealthy = dto.Healthy,
            DepartmentId = dto.DepartmentId ?? 0 // Handle null as 0 if needed
        };
    }

    //update relevant - not using the special class yet
    public static CowUpdateRequest CowUpdateRequestToGrpc(CowDto dto,
        long requesterUserId)
    {
        CowData cowToUpdate = CowDtoToGrpc(dto);
        return new CowUpdateRequest
        {
            CowData = cowToUpdate,
            RequestedBy = requesterUserId
        };

    }


    //creation function

    public static CowCreationDto CowToCreationDto(CowCreationRequest data)
    {
        return new CowCreationDto
        {
            RegNo = data.RegNo,
            BirthDate = DateOnly.Parse(data.BirthDate),
            DepartmentId = data.QuarantineDepartmentId == 0 ? null : data.QuarantineDepartmentId,
            RegisteredByUserId = data.RegisteredByUserId
        };
    }

    public static CowCreationRequest CowCreationDtoToCow(CowCreationDto dto)
    {
        return new CowCreationRequest
        {
            RegNo = dto.RegNo,
            BirthDate = dto.BirthDate.ToString("yyyy-MM-dd"),
            QuarantineDepartmentId = dto.DepartmentId ?? 0,
            RegisteredByUserId = dto.RegisteredByUserId
        };
    }

}