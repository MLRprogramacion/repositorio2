using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver;

public static class LenguajeRequestHandler{
    public static IResult ListarRegistros(string idCategoria){
        var filterBuilder = new FilterDefinitionBuilder<LenguajeDbMap>();
        var filter = filterBuilder.Eq(x => x.IdCategoria, idCategoria);

        BaseDatos bd = new BaseDatos();
        var coleccion = bd.ObtenerColeccion<LenguajeDbMap>("Lenguaje");
        var lista = coleccion.Find(filter).ToList();

        return Results.Ok(lista.Select(x => new {
            Id = x.Id.ToString(),
            IdCategoria = x.IdCategoria,
            Titulo = x.Titulo,
            Descripcion = x.Descripcion,
            EsVideo = x.EsVideo,
            Url = x.Url
        }).ToList());
    } 

    public static IResult CrearRegistro(LenguajeDTO dto){
        string errores = "";
        if(string.IsNullOrWhiteSpace(dto.Titulo)){
            errores += "El es título requerido. "+ Environment.NewLine;
        }

        if(string.IsNullOrWhiteSpace(dto.Descripcion)){
            errores += "La descripción es requerida. "+ Environment.NewLine;
        }

        if(string.IsNullOrWhiteSpace(dto.Url)){
            errores += "El url es requerido. "+ Environment.NewLine;
        }

        if(string.IsNullOrWhiteSpace(dto.IdCategoria)){
            errores += "El IdCategoría es requerido. "+ Environment.NewLine;
        }
        if(!string.IsNullOrWhiteSpace(errores)){
            return Results.BadRequest(errores);
         }
    

        if(!ObjectId.TryParse(dto.IdCategoria, out ObjectId idCategoria)){
            return Results.BadRequest($"El Id de la categoria ({dto.IdCategoria}) no es válido");
        }
         

        BaseDatos bd = new BaseDatos();

        var filterBuilderCategorias = new FilterDefinitionBuilder<CategoriaDBMap>();
        var filterCategoria = filterBuilderCategorias.Eq(x => x.Id, idCategoria);
        var coleccionCategoria = bd.ObtenerColeccion<CategoriaDBMap>("Categorias");
        var categoria = coleccionCategoria.Find(filterCategoria).FirstOrDefault();

        if(categoria == null){
            return Results.NotFound($"No existe una categoria con ID = '{dto.IdCategoria}'");
        }

        LenguajeDbMap registro = new LenguajeDbMap();
        registro.Titulo = dto.Titulo;
        registro.EsVideo = dto.EsVideo;
        registro.Descripcion = dto.Descripcion;
        registro.Url = dto.Url;
        registro.IdCategoria = dto.IdCategoria;

        var coleccionLenguaje = bd.ObtenerColeccion<LenguajeDbMap>("Lenguaje");
        coleccionLenguaje!.InsertOne(registro);

        return Results.Ok(registro.Id.ToString());
    }

    public static IResult Eliminar(string id){
        if(!ObjectId.TryParse(id, out ObjectId idLenguaje)){
            return Results.BadRequest($"El Id proporcionado ({id}) no es válido");
       } 

       BaseDatos bd = new BaseDatos();
       var filterBuilder = new FilterDefinitionBuilder<LenguajeDbMap>();
       var filter = filterBuilder.Eq(x => x.Id, idLenguaje);
       var coleccion = bd.ObtenerColeccion<LenguajeDbMap>("Lenguaje");
       coleccion!.DeleteOne(filter);

       return Results.NoContent();
    }
     public static IResult Buscar(string texto){
        var queryExpr = new BsonRegularExpression(new Regex(texto, RegexOptions.IgnoreCase));
        var filterBuilder = new FilterDefinitionBuilder<LenguajeDbMap>();
        var filter = filterBuilder.Regex("Titulo", queryExpr) |
            filterBuilder.Regex("Descripcion", queryExpr);

        BaseDatos bd = new BaseDatos();
        var coleccion = bd.ObtenerColeccion<LenguajeDbMap>("Lenguaje");
        var lista = coleccion.Find(filter).ToList();

        return Results.Ok(lista.Select(x => new {
            Id = x.Id.ToString(),
            IdCategoria = x.IdCategoria,
            Titulo = x.Titulo,
            Descripcion = x.Descripcion,
            EsVideo = x.EsVideo,
            Url = x.Url
        }).ToList());
}}