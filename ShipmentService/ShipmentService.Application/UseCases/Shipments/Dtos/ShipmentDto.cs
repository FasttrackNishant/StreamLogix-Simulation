using ShipmentService.Domain.Enums;

namespace ShipmentService.UseCases.Shipments.Dtos
{
    public class ShipmentDto
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public int ShipmentStatusId { get; set; }
        public DateTime TriggeredAt { get; set; }
        public DateTime ExpectedDelivery { get; set; }
        public string ExternalOrderId { get; set; }
    }
}