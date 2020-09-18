using DogGo.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public interface IWalksRepository
    {
        List<Walks> GetAllWalks();
        List<Walks> GetWalksById(int walkerId);

        public void AddWalk(Walks walk);
        Walks GetWalkByIdDetails(int id);

    }
}
