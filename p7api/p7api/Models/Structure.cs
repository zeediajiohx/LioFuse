using pseven.Models;
using pseven.Models.well;
using System.Collections.Generic;

namespace pseven.Models.structure
{
    public class Structure
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Operator { get; set; }
        public bool? Is_offshore { get; set; }
        public Location? Location { get; set; }
        public Wgs84Location? Wgs84_location { get; set; }
        public ElevationDatum? Elevation_datum { get; set; }
        public Elevation? Elevation { get; set; }
        public double? Structure_mas { get; set; }
        public Metadata? Metadata { get; set; }
        public string? Field_name { get; set; }
        public string? Country { get; set; }
        public string? Client_name { get; set; }
        public string? Location_code { get; set; }
        public List<SlotSet>? Slot_set { get; set; }
        public List<WellSet>? Well_set { get; set; }
        public int? _Version { get; set; }
        public string? Comments { get; set; }
    }
    public class Location
    {
        public string? Coordinate_reference_system { get; set; }
        public string? Azimuth_reference { get; set; }
        public Uncertainty? Uncertainty { get; set; }
        public string? Coordinates_type { get; set; }
        public double? Northing { get; set; }
        public double? Easting { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? North_south { get; set; }
        public double? East_west { get; set; }
        public double? Azimuth { get; set; }
        public double? Distance { get; set; }
        public double? Grid_convergence { get; set; }
        public double? Scale_factor { get; set; }
    }
    public class Uncertainty
    {
        public double? Confidence { get; set; }
        public double? Radius { get; set; }
    }
    public class Wgs84Location
    {
        public string? Type { get; set; }
        public List<double>? Coordinates { get; set; }
    }
    public class ElevationDatum
    {
        public string? Name { get; set; }
        public double? Offset { get; set; }
    }


    public class Elevation
    {
        public double? Default_ground_level { get; set; }
        public string? Name { get; set; }
        public double? Offset { get; set; }
    }


    public class Metadata
    {
        public string? Src { get; set; }
        public string? Trace { get; set; }
        public string? Id { get; set; }
        public string? Time { get; set; }
        public string? Transfer_version { get; set; }
        public DateTime? Last_update_time { get; set; }
        public string? Drillplan_id { get; set; }
    }


    public class SlotSet
    {
        public List<AssignedWell>? Assigned_wells { get; set; }
        public bool? Is_assigned_to_actual_well { get; set; }
        public string? _Id { get; set; }
        public string? Name { get; set; }
        public SlotLocation? Location { get; set; }
        public double? Diameter { get; set; }
        public Metadata? Metadata { get; set; }
    }
    public class AssignedWell
    {
        public string? Well_id { get; set; }
        public string? Well_name { get; set; }
        public List<string>? Drillplan_wellbore_node_ids { get; set; }
    }
    public class SlotLocation
    {
        public string? Coordinates_type { get; set; }
        public double? Northing { get; set; }
        public double? Easting { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? North_south { get; set; }
        public double? East_west { get; set; }
        public double? Azimuth { get; set; }
        public double? Distance { get; set; }
        public double? Grid_convergence { get; set; }
        public double? Scale_factor { get; set; }
    }
    public class WellSet
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Slot_id { get; set; }
        public string? Slot_name { get; set; }
        public string? Structure_id { get; set; }
        public Location? Location { get; set; }
        public double? Ground_level { get; set; }
        public Wgs84Location? Wgs84_location { get; set; }
        public Elevation? Elevation { get; set; }
        public ElevationDatum? Elevation_datum { get; set; }
        public Wrp? Wrp { get; set; }
        public double? Conductor_diameter { get; set; }
        public string? Mother_well_slot { get; set; }
        public Metadata? Metadata { get; set; }
        public bool? Is_offshore { get; set; }
        public string? Field_name { get; set; }
        public string? Country { get; set; }
        public string? Client_name { get; set; }
        public string? Location_code { get; set; }
        public string? Area { get; set; }
        public string? State { get; set; }
        public string? County { get; set; }
        public string? City { get; set; }
        public string? Uwi { get; set; }
        public string? Well_type { get; set; }
        public string? Well_purpose { get; set; }
        public int? _Version { get; set; }
        public List<string>? Drillplan_wellbore_node_ids { get; set; }
    }
    public class Wrp
    {
        public double? Ns { get; set; }
        public double? Ew { get; set; }
        public double? Tvd { get; set; }
        public double? Md { get; set; }
        public double? Inclination { get; set; }
        public double? Azimuth { get; set; }
    }
   
}
