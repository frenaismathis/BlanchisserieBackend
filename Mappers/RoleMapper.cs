using BlanchisserieBackend.DTOs;
using BlanchisserieBackend.Models;

namespace BlanchisserieBackend.Mappers
{
    public static class RoleMapper
    {
        public static RoleDto ToRoleDto(Role role)
        {
            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
            };
        }
    }
}
