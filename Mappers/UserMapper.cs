using BlanchisserieBackend.DTOs;
using BlanchisserieBackend.Models;

namespace BlanchisserieBackend.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Firstname + " " + user.Lastname,
                Email = user.Email,
                Civilite = user.Civilite,
                RoleId = user.RoleId,
                Role = user.Role != null ? RoleMapper.ToRoleDto(user.Role) : new RoleDto()
            };
        }
    }
}
