using PINChat.Api.Library.Models;

namespace PINChat.Api.Library.DataAccess.Interfaces;

public interface IGroupData
{
    List<GroupModel> GetAllGroups();
    void Create(dynamic p);
    void Update(GroupDbModel group);
}