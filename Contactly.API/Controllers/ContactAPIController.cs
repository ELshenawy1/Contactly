using AutoMapper;
using Contactly.API.Hubs;
using Contactly.Core.Common;
using Contactly.Core.DTOs;
using Contactly.Core.Entities;
using Contactly.Core.Interfaces;
using Contactly.Core.Specifications;
using Contactly.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;

namespace Contactly.API.Controllers
{
    [Route("api/ContactApi")]
    [ApiController]
    public class ContactAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        private IHubContext<ContactHub> HubContext { get; set; }
        public ContactAPIController(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<ContactHub> hubcontext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new();
            HubContext = hubcontext;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] ContactSpecParams contactParams)
        {
            try
            {
                var spec = new ContactsWithFilterationSpecification(contactParams);
                var countSpec = new ContactWithFiltersForCountSpecification(contactParams);
                var totalItems = await _unitOfWork.Repository<Contact>().CountAsync(countSpec);
                var contacts = await _unitOfWork.Repository<Contact>().ListAsync(spec);
                var data = _mapper.Map<List<ContactDTO>>(contacts);

                _response.SetPagination<ContactDTO>(pageIndex: contactParams.PageIndex,
                                                    pageSize: contactParams.PageSize, 
                                                    totalCount: totalItems,
                                                    data: data);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return BadRequest(_response);
            }
        }

        [Authorize]
        [HttpGet("{id:int}", Name = "GetContact")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetContact(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var contact = await _unitOfWork.Repository<Contact>().GetByIdAsync(id);
                if (contact == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<ContactDTO>(contact);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }  
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [Authorize]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> UpdateContact(int id, [FromBody] ContactUpdateDTO updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.ID)
                {
                    return BadRequest();
                }

                Contact model = _mapper.Map<Contact>(updateDto);


                _unitOfWork.Repository<Contact>().Update(model);
                var result = await _unitOfWork.Complete();
                if (result <= 0) return BadRequest(updateDto);
                _response.StatusCode = HttpStatusCode.NoContent;
                await HubContext.Clients.All.SendAsync("ContactUpdate" , updateDto.PageIndex , model);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            
            return _response;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateContact(ContactCreateDTO createDto)
        {
            try
            {
                if (createDto == null)
                {
                    return BadRequest(createDto);
                }


                Contact contact = _mapper.Map<Contact>(createDto);

                _unitOfWork.Repository<Contact>().Add(contact);
                var result = await _unitOfWork.Complete();
                if (result <= 0) return BadRequest(createDto);

                _response.Result = _mapper.Map<ContactDTO>(contact);
                _response.StatusCode = HttpStatusCode.Created;
                await HubContext.Clients.All.SendAsync("NewContact");
                return CreatedAtRoute("GetContact", new { id = contact.ID }, _response);
            }
            catch (Exception ex)
            {
                    
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            
            return _response;
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteContact(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var contact = await _unitOfWork.Repository<Contact>().GetByIdAsync(id);
                if (contact == null)
                {
                    return NotFound();
                }


                _unitOfWork.Repository<Contact>().Delete(contact);

                var result = await _unitOfWork.Complete();
                if (result <= 0) return BadRequest();

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                await HubContext.Clients.All.SendAsync("DeleteContact",id);

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}
