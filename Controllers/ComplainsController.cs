using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fastdo.Core.Models;
using Fastdo.backendsys.Repositories;
using Fastdo.backendsys.Models;
using AutoMapper;

namespace Fastdo.backendsys.Controllers
{
    [Route("api/complains")]
    [ApiController]
    public class ComplainsController : ControllerBase
    {
        #region constructor and properties
        private IComplainsRepository _complainsRepository { get; }

        private IMapper _mapper { get; }

        public ComplainsController(IComplainsRepository complainsRepository,IMapper mapper)
        {
            _complainsRepository = complainsRepository;
            _mapper = mapper;
        }
        #endregion


        #region get
        [HttpGet]
        public async Task<IActionResult> GetComplains()
        {
            return Ok(await _complainsRepository.GetAll().ToListAsync());
        }

        // GET: api/Complains/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetComplain(Guid id)
        {
            var complain = await _complainsRepository.GetByIdAsync(id);

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
                await _complainsRepository.Update(complain);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _complainsRepository.ComplainExists(id))
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
             _complainsRepository.Add(complain);

            return CreatedAtAction("GetComplain", new { id = complain.Id }, complain);
        }
        #endregion

        #region delete
        // DELETE: api/Complains/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Complain>> DeleteComplain(Guid id)
        {
            var complain = await _complainsRepository.Delete(id);
            if (complain==null)
            {
                return NotFound();
            }

            return complain;
        }
        #endregion
    }
}
