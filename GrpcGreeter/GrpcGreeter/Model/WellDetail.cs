namespace Model;
public class WellDetail
{
    public string Id { get; set; }
    public string? Name { get; set; }
    public string? SlotId { get; set; }
    public string? SlotName { get; set; }
    public string? StructureId { get; set; }
    public Location? Location { get; set; }
    public double? GroundLevel { get; set; }
    public Wgs84Location Wgs84Location { get; set; }
    public Elevation? Elevation { get; set; }
    public Metadata Metadata { get; set; }
    public bool? IsOffshore { get; set; }
    public string? FieldName { get; set; }
    public string? Country { get; set; }
    public string? ClientName { get; set; }
    public string? LocationCode { get; set; }
    public string Area { get; set; }
    public string State { get; set; }
    public string County { get; set; }
    public string City { get; set; }
    public string Uwi { get; set; }
    public string WellType { get; set; }
    public string WellPurpose { get; set; }
    public int? Version { get; set; }
    public List<string> DrillplanWellboreNodeIds { get; set; }
}

public class Location
{
    public string CoordinateReferenceSystem { get; set; }
    public string AzimuthReference { get; set; }
    public Uncertainty Uncertainty { get; set; }
    public string CoordinatesType { get; set; }
    public double? Northing { get; set; }
    public double? Easting { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public double? NorthSouth { get; set; }
    public double? EastWest { get; set; }
    public double? Azimuth { get; set; }
    public double? Distance { get; set; }
    public double? GridConvergence { get; set; }
    public double? ScaleFactor { get; set; }
}

public class Uncertainty
{
    public int? Confidence { get; set; }
    public double? Radius { get; set; }
}

public class Wgs84Location
{
    public string Type { get; set; }
    public List<double> Coordinates { get; set; }
}

public class Elevation
{
    public string Name { get; set; }
    public double? Offset { get; set; }
}

public class Metadata
{
    public string Src { get; set; }
    public string Trace { get; set; }
    public string Id { get; set; }
    public DateTime? Time { get; set; }
    public string TransferVersion { get; set; }
    public DateTime? LastUpdateTime { get; set; }
    public string DrillplanId { get; set; }
}
