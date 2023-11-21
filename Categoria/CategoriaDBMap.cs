using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Driver;

public class CategoriaDBMap{
    public string Nombre {get; set;} = string.Empty;

    public string UrlIcono {get; set;} = string.Empty;
    
    public ObjectId Id {get; set;}

}