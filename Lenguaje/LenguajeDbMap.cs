using MongoDB.Bson;

public class LenguajeDbMap{
    public ObjectId Id {get;set;}
    public string IdCategoria {get;set;}
    public string Descripcion {get;set;}
    public string Titulo {get;set;}
    public bool EsVideo {get;set;}
    public string Url {get;set;}
}