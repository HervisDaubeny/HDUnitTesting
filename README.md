# body (co implementovat)

## knihovna HD_unit_testing:

### cast spousteci (zavadec unit testu)

1) potrebuje ref na `testClass` definovanou uzivatelem
2) je konzolova aplikace, ktera jako svuj konstruktor pozere `testClass` vytvorenou uzivatelem
3) potom potrebuje tento `testClass` prolezt (*reflection*) a zavolat (*paralalismus*) metody oznacene jako `testMethod`

### cast pro uzivatele (aby si mohl napsat unit testy)

1) definice vlastnich test atributu `testMethod` `testClass`
2) vlastni `static class Assert` (pro provadeni testu)
3) prozkoumat vyuziti Fakeru, ktere umozni testovani objektu nahodnymi daty
   coz pri 

## priklad fungovani:
``` cs
/// users current namespace
namespace SomeApp {

    using HD_Unit_Testing;

    [TestClass]
    /// users class where he creates desired unit tests
    public class UserDefinedTestClass {
        [TestMethod]
        public void Test1() { }
        [TestMethod]
        public void Test2() { }
        [TestMethod]
        public void Test3() { }
        [TestMethod]
        public void Test4() { }
    }

    /// users default class containing Main()
    public class Program {

        /// programs entry point
        public void Main(string[] args) {
            UhlikTest.Run(new UserDefinedTestClass());
        }
    }
}
```

``` cs
/// namespace of my unit testing library
namespace HD_Unit_Testing {

    /// entry point of unit my unit tests
    public class UhlikTest {

        public UhlikTest() {
            // some important constructor
        }

        public static void Run(object o) {
            // if o has [TestClass]
            // find all methods with [TestMethod] and run them
        }
    }

    /// my own testClass attribute
    public class HDTestClass : Attribute {
        // implements Attribute interface
    }

    /// my own testMethod attribute
    public class HDTestMethod : Attribute {
        // implements Attribute interface
    }

    /// my implementation of Assert class (used for test results evaluation)
    public static class Assert {
        // some nice implementation
    }
}
```
