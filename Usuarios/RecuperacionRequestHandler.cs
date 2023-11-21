using MongoDB.Driver;
public class RecuperacionRequestHandler{

    public static IResult RecuperarContra(DatosRecuperarContra datos){
         string errores = "";
        if(string.IsNullOrWhiteSpace(datos.EmailId)){
            errores+= "El E-mail ID es requerido. ";

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
        if(usuarioExistente == null){
            return Results. NotFound($"No existe un usuario con el correo proporcionado");
        }
        Correo c = new Correo();
       c.Destinatario = usuarioExistente.EmailId;
            c.Asunto = "Recuperacion de la contraseña";
            c.Mensaje = "Tu contraseña es: "+usuarioExistente.Contrasenia;
            c.EnviarCorreo();

        return Results.Ok("Se envio un correo de recuperacion");
    }
}
