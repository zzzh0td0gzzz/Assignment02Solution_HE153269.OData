﻿using BusinessObject.Models;
using DataAccess.Intentions;
using eBookStoreAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace eBookStoreAPI.Controllers
{
    public class RolesController : ODataController
    {
        protected readonly IRoleRepository _roleRepository;

        public RolesController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpGet]
        [EnableQuery]
        [Authorize]
        public IQueryable<Role> Get()
        {
            return _roleRepository.GetRoles();
        }

        [EnableQuery]
        [Authorize]
        public SingleResult<Role> Get(int key)
        {
            IQueryable<Role> result = _roleRepository.GetRoleById(key);
            return SingleResult.Create(result);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Post([FromBody] Role role)
        {
            role = await _roleRepository.Create(role);
            return Created(role);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Patch(int key, Delta<Role> role)
        {
            var entity = await _roleRepository.Update(key, role);
            if (entity == null) return NotFound();
            return Updated(entity);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int key)
        {
            var count = await _roleRepository.Delete(key);
            if (count == 0) return NotFound();
            return NoContent();
        }
    }
}
