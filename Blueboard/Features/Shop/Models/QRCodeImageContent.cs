namespace Blueboard.Features.Shop.Models;

/// <summary>
///     The content of a QR code image.
/// </summary>
public class QRCodeImageContent
{
    public int Id { get; set; }
    public string Secret { get; set; }
}