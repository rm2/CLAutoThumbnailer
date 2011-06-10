using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace CLAutoThumbnailer
    {
    /// <summary>
    /// From C# 4.0 in a Nutshell, Ch 17 Assemblies - "Packing a Single-File Executable", p 676.
    /// </summary>
    public class Loader
        {
        static Dictionary <string, Assembly> _libs = new Dictionary<string, Assembly> ();

        static void Main (string[] args)
            {
            AppDomain.CurrentDomain.AssemblyResolve += FindAssembly;
            CLAutoThumbnailer.Main (args);
            }

        static Assembly FindAssembly (object sender, ResolveEventArgs args)
            {
            string shortName = new AssemblyName (args.Name).Name;
            if (_libs.ContainsKey (shortName))
                return _libs[shortName];
            using (Stream s = Assembly.GetExecutingAssembly ().
                   GetManifestResourceStream ("CLAutoThumbnailer." + shortName + ".dll"))
                {
                byte[] data = new BinaryReader (s).ReadBytes ((int) s.Length);
                Assembly a = Assembly.Load (data);
                _libs[shortName] = a;
                return a;
                }
            }
        }
    }
