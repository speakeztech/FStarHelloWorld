# F* Hello World with MISRA Safety Verification

[![F* Verification](https://img.shields.io/badge/F*-Verified-green)](https://www.fstar-lang.org/)
[![MISRA Compliant](https://img.shields.io/badge/MISRA-Compliant-blue)](https://www.misra.org.uk/)

## 🎯 What This Project Demonstrates

This project shows how F* has some default code generations hooks to generate mathematically verified F# code that enforces safety-critical programming standards at compile time. When F* successfully extracts to F#, the safety properties are **mathematically proven** - not just tested. These are very basic, simple use cases but they make the point that verified code from F* to F# is practical.

### Key Features

- ✅ **Compile-Time Verification** - All safety checks happen during F* compilation
- ✅ **Zero Runtime Overhead** - Proofs are erased after verification
- ✅ **MISRA-Style Safety Rules** - Industry-standard safety patterns
- ✅ **SMT Solver Integration** - Uses Z3 for mathematical proof checking
- ✅ **Clean F# Output** - Generates idiomatic F# code

## 🚀 Quick Start

### Prerequisites

- [F* (F-star)](https://github.com/FStarLang/FStar) - The verification language
- [Z3 SMT Solver](https://github.com/Z3Prover/z3) - Automatically installed with F*

### Installation

1. Install F* via OPAM:
```bash
opam install fstar
```

Or use Docker:
```bash
docker pull fstarlang/fstar
```

2. Clone this repository:
```bash
git clone https://github.com/yourusername/fstar-hello-world.git
cd fstar-hello-world
```

### Building and Running

1. **Verify and extract F* to F#:**
```bash
fstar.exe --codegen FSharp VerifiedMath.fst --extract VerifiedMath
```

Expected output:
```
Extracted module VerifiedMath
Verified module: VerifiedMath
All verification conditions discharged successfully
```

2. **Run the F# demonstration:**
```bash
dotnet run
```

## 📋 MISRA Safety Rules Demonstrated

The project implements and verifies the following MISRA-C style safety rules:

### Type Safety (10.x)
- **10.1** - Type-appropriate operations with bounds checking
- **10.3** - No implicit narrowing conversions
- **10.4** - Integer overflow prevention

### Side Effects (13.x)
- **13.2** - No undefined behavior (division by zero impossible)
- **13.5** - No side effects in logical operators

### Control Flow (14.x, 15.x)
- **14.2** - Single loop counter with termination proof
- **14.3** - Non-invariant conditional expressions
- **15.5** - Single exit point from functions

### Functions (17.x)
- **17.5** - Fixed array sizes verified at compile time
- **17.8** - Immutable function parameters

### Memory (21.x)
- **21.3** - No dynamic memory allocation (stack only)

## 🔬 How It Works

### 1. Write F* with Safety Specifications

```fstar
(* F* source with refinement types and proofs *)
let safe_divide (x: int) (y: int{y <> 0}) : Tot int =
  x / y  (* Division by zero impossible - proven at compile time *)
```

### 2. F* Verifies and Extracts to F#

```fsharp
// Generated F# - proof erased, safety guaranteed
let safe_divide (x: Prims.int) (y: Prims.int) : Prims.int =
  x / y  // y <> 0 already proven, no runtime check needed
```

### 3. Run with Confidence

The extracted F# code runs at full speed with no safety overhead because all properties were verified mathematically during compilation.

## 🧪 Try Breaking It!

Want to see F* verification in action? Try these simple changes that will cause compile-time errors:

1. **Remove division safety** in `VerifiedMath.fst`:
```fstar
// Change: let safe_and_check (x: int) (y: int{y <> 0}) : Tot bool =
// To:     let safe_and_check (x: int) (y: int) : Tot bool =
```
Result: F* error - "Possible division by zero"

2. **Break termination proof**:
```fstar
// Change: : Tot int (decreases lst) =
// To:     : Tot int (decreases acc) =
```
Result: F* error - "Could not prove termination"

3. **Violate postcondition**:
```fstar
let increment (x: int) : Tot (y:int{y > x}) = 
  x - 1  // Should be x + 1
```
Result: F* error - "Cannot prove y > x"

## 📁 Project Structure

```
FStarHelloWorld/
├── VerifiedMath.fst      # F* source with safety specifications
├── VerifiedMath.fs       # Extracted F# code (generated)
├── Program.fs            # Demo program showing verified functions
├── FStarHelloWorld.fsproj
└── README.md
```

### Key Dependencies

- `ulibfs` - F* runtime library for F#
- `Prims.fs` - F* primitive types
- `FStar_Pervasives_Native.fs` - F* standard library

## 🏗️ Integration with Fidelity Framework

This project serves as a proof-of-concept for the [Fidelity Framework](https://github.com/speakez-llc/fidelity), which aims to bring formal verification to production F# development through:

- **Hypergraph-based compilation** preserving proof obligations
- **MLIR integration** for verified optimization
- **Zero-cost abstractions** with compile-time safety

## 📚 Learn More

- [F* Tutorial](https://www.fstar-lang.org/tutorial/)
- [MISRA C Guidelines](https://www.misra.org.uk/)
- [Z3 SMT Solver](https://github.com/Z3Prover/z3)
- [Fidelity Framework Vision](https://speakez.tech/blog/verifying-fsharp/)

## 🤝 Contributing

Contributions are welcome! Feel free to:

- Add more MISRA rule implementations
- Improve the F* specifications
- Create additional safety demonstrations
- Report issues or suggest enhancements

## 🙏 Acknowledgments

- **F* Team** at Microsoft Research for creating this powerful verification system
- **Z3 Team** for the SMT solver that makes automated proving possible
- **MISRA** for establishing industry-standard safety guidelines
- **F# Community** for excellent functional programming support

---

**Remember:** If the code compiles, the safety properties are mathematically proven. No hope, no tests, just proofs. 🎯
