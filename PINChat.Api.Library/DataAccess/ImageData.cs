using PINChat.Api.Library.DataAccess.Interfaces;
using PINChat.Api.Library.Models;
using SkiaSharp;

namespace PINChat.Api.Library.DataAccess;

public class ImageData : IImageData
{
    private readonly ISqlDataAccess _sql;

    public ImageData(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    public byte[] GetUserImage(string userId)
    {
        var p = new { Id = userId };
        
        var userDb = _sql.LoadData<UserDBModel, dynamic>("[PINChat].[spUsers_GetById]", p, "PINChatData").FirstOrDefault();

        return userDb!.Avatar!;
    }
    
    public byte[] GetGroupImage(string groupId)
    {
        var p = new { Id = groupId };
        
        var groupDb = _sql.LoadData<GroupDbModel, dynamic>("[PINChat].[spGroups_GetById]", p, "PINChatData").FirstOrDefault();

        var avatar = GenerateImageFromInitials($"{groupDb!.Name![0]}");
        
        return avatar;
    }

    private byte[] GenerateImageFromInitials(string? name)
    {
        var info = new SKImageInfo(100, 100); // Set the size of the bitmap
        var bitmap = new SKBitmap(info);
    
        using (var surface = SKSurface.Create(info, bitmap.GetPixels(), info.RowBytes))
        {
            var canvas = surface.Canvas;

            canvas.Clear(SKColors.LightSlateGray); // Set background color
        
            // Create a paint object for the text
            using (var paint = new SKPaint())
            {
                paint.Color = SKColors.White; // Set text color
                paint.TextAlign = SKTextAlign.Center;
                paint.TextSize = 36;
                paint.FakeBoldText = true;

                // Calculate the position to center the text on the bitmap
                var x = info.Width / 2;
                var y = (info.Height + paint.TextSize / 2) / 2;

                // Draw the text on the bitmap
                canvas.DrawText(name, x, y, paint);
            }
        }
        
        using (var image = SKImage.FromBitmap(bitmap))
        using (var data = image.Encode(SKEncodedImageFormat.Png, 300))
        using (var stream = data.AsStream())
        {
            // Get the byte array from the encoded data
            return data.ToArray();
        }
    }
}