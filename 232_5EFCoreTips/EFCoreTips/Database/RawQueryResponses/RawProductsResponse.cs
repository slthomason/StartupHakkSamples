using System;

namespace EFCoreTips.Database.RawQueryResponses;

public class RawProductsResponse
{
    public required int productId {get;set;}
    public required string productName {get;set;}
    public int catId {get;set;}
    public bool isActive {get;set;}
}
