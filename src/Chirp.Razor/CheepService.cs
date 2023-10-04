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
        List<CheepViewModel> list = new List<CheepViewModel>();
        int limit = db.GetCheeps().Count > page*32 ?page*32: db.GetCheeps().Count;

        for(int i = 32*(page-1);i <limit ;i++ ){
            list.Add(db.GetCheeps()[i]);
        }
        return list;
    }

    public List<CheepViewModel> GetCheepsFromAuthor(int author)
    {
        List<CheepViewModel> list = new List<CheepViewModel>();
        int limit =  db.GetCheepsByAuthor(author).Count > page*32 ?page*32:  db.GetCheepsByAuthor(author).Count;

        for(int i = 32*(page-1);i <limit ;i++ ){
            list.Add( db.GetCheepsByAuthor(author)[i]);
        }
        return list;
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
}
