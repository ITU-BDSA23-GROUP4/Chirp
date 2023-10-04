using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using CheepRecord;
using SQLDB;

//public record Cheep(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    public void AlterPage(int listSize, int change) ;
    public int GetPage();
    
    
    public List<CheepViewModel> GetCheepsFromAuthor(int author);
}

public class CheepService : ICheepService
{
    DB db = DB.GetInstance();

    private int page = 1;
    public int GetPage(){
        return page;
    }
    public void AlterPage(int listSize, int change){
        if ((page+change)>=1 && (page+change)<=(listSize/32+1)){
            page= page+change;    
        }
        
    }
    public List<CheepViewModel> GetCheeps()
    {      
        return db.GetCheeps();
    }

   /*   public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }
 */
    public List<CheepViewModel> GetCheepsFromAuthor(int author)
    {
        return db.GetCheepsByAuthor(author);
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
}
