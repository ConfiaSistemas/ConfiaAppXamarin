using System;
using System.Collections.Generic;
using System.Text;

namespace ConfiApp.Modelos
{

    class User
    {
        public static string nombre = "";
        public User(string nombreU)
        {
            if(App.Current.Properties.ContainsKey("name"))
            {
                var val = App.Current.Properties["name"];
                nombre = val.ToString();
            }
        }
        public string PruebaNombre()
        {
            return nombre;
        }
    }
}
