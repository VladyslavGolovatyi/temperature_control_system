using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsAppLab2.Converting
{
    public static class PermissionConverting
    {
        public static Permission GetPermission(this string permissionString)
        {
            switch (permissionString)
            {
                case "admin":
                    return Permission.admin;
                case "sensor1":
                    return Permission.sensor1;
                case "sensor2":
                    return Permission.sensor2;
                case "report1":
                    return Permission.report1;
                case "report2":
                    return Permission.report2;
                default:
                    throw new ArgumentOutOfRangeException(nameof(permissionString), "No such permission");
            }
        }
    }
}
