using CommandService.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private AppDbContext _context;

        public CommandRepo(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public void CreateCommand(int platformId, Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            command.PlatformId = platformId;
            _context.Commands.Add(command);
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            _context.Platforms.Add(platform);


        }

        public bool ExternalPlatformExist(int externalPlatformId)
        {
            return _context.Platforms.Any(p => p.Id == externalPlatformId);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Command GetCommand(int pltformId, int commandId)
        {
            return _context.Commands
                   .Where(w => w.PlatformId == pltformId && w.Id == commandId)
                   .FirstOrDefault();
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            return _context.Commands
             .Where(w => w.PlatformId == platformId)
             .OrderBy(o => o.Platform.Name);
        }

        public bool PlatformExits(int platformId)
        {
            return _context.Platforms.Any(p => p.Id == platformId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
