# Expecto Locator Sample

This repository is meant to demonstrate the experimental FSharp.Compiler.Service-based code location for expecto.

This is needed because updating the test location involves changes both to Expecto and to YoloDev.Expecto.TestSdk


[Expecto code location branch](https://github.com/haf/expecto/tree/ast-location-discovery)
[YoloDev.Expecto.TestSdk branch]()

This repository consumes the experimental branches of these libraries as local nuget packages in the [local-packages folder](./local-packages/)


## What's changed?

In short, many more test types will have code locations across IDEs.

Expecto's current code location depend on reflection and can only locate a very small portion of expecto's test methods mostly testCase, ptestCase, ftestCase, and testParam.
Async-based and task-based tests are all unsupported as well as many other test types like testTheory, testProperty, all the test builders (i.e. `test "name" {}`).

This repository takes nojaf's test location work from Ionide and packages it as a library that can be consumed by YoloDev.Expecto.TestSdk.
Providing test locations through the test adapter allows them to be uniformly consumed by different IDEs (visual studio, Rider, dotnet test, Ionide).
This brings other IDEs closer to the expecto experience with Ionide


## Remaining challenges


- C# is not supported
- TopLevelDefaults works, but feels weird to set if you don't have a main
- testParam and testFixture are unsupported. They'll require a separate syntax matching approach since the actual cases are tuples in a list.
- etestProperty and test methods where the test name isn't the first parameter aren't currently supported. This should be solvable.
- It's unclear if testList and testTheory can be supported via YoloDev currently. nojaf's locator can find them, but but VSTest isn't great about handling hierarchy. They probably could be supported if we ever move to pure Microsoft.Testing.Platform.


## Feedback Requests

Q: What's appropriate for the TestLocator signature? 
Currently it's `type TestLocator = System.Reflection.Assembly -> SourceFilePath -> FlatTest -> SourceLocation option`. 

SourceFilePath could be avoided, but it's available from the testAdapter. It allows us to avoid guessing where the project is located and avoid merging [Ionide.ProjInfo](https://www.nuget.org/packages/Ionide.ProjInfo) into our assembly. It also allows us to avoid parsing the whole project from the first request.

FlatTest might not work for supporting testList, but Test is hierarchical. I don't think we'd want to pass in a whole test hierarchy or require a sudo-valid test.


Q: How do people feel about the `TopLevelDefaults` module?
It allows a sort of plugin experience for libraries while keeping state top-level.

I imagine this module eventually helping us get away from our global static dependence on Expecto.Impl.logger and replace it with all state being specified at top level, but allowing top-level methods that don't require the user to specify all the configuration by composing the static state.

This could also help us de-privledge FsCheck (and future runner extensions), removing the FsCheckConfig from the core library and making FsCheck a true extension. 
That's an ambitious project, but it could make Fable compatibility a plausible option.


Q: Feedback on key names?

I've been flailing at this problem long enough I probably can't see the forest for the trees.
Do the key namespaces and modules make sense.

- Expecto.TestLocator.CompilerService
- Expecto.TestLocator.CompilerService.FCSTestLocator
- Expecto.TestLocator.CompilerService.FCSTestLocator.testLocator
- TopLevelDefault