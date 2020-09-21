# TODO:
- [ ] Complete RunTests method -> run subset of tests, serialization usage
- [ ] Fill list of supported commands for ConsoleControler
- [x] implement RunTest method -> there will be no RunTest method
- [x] implement RunTests -> finished creating TestProcess -> finished RunTests() method
- [x] solve multiple constructors for a TestMethod
      -> solved by getting array of parameters instead of single -> then executing in a cycle
- [x] Is it possible to create instance of generic class using different then parameterless constructor? -> yes but it is not possible to invoke a method on generic type
- [x] Check Cycle dependency and missing prerequisity

# Documentation notes
- when executing method run_tests there are two options
- 1) Default - runs tests specified by Args[] Class[] Namespace[] parameters
  2) remaining - disregard content of Args[] Class[] Namespace[] => runs based on last run => doesn't make sense on first run
