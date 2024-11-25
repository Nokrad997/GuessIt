using Backend.Dtos;
using Backend.Dtos.EditDtos;

namespace Backend.Services.Interfaces;

public interface IFriendsService
{
    Task<IEnumerable<FriendsDto>> Retrieve();
    Task<FriendsDto> Retrieve(int id);
    Task AddFriends(FriendsDto friendsDto, string token);
    Task<FriendsDto> EditFriendsAsUser(int id, EditFriendsDto editFriendsDto, string token);
    Task<FriendsDto> EditFriendsAsAdmin(int id, FriendsDto friendsDto);
    Task DeleteFriends(int id);
}