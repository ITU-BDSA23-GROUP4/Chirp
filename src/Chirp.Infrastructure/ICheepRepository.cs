using System.Collections.Generic;
using System.Threading.Tasks;
using Chirp.Infrastructure;

namespace Chirp.Core
{
    public interface ICheepRepository
    {
        void AddCheep(int authorId, string text);
        List<CheepDTO> GetCheeps(int? pageNum);
        List<CheepDTO> GetCheepsFromAuthor(string author, int? pageNum);
        int GetCountOfAllCheeps();
        int GetCountOfAllCheepFromAuthor(string author);
        void Create(CheepCreateDTO cheep);
        public void DeleteCheepsFromAuthor(int authorid);
        
    }
}
