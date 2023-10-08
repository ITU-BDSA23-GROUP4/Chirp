using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;

namespace CustomComparer;

public class CheepViewModelComparer : IEqualityComparer<CheepViewModel> 
{
    public bool Equals(CheepViewModel? first, CheepViewModel? second) 
    {
        if (first != null && second != null)
        {
            if (first.Author !=second.Author)
                return false;

            if (first.Message != second.Message)
                return false;

            if (first.Timestamp != second.Timestamp)
                return false;
            
            return true;
        }
        return false;
    }

    public int GetHashCode(CheepViewModel? obj) {

        if (obj != null)
            if (obj.Author != null && obj.Message != null && obj.Timestamp != null)
                return obj.Author.GetHashCode() + obj.Message.GetHashCode() + obj.Timestamp.GetHashCode();

        return 0;
    }
}
