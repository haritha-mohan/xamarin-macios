using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;
//using ObjCRuntime;
#if MONOMAC
using AppKit;
#else
//using UIKit;
#endif
//using Foundation;
using Mono.Cecil;
using Xamarin.Tests;
using Xamarin.Utils;

namespace Cecil.Tests {
	[TestFixture]
	public class ApiCapitalizationTest {

		bool NotObsolete (MethodDefinition mi)
		{
			//Console.WriteLine ("Method Name: ", mi.Name);
			//Console.WriteLine ("Method Public: ", mi.IsPublic);
			if (mi == null)
				return false;

			if (mi.HasCustomAttributes) {
				foreach (var ca in mi.CustomAttributes) {
					switch (ca.AttributeType.Name) {
					case "ObsoleteAttribute":
						return false;
					}
				}
			}
			return true;
		}

		[TestCaseSource (typeof (Helper), nameof (Helper.PlatformAssemblies))]
		[TestCaseSource (typeof (Helper), nameof (Helper.NetPlatformAssemblies))]
		[Test]
		public void CapitalizationTest (string assemblyPath)
		{
			var assembly = Helper.GetAssembly (assemblyPath);
			if (assembly == null) {
				Assert.Ignore ("{assemblyPath} could not be found (might be disabled in build)");
				return; 
			}

			foreach (var type in assembly.MainModule.Types) {

				var errMethods =
					(from m in type.Methods
					where m.IsPublic && NotObsolete (m) && !(Char.IsUpper (m.Name [0]))
					select m).ToArray();

				var errProperties =
					from p in type.Properties
					where p.GetMethod.IsPublic || p.SetMethod.IsPublic
					where !(Char.IsUpper (p.Name [0]))
					select p;

				var errEvents =
					from e in type.Events //how to check if event is public
					where !(Char.IsUpper (e.Name [0]))
					select e;

				var errFields =
					from f in type.Fields
					where f.IsPublic && !(Char.IsUpper (f.Name [0]))
					select f;


				//Console.WriteLine ("methods are errs", errMethods);

				if (errMethods is not null) {
					foreach (MethodDefinition m in errMethods) {
						Assert.Fail ("This method is incorrectly capitalized: ", $"{m.Name} is name");
					}
				}

				if (errProperties is not null) {
					foreach (PropertyDefinition p in errProperties) {
						Assert.Fail ("This property is incorrectly capitalized: ", $"{p.Name} is name");
					}
				}

				Assert.Pass ("test pass");

			}

		}
	}
}
