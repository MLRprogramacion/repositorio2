using MongoDB.Driver;
public static class RegistroRequestHandler{
    public static IResult Registrar(DatosRegistro datos){
        string errores = "";
        if(string.IsNullOrWhiteSpace(datos.EmailId)){
            errores += "El E-mail ID es requerido. "+ Environment.NewLine;

        } 
        if(string.IsNullOrWhiteSpace(datos.Nombre)){
            errores += "El nombre es requerido. "+ Environment.NewLine;

        }
        if(string.IsNullOrWhiteSpace(datos.Contrasenia)){
            errores += "La contraseña es requerida. "+ Environment.NewLine;
        }

        if(!string.IsNullOrWhiteSpace(errores)){
            return Results.BadRequest(errores);
        }

        BaseDatos bd = new BaseDatos();
        var coleccion = bd.ObtenerColeccion<DatosRegistro>("Usuarios");
        if(coleccion == null){
            throw new Exception("No existe la colección Usuarios");
        }

        FilterDefinitionBuilder<DatosRegistro> filterBuilder = new FilterDefinitionBuilder<DatosRegistro>();
        var filter = filterBuilder.Eq(x => x.EmailId, datos.EmailId);

        DatosRegistro? usuarioExistente =coleccion.Find(filter).FirstOrDefault();
        if(usuarioExistente != null){
            return Results.BadRequest($"Ya existe un usuario con el correo {datos.EmailId}");
        }

        coleccion.InsertOne(datos);
        return Results.Ok();
    }
}