using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;

using Mono.Cecil;
using Xamarin.Tests;
using Xamarin.Utils;

namespace Cecil.Tests {
	[TestFixture]
	public class ApiCapitalizationTest {

		bool NotObsolete (MethodDefinition mi)
		{

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
		public void PropertiesCapitalizationTest (string assemblyPath)
		{
			Func<TypeDefinition, IEnumerable <object>> selectLambda = (type) => {
				var typeName = type.Name;
				var c = type.Properties.Where (p => p.GetMethod?.IsPublic == true || p.SetMethod?.IsPublic == true).Where (p=>!(Char.IsUpper (p.Name [0]))).Select (p => new { typeName, p.Name });
				return c;
			};
			CapitalizationTest (assemblyPath, selectLambda);
		}

		[TestCaseSource (typeof (Helper), nameof (Helper.PlatformAssemblies))]
		[TestCaseSource (typeof (Helper), nameof (Helper.NetPlatformAssemblies))]
		[Test]
		public void MethodsCapitalizationTest (string assemblyPath)
		{
			Func<TypeDefinition, IEnumerable<object>> selectLambda = (type) => {
				var typeName = type.Name;
				var c = from m in type.Methods
						where m.IsPublic && NotObsolete (m) && !(Char.IsUpper (m.Name [0]))
						select new { typeName, m.Name }; 
				return c;
			};
			CapitalizationTest (assemblyPath, selectLambda);
		}

		[TestCaseSource (typeof (Helper), nameof (Helper.PlatformAssemblies))]
		[TestCaseSource (typeof (Helper), nameof (Helper.NetPlatformAssemblies))]
		[Test]
		public void EventsCapitalizationTest (string assemblyPath)
		{
			Func<TypeDefinition, IEnumerable<object>> selectLambda = (type) => {
				var typeName = type.Name;
				var c = from e in type.Events 
						where !(Char.IsUpper (e.Name [0]))
						select new { typeName, e.Name};
				return c;
			};
			CapitalizationTest (assemblyPath, selectLambda);
		}

		[TestCaseSource (typeof (Helper), nameof (Helper.PlatformAssemblies))]
		[TestCaseSource (typeof (Helper), nameof (Helper.NetPlatformAssemblies))]
		[Test]
		public void FieldsCapitalizationTest (string assemblyPath)
		{
			Func<TypeDefinition, IEnumerable<object>> selectLambda = (type) => {
				var typeName = type.Name;
				var c = from f in type.Fields
						where f.IsPublic && !(Char.IsUpper (f.Name [0]))
						select new {typeName, f.Name};
				return c;
			};
			CapitalizationTest (assemblyPath, selectLambda);
		}

		public void CapitalizationTest (string assemblyPath, Func<TypeDefinition, IEnumerable<object>> selectLambda)
		{
			var assembly = Helper.GetAssembly (assemblyPath);
			if (assembly is null) {
				Assert.Ignore ($"{assemblyPath} could not be found (might be disabled in build)");
				return;
			}

			// Walk every class/struct/enum/property/method/enum value/pinvoke/event
			foreach (var module in assembly.Modules) {
				foreach (var type in module.Types) {
					var err = selectLambda (type);
					if (err is not null) {
						Assert.AreEqual (0, err.Count (), $"Capitalization Issues Found: {string.Join (";", err)}");
					}

				}
			}

		}
	}
}
