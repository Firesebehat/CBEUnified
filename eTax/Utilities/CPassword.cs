using System.Web;
using System.Xml;

//namespace CUSTOR.PasswordPolicy
//{
/// <summary>
///     Summary description for CPassword
/// </summary>
public class PasswordSetting
{
    #region properties

    public int Duration { get; set; }
    public int MinLength { get; set; }
    public int MaxLength { get; set; }
    public int NumsLength { get; set; }
    public int UpperLength { get; set; }
    public int SpecialLength { get; set; }
    public string SpecialChars { get; set; }
    public int LockoutDuration { get; set; }

    #endregion
}

public class CPasswordManager
{
    public static PasswordSetting GetPasswordSetting()
    {
        var xmlDoc = new XmlDocument();
        xmlDoc.Load(HttpContext.Current.Server.MapPath("~/PasswordPolicy/PasswordPolicy.xml"));

        var passwordSetting = new PasswordSetting();

        foreach (XmlNode node in xmlDoc.ChildNodes)
        foreach (XmlNode node2 in node.ChildNodes)
        {
            passwordSetting.Duration = int.Parse(node2["duration"].InnerText);
            passwordSetting.MinLength = int.Parse(node2["minLength"].InnerText);
            passwordSetting.MaxLength = int.Parse(node2["maxLength"].InnerText);
            passwordSetting.NumsLength = int.Parse(node2["numsLength"].InnerText);
            passwordSetting.SpecialLength = int.Parse(node2["specialLength"].InnerText);
            passwordSetting.UpperLength = int.Parse(node2["upperLength"].InnerText);
            passwordSetting.SpecialChars = node2["specialChars"].InnerText;
        }

        return passwordSetting;
    }
}

//}