﻿namespace Apps.Asana.Models;

public class AuthData
{
    public string AccessToken { get; set; }
    public string ExpiresIn { get; set; }
    public string RefreshToken { get; set; }
}