using UserManagementAPI.Base;

namespace UserManagementAPI.Const
{
    
    public class ResponseErrors{

        //SERVER ERRORS
        public static BaseError ServerInternalError => new BaseError
        {
            Code = "SRV500",
            Description = "An internal server error occurred"
        };
        public static BaseError ServerDataSaveError = new BaseError { Code = "SRVR001", Description = "Internal data error saving" };

        //USER ERRORS
        public static BaseError UserDataExist = new BaseError { Code = "USR001", Description = "It cannot repeat data, names or emails" };
        public static BaseError UserInvalidAttribs = new BaseError { Code = "USR002", Description = "This is not a suitable user to register"};

    }
    
}