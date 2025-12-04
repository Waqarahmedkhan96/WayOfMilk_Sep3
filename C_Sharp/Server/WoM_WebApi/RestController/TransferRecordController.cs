using ApiContracts.TransferRecord;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WoM_WebApi.RestController;

[ApiController]
[Route("[controller]")]
public class TransferRecordsController : ControllerBase
{
    private readonly ITransferRecordRepository _transfers;

    public TransferRecordsController(ITransferRecordRepository transfer)
    {
        _transfers = transfer;
    }

    // POST /transferrecords
    [HttpPost]
    public async Task<ActionResult<TransferRecordDto>> AddTransfer([FromBody] CreateTransferRecordDto request)
    {
        var created = await _transfers.AddAsync(new TransferRecord
        {
            Department = request.Department,
            MovedAt = request.MovedAt,
            FromDept = request.FromDept,
            ToDept = request.ToDept,
            RequestedByUserId = request.RequestedByUserId,
            CowId = request.CowId
        });

        var dto = new TransferRecordDto
        {
            Id = created.Id,
            Department = created.Department,
            MovedAt = created.MovedAt,
            FromDept = created.FromDept,
            ToDept = created.ToDept,
            RequestedByUserId = created.RequestedByUserId,
            ApprovedByVetUserId = created.ApprovedByVetUserId,
            CowId = created.CowId
        };
        return Created($"/transferrecords/{dto.Id}", dto);
    }

    // GET /transferrecords/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TransferRecordDto>> GetTransfer(int id)
    {
        var t = await _transfers.GetSingleAsync(id);
        var dto = new TransferRecordDto
        {
            Id = t.Id,
            Department = t.Department,
            MovedAt = t.MovedAt,
            FromDept = t.FromDept,
            ToDept = t.ToDept,
            RequestedByUserId = t.RequestedByUserId,
            ApprovedByVetUserId = t.ApprovedByVetUserId,
            CowId = t.CowId
        };
        return Ok(dto);
    }

    // GET /transferrecords?cowId=...&department=...&fromMovedAt=...&toMovedAt=...
    [HttpGet]
    public ActionResult<IEnumerable<TransferRecordDto>> GetTransfers(
        [FromQuery] int? cowId,
        [FromQuery] Department? department,
        [FromQuery] DateOnly? fromMovedAt,
        [FromQuery] DateOnly? toMovedAt)
    {
        var query = _transfers.GetManyAsync();

        if (cowId.HasValue)
            query = query.Where(t => t.CowId == cowId.Value);
        if (department.HasValue)
            query = query.Where(t => t.Department == department.Value);
        if (fromMovedAt.HasValue)
            query = query.Where(t => t.MovedAt >= fromMovedAt.Value);
        if (toMovedAt.HasValue)
            query = query.Where(t => t.MovedAt <= toMovedAt.Value);

        var list = query.Select(t => new TransferRecordDto
        {
            Id = t.Id,
            Department = t.Department,
            MovedAt = t.MovedAt,
            FromDept = t.FromDept,
            ToDept = t.ToDept,
            RequestedByUserId = t.RequestedByUserId,
            ApprovedByVetUserId = t.ApprovedByVetUserId,
            CowId = t.CowId
        }).ToList();

        return Ok(list);
    }

    // PUT /transferrecords/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTransfer(int id, [FromBody] UpdateTransferRecordDto request)
    {
        var transfer = new TransferRecord
        {
            Id = id,
            Department = request.Department,
            MovedAt = request.MovedAt,
            FromDept = request.FromDept,
            ToDept = request.ToDept,
            RequestedByUserId = request.RequestedByUserId,
            ApprovedByVetUserId = request.ApprovedByVetUserId,
            CowId = request.CowId
        };
        await _transfers.UpdateAsync(transfer);
        return NoContent();
    }

    // DELETE /transferrecords/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTransfer(int id)
    {
        await _transfers.DeleteAsync(id);
        return NoContent();
    }
}
