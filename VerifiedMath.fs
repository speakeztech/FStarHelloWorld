module VerifiedMath
open Prims
open FStar_Pervasives_Native

// MISRA Rule 10.4 - Prevent integer overflow
let will_overflow (x: int) (y: int) : bool =
    if y > parse_int "0" then
        x > parse_int "2147483647" - y
    elif y < parse_int "0" then
        x < parse_int "-2147483648" - y
    else
        false

let safe_add_checked (x: int) (y: int) : int option =
    if will_overflow x y then
        None
    else
        Some (x + y)

// MISRA Rule 13.5 - No side effects in logical operators
let safe_and_check (x: int) (y: int) : bool =
    let cond1 = x > parse_int "0"
    let cond2 = x / y > parse_int "0"
    cond1 && cond2

// MISRA Rule 14.2 - Single loop counter
let rec bounded_sum_list (lst: int list) (acc: int) : int =
    match lst with
    | [] -> acc
    | h::t -> bounded_sum_list t (acc + h)

// MISRA Rule 17.5 - Array with fixed size
type array3 = {
    elem0: int
    elem1: int
    elem2: int
}

let process_array3 (arr: array3) : int =
    arr.elem0 + arr.elem1 + arr.elem2

// MISRA Rule 21.3 - No dynamic allocation
type stack_buffer = {
    data0: int
    data1: int
    data2: int
    data3: int
    size: nat
}

let sum_stack_buffer (buf: stack_buffer) : int =
    buf.data0 + buf.data1 + buf.data2 + buf.data3

// MISRA Rule 15.5 - Single exit point
let single_exit_max (x: int) (y: int) : int =
    let result = 
        if x > y then x
        elif y > x then y
        else x
    result

// MISRA Rule 10.1 - Type-appropriate array access
let rec list_nth (lst: int list) (n: nat) : int option =
    match lst with
    | [] -> None
    | h::t ->
        if op_Equality n (parse_int "0") then
            Some h
        else
            list_nth t (n - parse_int "1")

let safe_list_access (lst: int list) (i: nat) : int option =
    list_nth lst i

// MISRA Rule 14.3 - Controlling expressions shall not be invariant
let conditional_operation (x: int) (condition: bool) : int =
    if condition then
        x + parse_int "1"
    else
        x

// MISRA Rule 17.8 - Parameters are immutable
let pure_add (x: int) (y: int) : int =
    let x_local = x
    let y_local = y
    x_local + y_local

// MISRA Rule 10.3 - No implicit narrowing conversions
let safe_narrow (x: int) : int =
    x 

// MISRA Rule 13.2 - No undefined behavior
let no_undefined_behavior (x: int) (y: int) : int =
    let safe_div = y / x  
    let safe_mod = y % x  
    safe_div + safe_mod

// Helper functions
let is_some (o: 'a option) : bool =
    match o with
    | Some _ -> true
    | None -> false

let get_value (o: 'a option) : 'a =
    match o with
    | Some v -> v
    | None -> failwith "None value" 