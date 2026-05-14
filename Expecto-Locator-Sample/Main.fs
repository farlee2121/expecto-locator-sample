module Expecto_Locator_Sample
open Expecto

TopLevelDefaults.testLocator <- Expecto.TestLocator.CompilerService.FCSTestLocator.testLocator
[<EntryPoint>]
let main argv =
    Tests.runTestsInAssemblyWithCLIArgs [] argv
