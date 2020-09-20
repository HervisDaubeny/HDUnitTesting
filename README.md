# HD Unit Testing

## Pouziti
user s i vytvori novy projekt pro unit testovani, prida si HDUnitLibrary a preda ji referenci na testovaci projekt


## knihovna HDUnitLibrary:

### cast spousteci (zavadec unit testu)
1) najdi vsechny [HDTestClass] v CallingAssembly -> List<Type> TestClassList
2) najdi v kazde takove tride vsechny [HDTestMethod] -> List<List<MethodInfo>> TestMethodNestedList
3) Nad kazdou tridou proved analyzu...
  a) je treba vytvaret instanci? -> [ClassConstructorAttribute], [GenericClassConstructorAttribute]
  b) projdi vsechny metody a najdi zavislosti -> [RunAfter]

### cast pro uzivatele (aby si mohl napsat unit testy)

1) definice vlastnich test atributu `testMethod` `testClass`
2) vlastni `static class Assert` (pro provadeni testu)
