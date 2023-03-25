using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CommandService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private ICommandRepo _repo;
        private IMapper _mapper;

        public PlatformsController(ICommandRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms(int plaformId)
        {
            Console.WriteLine("GetPlatforms");
            var plaformItems = _repo.GetAllPlatforms();

            if (!_repo.PlatformExits(plaformId))
            {
                return NotFound();
            }

            var commands = _repo.GetCommandsForPlatform(plaformId);

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpPost]
        public ActionResult TestInbondConnection()
        {
            Console.WriteLine("--->Inbound Test <---");
            return Ok("Inbound test Platform Controller");
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int plaformId, int commandId)
        {
            Console.WriteLine("GetCommandForPlatform");

            if (!_repo.PlatformExits(plaformId))
            {
                return NotFound();
            }

            var commands = _repo.GetCommand(plaformId, commandId);

            if (commands == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }
    }
}
