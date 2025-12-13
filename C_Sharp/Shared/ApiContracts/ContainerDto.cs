using System;
using System.Collections.Generic;

namespace ApiContracts;

// DTO: create container
public class CreateContainerDto
{
    public double CapacityL { get; set; }
}

// DTO: single container
public class ContainerDto
{
    public long Id { get; set; }
    public double CapacityL { get; set; }
    public double OccupiedCapacityL { get; set; } // current volume
}

// DTO: update container
public class UpdateContainerDto
{
    public long Id { get; set; }
    public double CapacityL { get; set; }
}

// DTO: delete containers batch
public class DeleteContainersDto
{
    public required long[] Ids { get; set; }
}

// DTO: list of containers
public class ContainerListDto
{
    public List<ContainerDto> Containers { get; set; } = new();
}

// DTO: container filters
public class ContainerQueryParameters
{
    public double? MinCapacityL { get; set; }
    public double? MaxCapacityL { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
