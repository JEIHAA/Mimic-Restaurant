//2024-05-22: CUSTOM UNITY TEMPLATE 

public class MemberDTO
{
    public string id = string.Empty;
    public string password = string.Empty;
    public string nickname = string.Empty;

    public MemberDTO(string id, string password, string nickname)
    {
        this.id = id;
        this.password = password;
        this.nickname = nickname; 
    }
}
