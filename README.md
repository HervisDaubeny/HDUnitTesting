# HD Unit Testing - dokumentace k projektu

## Účel
Program slouží jako zápočtová práce pro předměty *NPRG035*, *NPRG038* a *NPRG057* na MFF UK.

---

## Zadání
Rád bych napsal software pro vlastní unit testy s myšlenkou, že si z těch existujících vezmu ty funkce,  
které se mi líbí a budu je implementovat. Primárně to znamená:  
- ### vlastní atributy
  - nejen klasické TestClass a Method, ale i něco zajímavějšího, jako možnost předání dat pro konstruktor [*]
  - možnost testovat generické metody
  - testovat na jedno spuštění více dat (jako by bylo spuštěno vícekrát)
  - možnost kontrolovat kdy (po jaké metodě) se má testovat aktuální metoda, ...
- ### vlastní třídu Assert
- ### vlastní výjimky
- ### rozsáhlé použití reflection
- ### zavaděč testů
- ### vlastní implementaci, bez využití nějakého testovacího frameworku nebo SDK
chci aby šlo mé testy používat podobně jako ty existující, tedy potom co uživatel
napíše svůj projekt / nějakou jeho část a pomocí mojí knihovny pro testy si vytvoří testovací projekt, bude způsob, jak by svoje testy spustil a byl mu oznámen jejich výsledek

---

## Použití
Po té co uživatel dopíše kód, který by rád testoval, vytvoří si nový testovací projekt. Do tohoto projektu naimportuje mou knihovnu ```HDUnitLibrary.dll``` a nastaví mu referenci na kód, který chce otestovat.  
Je důležité aby všechny testovací entity byly označeny pomocí ```HDTest***Attribute```, protože jinak je tento software nedetekuje. 

---
Zdokumentovaný seznam těchto attributů je k dispozici zde:
> https://github.com/HervisDaubeny/HDUnitTesting/wiki/Attributes

Pro testování kódu je uživateli k dispozici rozsáhla statická třída ```HDAssert```, která je zdokumntována zde:
> https://github.com/HervisDaubeny/HDUnitTesting/wiki/Assert
---

Dalším krokem je, vytvořit v testovacím projektu *entry poing* pro testy, v podobě metody, která zinicializuje ```HDTester``` a otevře pro uživatele ovládací konzoli.
```cs
public static void Main(string[] args) {
   HDTester.InitTests();
}
```
```HDTester``` si uloží referenci na assembly, ze které byla zavolána jeho metoda ```InitTests()```, díky které pak pomocí reflection získává oattributované testy. Uživateli se představí konzole s jednoduchým shell-like prostředím, sdělí mu, jak získá pomoc ohledně dostupných příkazů a čeká na jeho vstup.
Příklad takové interakce vypadá takto:
```
Welcome to HDUnitTesting. For list of commands type 'help' in the console.
To exit input newLine.
>help
Command 'help'
Usage:  help
Prints this help list.
==========================================
Command 'run_tests'
Usage:  run_tests [OPTION]... [NAMES]...
Run all tests on default.
Options and Names specify which tests to run.
Example usage:  run_tests -c:ClassName --namespace:MyNamespace MyTestMethod1 MyTestMethod2

Options having ':' must have name argument right after.
  -c:,  --class:           specify class searched for test methods
  -n:,  --namespace:       specify namespace searched for test classes
  -r,   --repeat           run same tests ran in the last test run
  -f,   --failed           run tests that failed in the last test run
  -p,   --passed           run tests that failed in the last test run
  -u,   --unruned          run tests that were not ran in the last test run
  -m,   --multithread      run tests in multiple threads
==========================================
Command 'run_test'Usage:  run_test [OPTION]... <NAME>...
Run single test. Options specify where to look for the test method.
Example usage:  run_test --class:ClassName -n:MyNamespace MyTestMethod3

Options having ':' must have name argument right after.
  -c:,  --class:           specify class searched for test methods
  -n:,  --namespace:       specify namespace searched for test classes
==========================================
>run_tests -c:MyTestClass -m
```

Po zavolání, kteréhokoli z run_* příkazů se vypíše výsledek všech spuštěných testů a případné informace o invalidním vstupu.

Konzoli uživatel může ukončit zadáním prázdného řádku, případně provést nějaké úpravy kódu, rekompilovat projekt a může se testovat dál.  
Pro příjemné spouštění stejných testů jako v minulém běhu je uživateli k dispozici přepínač:
```
>run_tests -r
```

---

## Flow

Po zadání příkazu run_* uživatelem dojde k vytvoření objektu ```Command``` a spustí se analýza uložené volající assembly.

Nejprve se pomocí reflection získají všechny testovací třídy ```GetClasses(...)```, které nesou attribut ```HDTestClassAttribute```. Potom se provede kontrola, zda ```command``` obsahuje požadavky na *namespace* nebo *class* a pokud ano, získané typy se dle těchto požadavků vyfiltrují.

Ve výsledku předchozího procesu se pak hledají samotné testovací metody. To jsou metody, které nesou attribute ```HDTestMethodAttribute```. Ty jsou pak podrobeny filtraci podobně, jako třídy. Postup je následující:  
1) Pokud uživatel nezadal ani jeden z přepínačů, který používá výsledky předchozího spuštění testů, najde program všechny metody, které si uživatel přeje spustit a vyvtoří z nich pole ```TestProcess```ů, které slouží pro spuštění testů
2) Pokud si uživatel vybral některý z těchto přepínaču, je seznam metod které si uživatel přeje spustit (prázdný či nikoli) ignorován a pro filtraci metod ke spuštění jsou použity deserializované výsledky předchozího běhu.

Výstup do konzole je přesměrován do souboru, aby nezaplňoval kontrolní konzoli. Cesta k souboru je:
> ```.../TestProject/bin/Debug/netcoreapp3.1/tests_output.txt```

Jako poslední bod před samotným spuštěním dojde ke kontrole commandu na přítomnost přepínače **-m**, který řídí, zda se testy budou volat synchronně nebo ve více vláknech.

Po doběhnutí všech testů je výstup přesměrován zpět do konzole, jejich výsledky jsou vypsány a následně serializovány do ```.json``` souboru:
> ```.../TestProject/bin/Debug/netcoreapp3.1/testResult.json"```

---

## Závěr

Výsledkem je jednoduchý software pro vytváření a spouštění unit testů, ovládací konzole podobná shellu a vlastní implementace třídy ```HDAssert```, kde je u některých metod použito *maximální testování* - tedy snaha získat co nejvíce informací o chybách. Například při kontole rovnosti dvou kolekcí, neskončím procházení při první neshodě, ale najdu jich co nejvíc, než vyvolám výjimku.

Na projektu jsem se hodně naučil. Obecně to se jedná o můj zatím nejdelší C# kód a objevil jsem díky němu několik věcí, které považuji za krásné. Konkrétně se bavíme hlavně o Linqu, který mi neskutečně usnadnil život. Dále mě zaujala reflection a C# vlastní thread pool. I samotné googlení a řešení problémů bylo přínosné a mnohdy vyloženě úlevné, když jsem konečně zjistil jak mám něco udělat.

Za největší nevýhodu tohoto projektu považuji své konzolové ovládání. Sám jsem sice nadšeným uživatelem shellu a Archlinuxu, ale musím uznat, že ```TextExplorer```u, který je vlastní visual studiu nemohu konkurovat. Potenciální rozšíření projektu do budoucna by pak molo být grafické rozhraní místo konzolového, které by se kromě funkčnosti snažilo poskytnout uživateli co nejpříjemnější prostředí.