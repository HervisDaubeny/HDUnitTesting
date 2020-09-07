using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using HDUnit.Attributes;
using System.Linq;
using System.ComponentModel;
using HDUnit;
using HDUnit.Exceptions;
using HDUnit.Extensions;

namespace CustomersProjectTests {
    public class TestRun {
        public static void Main(string[] args) {

            HDTester.InitTests();
            HDTester.RunTest("dummyName");

            // ### LEGACY ###
            var classes = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var cl in classes) {
                if (cl.GetCustomAttribute<HDTestClassAttribute>() is HDTestClassAttribute) {
                    Console.WriteLine(cl.Name);
                    if (cl.Name is nameof(GenericGreeterTests)) {
                        var GenClass = new GenericGreeterTests();


                        var meth = cl.GetMethods();
                        foreach (var met in meth) {
                            var attributes = met.GetCustomAttributes<HDRootAttribute>(inherit: true);
                            foreach (var instance in attributes) {
                                if (instance is HDGenericParametersAttribute attValue) {
                                    MethodInfo myGenericMethod = met.MakeGenericMethod(attValue.Types);
                                    var testName = myGenericMethod.GetName();
                                    try {
                                        myGenericMethod.Invoke(GenClass, attValue.Parameters);
                                        Console.Write($"{testName}  |  ");
                                        Console.WriteLine("Test passed.");
                                    }
                                    catch(TargetInvocationException E) when(E.InnerException is HDAssertFailedException inner) {
                                        Console.Write($"{testName}  |  ");
                                        Console.WriteLine(inner.Message);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
