using PINChat.Api.Library.Models;

namespace PINChat.Api.Library.DataAccess.Interfaces;

public interface IImageData
{
    byte[] GetUserImage(string userId);
    byte[] GetGroupImage(string groupId);
}