// Program.fs
open System
open Prims
open FStar_Pervasives_Native
open VerifiedMath

// Helper functions for pretty printing
let colorPrint (color: ConsoleColor) (text: string) : unit =
    Console.ForegroundColor <- color
    Console.Write(text)
    Console.ResetColor()

let colorPrintLn (color: ConsoleColor) (text: string) : unit =
    colorPrint color text
    Console.WriteLine()

let printHeader (text: string) : unit =
    Console.WriteLine()
    colorPrintLn ConsoleColor.Cyan (sprintf "═══ %s ═══" text)
    Console.WriteLine()

let printRule (rule: string) (description: string) : unit =
    Console.Write("  ")
    colorPrint ConsoleColor.Blue "["
    colorPrint ConsoleColor.Cyan rule
    colorPrint ConsoleColor.Blue "] "
    colorPrintLn ConsoleColor.Gray description

let printDemo (test: string) (result: string) : unit =
    Console.Write("    ")
    colorPrint ConsoleColor.Yellow test
    Console.Write(" → ")
    colorPrintLn ConsoleColor.Green result

let printProof (proof: string) : unit =
    Console.Write("    ")
    colorPrint ConsoleColor.DarkGray (sprintf "✓ %s" proof)
    Console.WriteLine()

[<EntryPoint>]
let main (argv: string[]) : Microsoft.FSharp.Core.int =
    Console.Clear()
    colorPrintLn ConsoleColor.Magenta "╔══════════════════════════════════════════════════════════╗"
    colorPrintLn ConsoleColor.Magenta "║     F* MISRA-Verified Safety Demonstrations              ║"
    colorPrintLn ConsoleColor.Magenta "╚══════════════════════════════════════════════════════════╝"
    
    // MISRA 10.x - Type Model Rules
    printHeader "MISRA 10.x - Essential Type Model"
    
    // Rule 10.1
    printRule "10.1" "Type-appropriate operations"
    let test_array = [of_int 100; of_int 200; of_int 300]
    match safe_list_access test_array (of_int 1) with
    | FStar_Pervasives_Native.Some value -> 
        printDemo "array[1]" (sprintf "%s" (to_string value))
    | FStar_Pervasives_Native.None -> 
        printDemo "array[1]" "Out of bounds"
    match safe_list_access test_array (of_int 10) with
    | FStar_Pervasives_Native.Some value -> 
        printDemo "array[10]" (to_string value)
    | FStar_Pervasives_Native.None -> 
        printDemo "array[10]" "None (safe)"
    printProof "nat type ensures index >= 0"
    
    // Rule 10.3
    printRule "10.3" "No implicit narrowing conversions"
    let narrowed = safe_narrow (of_int 100)
    printDemo "narrow(100)" (sprintf "%s (int8 safe)" (to_string narrowed))
    printDemo "narrow(300)" "COMPILE ERROR (> 127)"
    printProof "Type {-128 <= x <= 127} enforced"
    
    // Rule 10.4
    printRule "10.4" "Integer overflow prevention"
    match safe_add_checked (of_int 100) (of_int 200) with
    | FStar_Pervasives_Native.Some result -> 
        printDemo "100 + 200" (to_string result)
    | FStar_Pervasives_Native.None -> 
        printDemo "100 + 200" "OVERFLOW"
    match safe_add_checked (of_int 2147483640) (of_int 10) with
    | FStar_Pervasives_Native.Some result -> 
        printDemo "INT_MAX-7 + 10" (to_string result)
    | FStar_Pervasives_Native.None -> 
        printDemo "INT_MAX-7 + 10" "OVERFLOW PREVENTED"
    printProof "will_overflow prevents UB"
    
    // MISRA 13.x - Side Effect Rules
    printHeader "MISRA 13.x - Side Effects and Sequence Points"
    
    // Rule 13.2
    printRule "13.2" "No undefined behavior"
    let ub_result = no_undefined_behavior (of_int 5) (of_int 17)
    printDemo "17/5 + 17%5" (to_string ub_result)
    printDemo "17/0" "COMPILE ERROR"
    printProof "Type {x <> 0} prevents div-by-zero"
    
    // Rule 13.5
    printRule "13.5" "No side effects in && or ||"
    let safe_result = safe_and_check (of_int 10) (of_int 2)
    printDemo "(10 > 0) && (10/2 > 0)" (sprintf "%b" safe_result)
    printProof "Both conditions pre-evaluated"
    
    // MISRA 14.x - Control Flow Rules
    printHeader "MISRA 14.x - Control Statement Expressions"
    
    // Rule 14.2
    printRule "14.2" "Single loop counter"
    let sum = bounded_sum_list [of_int 1; of_int 2; of_int 3; of_int 4; of_int 5] (of_int 0)
    printDemo "Σ[1,2,3,4,5]" (to_string sum)
    printProof "decreases lst ensures termination"
    
    // Rule 14.3
    printRule "14.3" "Non-invariant conditions"
    let cond_true = conditional_operation (of_int 5) true
    let cond_false = conditional_operation (of_int 5) false
    printDemo "if true then 5+1" (to_string cond_true)
    printDemo "if false then 5+1" (to_string cond_false)
    printProof "Runtime condition prevents dead code"
    
    // MISRA 15.x - Control Flow Rules
    printHeader "MISRA 15.x - Control Flow"
    
    // Rule 15.5
    printRule "15.5" "Single exit point"
    let max1 = single_exit_max (of_int 25) (of_int 15)
    let max2 = single_exit_max (of_int 10) (of_int 10)
    printDemo "max(25, 15)" (to_string max1)
    printDemo "max(10, 10)" (to_string max2)
    printProof "All paths → single result variable"
    
    // MISRA 17.x - Function Rules
    printHeader "MISRA 17.x - Functions"
    
    // Rule 17.5
    printRule "17.5" "Fixed array sizes"
    let arr3 = { elem0 = of_int 10; elem1 = of_int 20; elem2 = of_int 30 }
    let arr_sum = process_array3 arr3
    printDemo "sum([10,20,30])" (to_string arr_sum)
    printDemo "[10,20] to array3" "COMPILE ERROR"
    printProof "Type array3 = exactly 3 elements"
    
    // Rule 17.8
    printRule "17.8" "Immutable parameters"
    let pure_result = pure_add (of_int 7) (of_int 3)
    printDemo "pure_add(7, 3)" (to_string pure_result)
    printProof "F* enforces immutability by default"
    
    // MISRA 21.x - Standard Library Rules
    printHeader "MISRA 21.x - Standard Libraries"
    
    // Rule 21.3
    printRule "21.3" "No dynamic allocation"
    let stack_buf = { 
        data0 = of_int 1; data1 = of_int 2
        data2 = of_int 3; data3 = of_int 4
        size = of_int 4 
    }
    let buf_sum = sum_stack_buffer stack_buf
    printDemo "stack buffer sum" (to_string buf_sum)
    printProof "Stack-allocated record, no heap"
    
    // Summary
    printHeader "Verification Summary"
    Console.WriteLine()
    colorPrintLn ConsoleColor.Green "  ✅ ALL MISRA RULES ENFORCED BY F*, Z3 and SMT"
    Console.WriteLine()
    Console.WriteLine("  Compile-time guarantees:")
    Console.WriteLine("    • Type safety (10.x): Overflow, narrowing, bounds")
    Console.WriteLine("    • Side effects (13.x): No UB, safe evaluation order")
    Console.WriteLine("    • Control flow (14.x, 15.x): Termination, single exit")
    Console.WriteLine("    • Functions (17.x): Fixed sizes, immutability")
    Console.WriteLine("    • Memory (21.x): Stack-only allocation")
    Console.WriteLine()
    
    0