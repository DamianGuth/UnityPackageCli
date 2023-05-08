using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityPackageCli.Models
{
    /// <summary>
    /// Defines the structure of the config file for packages.
    /// </summary>
    public class UnityPackageConfig
    {
        public List<UnityPackageConfigElement> packages { get; set; } = new ();
    }

    public class UnityPackageConfigElement
    {
        public string? name { get; set; }
        public string? outputpath { get; set; }

        public List<string> directories { get; set; } = new List<string> ();
    }
}
