using System;
using System.Configuration;

public class Config
{
    public static int GetMobileLocation()
    {
        try
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["MobileLocation"]);
        }
        catch (Exception)
        {
            throw new Exception("Default location code not set on web config");
        }
    }
}