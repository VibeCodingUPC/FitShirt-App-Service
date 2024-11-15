namespace FitShirt.Domain.OrderManagement.Models.Commands;

public class CreateDesignOrderCommand
{
    public int DesignId { get; set; }
    public int SellerId { get; set; }

    public CreateDesignOrderCommand(int sellerId, int designId)
    {
        DesignId = designId;
        SellerId = sellerId;
    }
}