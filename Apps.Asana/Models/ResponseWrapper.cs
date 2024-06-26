﻿namespace Apps.Asana.Models;

public class ResponseWrapper<T>
{
    public T Data { get; set; }
    
    public PaginationInfo? NextPage { get; set; }
}