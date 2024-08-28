using System.Reflection;
using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Entities;
using Backend.Repositories;
using Backend.Repositories.Interfaces;
using Backend.Utility;
using Backend.Utility.Enums;

namespace Backend.Services;

public class FriendsService
{
    private readonly FriendsRepository _friendsRepository;
    private readonly TokenUtil _tokenUtil;
    
    public FriendsService(FriendsRepository friendsRepository, TokenUtil tokenUtil)
    {
        _friendsRepository = friendsRepository;
        _tokenUtil = tokenUtil;
    }

    public async Task<IEnumerable<FriendsDto>> Retrieve()
    {
        var retrievedFriends = await _friendsRepository.GetAllFriends();
        return retrievedFriends.Select(f => f.ConvertToDto()).ToList();
    }

    public async Task<FriendsDto> Retrieve(int id)
    {
        var retrievedFriends = await _friendsRepository.GetFriendsById(id);
        if( retrievedFriends is null)
        {
            throw new ArgumentException("No friends with provided id");
        }
        
        return retrievedFriends.ConvertToDto();
    }

    public async Task AddFriends(FriendsDto friendsDto, string token)
    {
        var requestUserId = _tokenUtil.GetIdFromToken(token);
        if(requestUserId != friendsDto.UserIdFk && requestUserId != friendsDto.FriendIdFk)
        {
            if(_tokenUtil.GetRoleFromToken(token) != "Admin")
            {
                throw new ArgumentException("Denied, cannot add friends for other users");
            }
        }
        
        var friends = await _friendsRepository.GetFriendsByUserAndFriendId(friendsDto.UserIdFk, friendsDto.FriendIdFk);
        if (friends is not null)
        {
            throw new ArgumentException("Friendship already exists");
        }
        
        if(_tokenUtil.GetRoleFromToken(token) != "Admin")
            friendsDto.UserFriendshipStatus = FriendsStatusTypes.Accepted;
        await _friendsRepository.AddFriends(friendsDto.ConvertToEntity());
    }

    public async Task<FriendsDto> EditFriendsAsUser(int id, EditFriendsDto editFriendsDto, string token)
    {
        var requestUserId = _tokenUtil.GetIdFromToken(token);
        var existingFriends = await _friendsRepository.GetFriendsById(id);
        if (existingFriends is null)
        {
            throw new ArgumentException("No friends with provided id");
        }
        
        switch (requestUserId)
        {
            case var userId when userId == existingFriends.UserIdFk:
                existingFriends.UserFriendshipStatus = editFriendsDto.UserFriendshipStatus;
                break;
            case var userId when userId == existingFriends.FriendIdFk:
                existingFriends.FriendFriendshipStatus = editFriendsDto.FriendFriendshipStatus;
                break;
            default:
                throw new ArgumentException("Denied, cannot edit friendship status for other users");
        }
        
        await _friendsRepository.EditFriends(existingFriends);
        
        return existingFriends.ConvertToDto();
    }
    
    public async Task<FriendsDto> EditFriendsAsAdmin(int id, FriendsDto friendsDto)
    {
        var existingFriends = await _friendsRepository.GetFriendsById(id);
        if (existingFriends is null)
        {
            throw new ArgumentException("No friends with provided id");
        }
        
        UpdatePropertiesIfNeeded(existingFriends, friendsDto, ["FriendsId", "CreatedAt", "UpdatedAt"]);
        await _friendsRepository.EditFriends(existingFriends);
        
        return existingFriends.ConvertToDto();
    }

    public async Task DeleteFriends(int id)
    {
        var existingFriends = await _friendsRepository.GetFriendsById(id);
        if (existingFriends is null)
        {
            throw new ArgumentException("No friends with provided id");
        }

        await _friendsRepository.DeleteFriendsById(id);
    }
    
    private void UpdatePropertiesIfNeeded<T>(Friends friend, T friendsDto, string[] excludedProperties)
    {
        var userProperties = typeof(Friends).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(prop => prop.CanRead && prop.CanWrite && !excludedProperties.Contains(prop.Name));
        var dtoProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(prop => !excludedProperties.Contains(prop.Name));
        
        foreach (var userProp in userProperties)
        {
            var dtoProp = dtoProperties.FirstOrDefault(p => p.Name == userProp.Name && p.PropertyType == userProp.PropertyType);
            if (dtoProp == null) continue;
            
            var sourceValue = dtoProp.GetValue(friendsDto);
            var targetValue = userProp.GetValue(friend);
            
            if (!Equals(sourceValue, targetValue) && sourceValue != null)
            {
                userProp.SetValue(friend, sourceValue);
            }
        }
    }
}