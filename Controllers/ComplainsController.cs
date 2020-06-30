using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Models;
using System_Back_End.Repositories;
using System_Back_End.Models;
using AutoMapper;

namespace System_Back_End.Controllers
{
    [Route("api/complains")]
    [ApiController]
    public class ComplainsController : ControllerBase
    {
        private IComplainsRepository _complainsRepository { get; }

        private IMapper _mapper { get; }

        public ComplainsController(IComplainsRepository complainsRepository,IMapper mapper)
        {
            _complainsRepository = complainsRepository;
            _mapper = mapper;
        }

        // GET: api/Complains
        [HttpGet]
        public async Task<IActionResult> GetComplains()
        {
            return Ok(await _complainsRepository.GetAll().ToListAsync());
        }

        // GET: api/Complains/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetComplain(string id)
        {
            var complain = await _complainsRepository.GetById(id);

            if (complain == null)
            {
                return NotFound();
            }

            return Ok(complain);
        }

        // PUT: api/Complains/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComplain(string id, Complain complain)
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

        // POST: api/Complains
        [HttpPost]
        public async Task<IActionResult> PostComplain(ComplainToAddModel model)
        {
            var complain = _mapper.Map<Complain>(model);
            complain.Id = Guid.NewGuid().ToString();
            await _complainsRepository.Add(complain);

            return CreatedAtAction("GetComplain", new { id = complain.Id }, complain);
        }

        // DELETE: api/Complains/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Complain>> DeleteComplain(string id)
        {
            var complain = await _complainsRepository.Delete(id);
            if (complain==null)
            {
                return NotFound();
            }

            return complain;
        }
    }
}
