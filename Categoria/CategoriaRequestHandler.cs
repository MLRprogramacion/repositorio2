using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Driver;

public static class CategoriaRequestHandler{
    public static IResult Crear(DTO datos){
        string errores = "";
        if(string.IsNullOrWhiteSpace(datos.Nombre)){
            errores += "El nombre requerido. "+ Environment.NewLine;
        }

        if(string.IsNullOrWhiteSpace(datos.UrlIcono)){
            errores += "El es url requerido. "+ Environment.NewLine;
        }
        if(!string.IsNullOrWhiteSpace(errores)){
            return Results.BadRequest(errores);
         }


        var filterBuilder = new FilterDefinitionBuilder<CategoriaDBMap>();
        var filter = filterBuilder.Eq(x => x.Nombre, datos.Nombre);

        BaseDatos DB = new BaseDatos();
        var coleccion = DB.ObtenerColeccion<CategoriaDBMap>("Categorias");
        CategoriaDBMap? registro = coleccion.Find(filter).FirstOrDefault();

        if(registro != null){
            return Results.BadRequest($"La categoría '{datos.Nombre}' ya existe en la base de datos");
        }

        registro = new CategoriaDBMap();
        registro.Nombre = datos.Nombre;
        registro.UrlIcono = datos.UrlIcono;

        coleccion!.InsertOne(registro);
        string IDnew = registro.Id.ToString();

        return Results.Ok(IDnew);
    }
    public static IResult Listar(){
        var filterBuilder = new FilterDefinitionBuilder<CategoriaDBMap>();
        var filter = filterBuilder.Empty;

        BaseDatos DB = new BaseDatos();
        var coleccion = DB.ObtenerColeccion<CategoriaDBMap>("Categorias");
        List<CategoriaDBMap> MongoDBList = coleccion.Find(filter).ToList();

        var Lista = MongoDBList.Select(x => new {
            Id = x.Id.ToString(),
            Nombre = x.Nombre,
            UrLIcono = x.UrlIcono
        }).ToList();

        return Results.Ok(Lista);
    }
}