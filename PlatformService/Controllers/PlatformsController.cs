using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {

        private readonly IPlatformRepository _platformRepository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public PlatformsController(IPlatformRepository repository,
                                   IMapper mapper,
                                   ICommandDataClient commandDataClient,
                                   IMessageBusClient messageBusClient)
        {
            _platformRepository = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;
        }


        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--Getting Platforms....");

            var platformItem = _platformRepository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItem));

        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatformById(int id)
        {
            Console.WriteLine("--Getting PlatformsById....");

            var platformItem = _platformRepository.GetPlatformById(id);

            return platformItem != null ? Ok(_mapper.Map<PlatformReadDto>(platformItem)) : NotFound();

        }

        [HttpPost]
        public async Task<ActionResult<PlatformCreateDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platformModel = _mapper.Map<Platform>(platformCreateDto);

            _platformRepository.CreatePlatform(platformModel);
            _platformRepository.SaveChanges();

            var platformDto = _mapper.Map<PlatformReadDto>(platformModel);

            //send sync message
            try
            {
                await _commandDataClient.SendPlatformToCommand(platformDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not send synchronously: {ex.Message}");
            }

            //send async message
            try
            {
                var platformPublishDto = _mapper.Map<PlatformPublishDto>(platformDto);
                platformPublishDto.Event = "Platform_Published";
                _messageBusClient.PublishNewPlatform(platformPublishDto);


            }
            catch (Exception ex)
            {

                Console.WriteLine($"Could not send async: {ex.Message}");
            }
            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformDto.Id }, platformDto);
        }

    }
}
