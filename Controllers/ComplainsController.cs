using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fastdo.Core.Models;
using Fastdo.API.Repositories;
using Fastdo.Core.ViewModels;
using AutoMapper;
using Fastdo.Core;

namespace Fastdo.API.Controllers
{
    [Route("api/complains")]
    [ApiController]
    public class ComplainsController : ControllerBase
    {
        #region constructor and properties
        private IUnitOfWork _unitOfWork { get; }

        private IMapper _mapper { get; }

        public ComplainsController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion


        #region get
        [HttpGet]
        public async Task<IActionResult> GetComplains()
        {
            return Ok(await _unitOfWork.ComplainsRepository.GetAll().ToListAsync());
        }

        // GET: api/Complains/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetComplain(Guid id)
        {
            var complain = await _unitOfWork.ComplainsRepository.GetByIdAsync(id);

            if (complain == null)
            {
                return NotFound();
            }

            return Ok(complain);
        }

        #endregion

        #region put
        // PUT: api/Complains/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComplain(Guid id, Complain complain)
        {
            if (id != complain.Id)
            {
                return BadRequest();
            }


            try
            {
                await _unitOfWork.ComplainsRepository.Update(complain);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _unitOfWork.ComplainsRepository.ComplainExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        #endregion

        #region post
        // POST: api/Complains
        [HttpPost]
        public IActionResult PostComplain(ComplainToAddModel model)
        {
            var complain = _mapper.Map<Complain>(model);
             _unitOfWork.ComplainsRepository.Add(complain);

            return CreatedAtAction("GetComplain", new { id = complain.Id }, complain);
        }
        #endregion

        #region delete
        // DELETE: api/Complains/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Complain>> DeleteComplain(Guid id)
        {
            var complain = await _unitOfWork.ComplainsRepository.Delete(id);
            if (complain==null)
            {
                return NotFound();
            }

            return complain;
        }
        #endregion
    }
}
