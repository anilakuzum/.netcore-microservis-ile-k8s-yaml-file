using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsControllers : Controller
    {
        private readonly ICommandRepo _repo;
        private readonly IMapper _mapper;

        public CommandsControllers(ICommandRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEquatable<CommandReadDto>> GetCommandForPlatform(int platformId)
        {
            Console.WriteLine("GetCommandForPlatform");
            if (_repo.PlatformExits(platformId))
            {
                return NotFound();
            }

            var commands = _repo.GetCommandsForPlatform(platformId);

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto createDto)
        {
            Console.WriteLine("CreateCommandForPlatform");
            if (_repo.PlatformExits(platformId))
            {
                return NotFound();
            }

            var commands = _mapper.Map<Command>(createDto);

            _repo.CreateCommand(platformId,commands);
            _repo.SaveChanges();

            var commandreadDto = _mapper.Map<CommandReadDto>(commands);
            return CreatedAtRoute(nameof(GetCommandForPlatform),new { platformId=platformId, commandId=commandreadDto.Id},commandreadDto);
        }
    }
}
