using CommandService.Models;
using System.Collections.Generic;

namespace CommandService.Data
{
    public interface ICommandRepo
    {
        bool SaveChanges();

        //Platforms
        IEnumerable<Platform> GetAllPlatforms();
        void CreatePlatform(Platform platform);
        bool PlatformExits(int platformId);
        bool ExternalPlatformExist(int externalPlatformId);


        //Commands
        IEnumerable<Command> GetCommandsForPlatform(int platformId);
        Command GetCommand(int pltformId, int commandId);
        void CreateCommand(int platformId, Command command);

    }
}
