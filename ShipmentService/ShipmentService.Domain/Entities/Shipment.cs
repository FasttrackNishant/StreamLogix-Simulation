namespace ShipmentService.Domain.Entities;

public class Shipment
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public int SourceAddressId { get; set; }
        public int DestinationAddressId { get; set; }
        public int  ShipmentStatusId { get; set; }
        public bool CustomFlow { get; set; }
        public int TriggeredBy { get; set; }
        public DateTime TriggeredAt { get; set; }
        public DateTime ExpectedDelivery { get; set; }
        public int SourceSystemId { get; set; }
        public string ExternalOrderId { get; set; }
    }