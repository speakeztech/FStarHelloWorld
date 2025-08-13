module VerifiedMath

(* Try changing + to - *)
let increment (x: int{x < 2147483647}) : Tot (y:int{y > x}) = 
  x + 1

(* MISRA Rule 10.4 - Prevent integer overflow *)
let will_overflow (x: int) (y: int) : Tot bool =
  if y > 0 then x > 2147483647 - y
  else if y < 0 then x < -2147483648 - y
  else false

let safe_add_checked (x: int) (y: int) : Tot (option int) =
  if will_overflow x y then None
  else Some (x + y)

(* MISRA Rule 13.5 - No side effects in logical operators *)
let safe_and_check (x: int) (y: int{y <> 0}) : Tot bool =
  let cond1 = x > 0 in
  let cond2 = x / y > 0 in
  cond1 && cond2

(* MISRA Rule 14.2 - Single loop counter *)
let rec bounded_sum_list (lst: list int) (acc: int) 
  : Tot int (decreases lst) =
  match lst with
  | [] -> acc
  | h::t -> bounded_sum_list t (acc + h)

(* MISRA Rule 17.5 - Array with fixed size *)
type array3 = {
  elem0: int;
  elem1: int;
  elem2: int
}

let process_array3 (arr: array3) : Tot int =
  arr.elem0 + arr.elem1 + arr.elem2

(* MISRA Rule 21.3 - No dynamic allocation *)
type stack_buffer = {
  data0: int;
  data1: int;
  data2: int;
  data3: int;
  size: nat
}

let sum_stack_buffer (buf: stack_buffer{buf.size = 4}) : Tot int =
  buf.data0 + buf.data1 + buf.data2 + buf.data3

(* MISRA Rule 15.5 - Single exit point *)
let single_exit_max (x: int) (y: int) : Tot int =
  let result = 
    if x > y then x
    else if y > x then y
    else x
  in
  result

(* MISRA Rule 10.1 - Type-appropriate array access *)
let rec list_nth (lst: list int) (n: nat) : Tot (option int) (decreases lst) =
  match lst with
  | [] -> None
  | h::t -> 
    if n = 0 then Some h
    else list_nth t (n - 1)

let safe_list_access (lst: list int) (i: nat) : Tot (option int) =
  list_nth lst i

(* MISRA Rule 14.3 - Controlling expressions shall not be invariant *)
let conditional_operation (x: int) (condition: bool) : Tot int =
  if condition then
    x + 1
  else
    x

(* MISRA Rule 17.8 - Parameters are immutable in F* by default *)
let pure_add (x: int) (y: int) : Tot int =
  let x_local = x in
  let y_local = y in
  x_local + y_local

(* MISRA Rule 10.3 - No implicit narrowing conversions *)
let safe_narrow (x: int{-128 <= x && x <= 127}) : Tot int =
  x  (* Within int8 range, verified at type level *)

(* MISRA Rule 13.2 - No undefined behavior from expression evaluation *)
let no_undefined_behavior (x: int{x <> 0}) (y: int) : Tot int =
  let safe_div = y / x in  (* Can't be undefined, x <> 0 proven *)
  let safe_mod = y % x in  (* Can't be undefined, x <> 0 proven *)
  safe_div + safe_mod

(* Helper for Option type *)
let is_some (o: option 'a) : Tot bool =
  match o with
  | Some _ -> true
  | None -> false

let get_value (o: option 'a{is_some o}) : Tot 'a =
  match o with
  | Some v -> v