namespace ApiContracts.Containers;

// Read
public class ContainerDto
{
    public int Id { get; set; }
    public double CapacityL { get; set; }
}

// Create
public class CreateContainerDto
{
    public double CapacityL { get; set; }
}

// Update
public class UpdateContainerDto
{
    public double CapacityL { get; set; }
}

// Delete (batch)
public class DeleteContainersDto
{
    public required int[] Ids { get; set; }
}

// List
public class ContainerListDto
{
    public List<ContainerDto> Containers { get; set; } = new();
}

// Query
public class ContainerQueryParameters
{
    public double? MinCapacityL { get; set; }
    public double? MaxCapacityL { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
