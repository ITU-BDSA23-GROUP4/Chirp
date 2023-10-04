using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    public void AlterP(int change) ;
    public int GetPage();
    
    
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    // These would normally be loaded from a database for example

    private int page = 1;
    public int GetPage(){
        return page;
    }
    public void AlterP(int change){
        if ((page+change)>=1 && (page+change)<=(_cheeps.Count/32+1)){
            page= page+change;    
        }
        
    } 
    public List<CheepViewModel> GetCheeps()
    {      
        List<CheepViewModel> list = new List<CheepViewModel>();
            int limit = _cheeps.Count() > page*32 ?page*32: _cheeps.Count();
            for(int i = 32*(page-1);i <limit ;i++ ){
                list.Add(_cheeps[i]);
            }
        return list;
    }

   /*   public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }
 */
    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        // filter by the provided author name
        return _cheeps.Where(x => x.Author == author).ToList();
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

    private static readonly List<CheepViewModel> _cheeps = new()
        {   
            new CheepViewModel("Helge", "1, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "2, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "3, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "4, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "5, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "6, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "7, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "8, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "9, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "10, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "11, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "12, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "13, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "14, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "15, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "16, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "17, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "18, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "19, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "20, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "21, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "22, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "23, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", " 24Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "25 Hello, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "26Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "27 Hello, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "28 Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "29 Hello, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "30 Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "31 Hello, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "32 Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "33 Hello, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "34, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "1, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "2, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "3, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "4, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "5, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "6, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "7, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "8, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "9, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "10, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "11, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "12, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "13, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "14, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "15, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "16, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "17, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "18, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "19, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "20, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "21, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "22, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "23, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", " 24Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "25 Hello, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "26Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "27 Hello, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "28 Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "29 Hello, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "30 Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "31 Hello, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "32 Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "33 Hello, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "34, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
        
        };
}
